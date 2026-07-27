using FlexyBill.Exceptions;
using Newtonsoft.Json;
using OnecertApiV1.Entities;
using OnecertApiV1.Exceptions;
using System.Net;

namespace OnecertApiV1.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception error)
            {

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = BaseResponse<string>._Failed(error.Message);
                switch (error)
                {
                    case ValidationException e:
                        responseModel.statusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.statusMessage = e.Message;
                        break;
                    case NotFoundException e:
                        responseModel.statusCode = (int)HttpStatusCode.NotFound;
                        responseModel.statusMessage = e.Message;
                        break;
                    case CustomValidationException e:
                        responseModel.statusCode = (int)HttpStatusCode.OK;
                        responseModel.statusMessage = e.Message;
                        break;
                    case UnauthorizedAccessException e:
                        responseModel.statusCode = (int)HttpStatusCode.Unauthorized;
                        responseModel.statusMessage = e.Message;
                        break;
                    default:
                        // unhandled error 
                        _logger.LogError(error, error.Source, error.InnerException, error.Message, error.ToString());
                        responseModel.statusCode = (int)HttpStatusCode.InternalServerError;
                        //responseModel.statusMessage = "Oops! Something went wrong. Please Try Again Later.";
                        responseModel.statusMessage = error.Message;
                        break;
                }
                var result = JsonConvert.SerializeObject(responseModel);
                await response.WriteAsync(result);
            }
        }
    }


}