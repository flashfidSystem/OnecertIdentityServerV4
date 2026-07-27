using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OnecertApiV1.Context;
using OnecertApiV1.Entities;
using OnecertApiV1.Services.Interface;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OnecertApiV1.Services.Implementation
{
    public class DataRepo : IDataRepo
    {
        private static readonly char[] _validChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_=+[]{}".ToCharArray();


        private readonly clsParameters _pa;
        private readonly DapperContext _dataRepo;
        private readonly clsMail _mail;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        public DataRepo(DapperContext dapperContext, IConfiguration config, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, clsMail mail, clsParameters pa)
        {
            _dataRepo = dapperContext;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _mail = mail;
            _pa= pa;
        }

        public async Task<TokenResponse> GetToken(string client_id, string grant_type, string client_secret)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            // Create the HttpClient using the custom handler
            var client = new HttpClient(handler);

            // Proceed with the HTTP request
            var request = new HttpRequestMessage(HttpMethod.Post, _config["OpenId:IdentityCall"]);

            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new KeyValuePair<string, string>("grant_type", grant_type));
            collection.Add(new KeyValuePair<string, string>("client_id", client_id));
            collection.Add(new KeyValuePair<string, string>("client_secret", client_secret));

            var content = new FormUrlEncodedContent(collection);
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TokenResponse?>(responseContent);
        }
        public static string GeneratePassword(int length = 12)
        {
            if (length < 8)
                throw new ArgumentException("Password length should be at least 8 characters for security.");

            byte[] randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var result = new StringBuilder(length);
            foreach (byte b in randomBytes)
            {
                // Use modulo to select a valid character
                result.Append(_validChars[b % _validChars.Length]);
            }

            return result.ToString();
        }

        public async Task<string> CreateAccount(string accountOwner, string AccountName, string AccountType, string AccountStatus, string Email, string Branch, List<Entitlement> Entitlements)
        {
            try
            {
                string password = GeneratePassword(12);

               
                string entitlementsString = string.Join("/", Entitlements.Select(e => e.RoleName));
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("spCreateUserOnecert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120;

                        // Set the parameter values and execute the command
                        command.Parameters.AddWithValue("@accountOwner", accountOwner);
                        command.Parameters.AddWithValue("@AccountName", AccountName);
                        command.Parameters.AddWithValue("@AccountType", AccountType);
                        command.Parameters.AddWithValue("@AccountStatus", AccountStatus);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Branch", Branch);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Entitlements", entitlementsString);
                        SqlParameter outputParam = new SqlParameter("@StatusO", SqlDbType.Char, 1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);
                        await command.ExecuteNonQueryAsync();


                        // Send mail here for password 
                        var objParameters = _pa.GetParameters();

                        string fromEmail = "";

                        if (objParameters.parameterNames.Length > 0)
                        {
                            fromEmail = _pa.getValue(objParameters, "fromEmail");
                        }

                        _mail.SendPasswordToEmail(fromEmail, accountOwner, password, Email);

                        // Retrieve the output parameter value
                        string status = outputParam.Value?.ToString();
                        return status;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> UpdateAccount(string accountOwner, string AccountName, string AccountType, string AccountStatus, string Email, string Branch, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("spAccountUpdateOnecert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120; // Set the timeout to 2 minutes (in seconds)

                        // Set the parameter values and execute the command
                        command.Parameters.AddWithValue("@accountOwner", accountOwner);
                        command.Parameters.AddWithValue("@AccountName", AccountName);
                        command.Parameters.AddWithValue("@AccountType", AccountType);
                        command.Parameters.AddWithValue("@AccountStatus", AccountStatus);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Branch", Branch);
                        command.Parameters.AddWithValue("@Password", password);
                        //command.Parameters.Add("@StatusO", SqlDbType.Char, 1).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        //string status = command.Parameters["@StatusO"].Value.ToString();
                        string status = "1";
                        return status;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public async Task<string> AddEntitlement(string accountOwner, string accountName, List<Entitlement> Entitlements)
        {
            try
            {
                string entitlementsString = string.Join("/", Entitlements.Select(e => e.RoleName));
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("spAddEntitlementOnecert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120; // Set the timeout to 2 minutes (in seconds)

                        // Set the parameter values and execute the command
                        command.Parameters.AddWithValue("@accountOwner", accountOwner);
                        command.Parameters.AddWithValue("@AccountName", accountName);
                        command.Parameters.AddWithValue("@Entitlements", entitlementsString);
                        command.Parameters.Add("@StatusO", SqlDbType.Char, 1).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        string status = command.Parameters["@StatusO"].Value.ToString();
                        return status;
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        public async Task<long> LogRequest(string clientId, string method, string requestDetails)
        {
            long retId = 0;

            using (var connection = _dataRepo.CreateConnection()) // Replace YourConnectionString with the actual connection string
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@clientid", clientId, DbType.String);
                parameters.Add("@method", method, DbType.String);
                parameters.Add("@requestdetails", requestDetails, DbType.String);
                parameters.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

                connection.Execute("spLogRequestOnecert", parameters, commandType: CommandType.StoredProcedure);

                retId = parameters.Get<long>("@ID");
            }

            return retId;
        }

        public void LogRequestUpdate(long id, string responsedesc)
        {
            using (var connection = _dataRepo.CreateConnection()) // Replace YourConnectionString with the actual connection string
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.Int64);
                parameters.Add("@responsedesc", responsedesc, DbType.String);

                connection.Execute("splogRequestUpdateOnecert", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> DeleteAccount(string accountOwner)
        {
            try
            {
                string status = "";
                using (var connection = _dataRepo.CreateConnection()) // Replace YourConnectionString with the actual connection string
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@accountOwner", accountOwner, DbType.String);
                    parameters.Add("@Status", dbType: DbType.String, size: 1, direction: ParameterDirection.Output);
                    connection.Execute("spDeleteUserOnecert", parameters, commandType: CommandType.StoredProcedure);

                    status = parameters.Get<string>("@Status");
                    return status;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> RemoveEntitlement(string accountOwner, string AccountName, List<Entitlement> Entitlements)
        {
            try
            {
                string entitlementsString = string.Join("/", Entitlements.Select(e => e.RoleName));
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("spDeleteEntitlementOnecert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120; // Set the timeout to 2 minutes (in seconds)

                        // Set the parameter values and execute the command
                        command.Parameters.AddWithValue("@accountOwner", accountOwner);
                        command.Parameters.AddWithValue("@AccountName", AccountName);
                        command.Parameters.AddWithValue("@Entitlements", entitlementsString);
                        command.Parameters.Add("@StatusO", SqlDbType.Char, 1).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        string status = command.Parameters["@StatusO"].Value.ToString();
                        return status;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> EnableAccount(string accountOwner)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("spEnableAccountOnecert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120; // Set the timeout to 2 minutes (in seconds)

                        // Set the parameter values and execute the command
                        command.Parameters.AddWithValue("@AccountOwner", accountOwner);
                        command.Parameters.Add("@Status", SqlDbType.Char, 1).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        string status = command.Parameters["@Status"].Value.ToString();
                        return status;
                    }
                }


            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<ApiResponse> AllAccount(PagingParameters req)
        {
            var apiResponse = new ApiResponse();
            var storedProcedure = "spListAllUsersOnecert";

            using (var connection = _dataRepo.CreateConnection())
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@CurrentPage", req.CurrentPage, DbType.Int32);
                parameters.Add("@PageSize", req.PageSize, DbType.Int32);

                parameters.Add("@NextSetExists", dbType: DbType.String, size: 1, direction: ParameterDirection.Output);
                parameters.Add("@TotalCount", dbType: DbType.Int64, direction: ParameterDirection.Output);

                IEnumerable<string> result = await connection.QueryAsync<string>(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                string concatenatedString = string.Join("", result);

                // Deserialize the JSON array to a string array
                var jsonValues = JsonConvert.DeserializeObject<ApiResponse>(concatenatedString);


                apiResponse.hasNext = parameters.Get<string>("@NextSetExists");
                apiResponse.totalCount = parameters.Get<long>("@TotalCount");

                if (jsonValues != null)
                {
                    apiResponse.accounts = jsonValues.accounts;

                    apiResponse.accounts.ForEach(account =>
                    {
                        // Check for multiple occurrences of accountOwner
                        if (apiResponse.accounts.Count(a => a.AccountOwner == account.AccountOwner) > 1)
                        {
                            account.AccessRoles = apiResponse.accounts
                                .Where(a => a.AccountOwner == account.AccountOwner)
                                .SelectMany(a => a.AccessRoles)
                                .DistinctBy(role => role.RoleName) // Use a DistinctBy extension method
                                .ToList();

                            // Check if "Super" is present in any roleName
                            account.IsPrivileged = account.AccessRoles.Any(role => role.RoleName.ToUpper().Contains("Super".ToUpper()))
                                ? "Yes"
                                : "No";
                        }
                    });

                    // Filter unique accountOwners
                    apiResponse.accounts = apiResponse.accounts
                        .GroupBy(account => account.AccountOwner)
                        .Select(group => group.First())
                        .ToList();

                }
                else
                {
                    var emptyAccounts = new List<UserAccount>();
                    apiResponse.accounts = emptyAccounts;
                }

                return apiResponse;



            }
        }


        public async Task<getGroupsRowResponse2> AllGroup(PagingParameters req)
        {
            var GetAccountList = new getGroupsRowResponse2();
            var storedProcedure = "spListAllRolesOnecert";

            using (var connection = _dataRepo.CreateConnection())
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@CurrentPage", req.CurrentPage, DbType.Int32);
                parameters.Add("@PageSize", req.PageSize, DbType.Int32);

                parameters.Add("@NextSetExists", dbType: DbType.String, size: 1, direction: ParameterDirection.Output);

                parameters.Add("@TotalCount", dbType: DbType.Int64, direction: ParameterDirection.Output);

                var result = await connection.QueryAsync<getGroupsRowResponse>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                GetAccountList.hasNext = parameters.Get<string>("@NextSetExists");
                GetAccountList.totalCount = parameters.Get<long>("@TotalCount");

                GetAccountList.GroupRes = result.ToList();
                return GetAccountList;
            }
        }
        public async Task<List<getAccountResponse>> GetAccount(string accountOwner)
        {

            using (var connection = _dataRepo.CreateConnection()) // Replace YourConnectionString with the actual connection string
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@maccountOwner", accountOwner);
                var result = await connection.QueryAsync<getAccountResponse>("spListUsersOnecert", parameters, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }


        public async Task<string> DisableAccount(string accountOwner)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("SqlConnection")))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("spDisableAccountOnecert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 120; // Set the timeout to 2 minutes (in seconds)

                        // Set the parameter values and execute the command
                        command.Parameters.AddWithValue("@AccountOwner", accountOwner);
                        command.Parameters.Add("@Status", SqlDbType.Char, 1).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        string status = command.Parameters["@Status"].Value.ToString();
                        return status;
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }




        public async Task<GetEntitlementById> GetGroups(string groupID)
        {
            using (var connection = _dataRepo.CreateConnection()) // Replace YourConnectionString with the actual connection string
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@mEntitlementName", groupID);
                var result = connection.QuerySingleOrDefault<GetEntitlementById>("spListEntitlementsOnecert", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
        public string GetClientIdFromToken()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            string authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                // Authorization header not found
                return null;
            }

            string[] headerParts = authorizationHeader.Split(' ');

            if (headerParts.Length != 2 || headerParts[0] != "Bearer")
            {
                // Invalid authorization header format
                return null;
            }

            string token = headerParts[1];

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(token);

            var clientId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "client_id")?.Value;

            return clientId;
        }

    }
}
