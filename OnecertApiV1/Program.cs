
using IdentityServer4.AccessTokenValidation;
using Microsoft.OpenApi.Models;
using OnecertApiV1;
using OnecertApiV1.Context;
using OnecertApiV1.Middleware;
using OnecertApiV1.Services;
using OnecertApiV1.Services.Implementation;
using OnecertApiV1.Services.Interface;
using OnecertApiV1.Vault;
using System.Net;

var builder = WebApplication.CreateBuilder(args);



ConfigurationManager configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddSingleton<clsData>();
builder.Services.AddSingleton<clsParameters>();
builder.Services.AddSingleton<clsMail>();
builder.Services.AddSingleton<clsSetup>();

// HashiCorp Vault: resolve the SQL User ID/Password from Vault and inject them into the
// existing "SqlConnection" connection string before the app builds, so every existing
// consumer (DapperContext, DataRepo) keeps reading ConnectionStrings:SqlConnection unchanged.
builder.Services.AddHashiCorpVault(configuration);

var vaultOptions = configuration.GetSection(VaultOptions.SectionName).Get<VaultOptions>() ?? new VaultOptions();
if (vaultOptions.Enabled)
{
    using (var vaultBootstrapProvider = builder.Services.BuildServiceProvider())
    {
        var vaultLogger = vaultBootstrapProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Vault.Bootstrap");
        var vaultService = vaultBootstrapProvider.GetRequiredService<IHashiCorpVaultService>();
        try
        {
            var vaultConnectionString = await vaultService.GetSqlConnectionStringAsync();
            configuration[$"ConnectionStrings:{vaultOptions.ConnectionStringName}"] = vaultConnectionString;
            vaultLogger.LogInformation("Successfully retrieved database credentials from HashiCorp Vault.");
        }
        catch (Exception ex)
        {
            vaultLogger.LogCritical(ex, "Failed to retrieve database credentials from HashiCorp Vault. Application will not start.");
            throw;
        }
    }
}
else
{
    Console.WriteLine("HashiCorp Vault integration disabled (Vault:Enabled=false); using ConnectionStrings:SqlConnection from configuration as-is.");
}

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IDataRepo, DataRepo>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder
           .AllowAnyOrigin() // Allow requests from any origin
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
}).AddIdentityServerAuthentication(options =>
{

    options.Authority = configuration.GetValue<string>("OpenId:Authority");
    options.RequireHttpsMetadata = true;
    options.ApiSecret = configuration.GetValue<string>("OpenId:AppSecret");
    options.ApiName = configuration.GetValue<string>("OpenId:ApiName");
    options.JwtBackChannelHandler = IdentityConfiguration.GetHandler();
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Onecert API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

//app.UseMiddleware<ExceptionMiddleware>();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 401 && !context.Response.HasStarted) // 401
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 401;

        // Customize your response object as needed
        var jsonResponse = new
        {
            statusCode = 401,
            statusMessage = "Access Token validation has failed. Invalid Access Token!",
            status = "Failure",
        };

        // Serialize the object to JSON and write it to the response
        await context.Response.WriteAsJsonAsync(jsonResponse);
    }

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted) // 404
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 404;

        // Customize your response object as needed
        var jsonResponse = new
        {
            statusCode = 404,
            statusMessage = "Invalid url.",
            status = "Failure",
        };

        // Serialize the object to JSON and write it to the response
        await context.Response.WriteAsJsonAsync(jsonResponse);
    }
});
// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
