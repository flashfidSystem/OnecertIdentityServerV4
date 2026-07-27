using System.Net;
using System.Text.Json.Serialization;

namespace OnecertApiV1.Entities
{
    public class BaseResponse<T>
    {
        [JsonPropertyName("status")]
        public string status { get; set; }
        [JsonPropertyName("statusCode")]
        public int statusCode { get; set; }
        [JsonPropertyName("statusMessage")]
        public string? statusMessage { get; set; } 

        public static BaseResponse<T?> _Success1(string statusMessage = "", HttpStatusCode statusCode = HttpStatusCode.OK, string? status = "Success")
        {
            return new BaseResponse<T?>
            { 
                status = status,
                statusCode = (int)statusCode,
                statusMessage = statusMessage,
            };
        }
        
        public static BaseResponse<T?> _Success(string statusMessage = "", HttpStatusCode statusCode = HttpStatusCode.OK, string? status = "Success")
        {
            return new BaseResponse<T?>
            { 
                status = status,
                statusCode = (int)statusCode,
                statusMessage = statusMessage,
            };
        }


        public static BaseResponse<T?> _Failed(string statusMessage = "", HttpStatusCode statusCode = HttpStatusCode.BadRequest, string? status = "Failure")
        {
            return new BaseResponse<T?>
            { 
                status = status,
                statusCode = (int)statusCode,
                statusMessage = statusMessage,
            };
        }
    }
    //public class Response<T>
    //{
    //    //public bool Success { get; set; }
    //    //public string Message { get; set; }
    //    //public T Data { get; set; }

    //}
}
