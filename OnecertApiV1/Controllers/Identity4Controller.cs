using Microsoft.AspNetCore.Mvc;
using OnecertApiV1.Services.Interface;

namespace OnecertApiV1.Controllers
{
    [Route("connect")]
    [ApiController]
    public class Identity4Controller : Controller
    {
        private readonly IDataRepo _dataRepo;
        private readonly IConfiguration _config;
        public Identity4Controller(IDataRepo dataRepo, IConfiguration config)
        {
            _dataRepo = dataRepo;
            _config = config;
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> token([FromForm] Dictionary<string, string> formData)
        {
            try
            {
                string grantType = formData["grant_type"];
                string clientId = formData["client_id"];
                string clientSecret = formData["client_secret"];

                if (clientId != _config["OpenId:ClientId"])
                {
                    return StatusCode(401, new { statusCode = "401", status = "Failure", message = "Invalid Client ID!." });
                }
                if (clientSecret != _config["OpenId:AppSecret"])
                {
                    return StatusCode(401, new { statusCode = "401", status = "Failure", message = "Invalid Client Secret!." });
                }
                if (grantType != "client_credentials")
                {
                    return StatusCode(401, new { statusCode = "401", status = "Failure", message = "Invalid Grant Type!." });
                }



                var res = await _dataRepo.GetToken(clientId, grantType, clientSecret);

                if (res?.access_token != null)
                {
                    return StatusCode(200, new { access_token = res.access_token, expires_in = res.expires_in, token_type = res.token_type, scope = res.scope});
                }
                else
                {
                    return StatusCode(401, new { statusCode = "401", status = "Failure", message = "Invalid client credentials" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(401, new { statusCode = "401", status = "Failure", message = "Invalid client credentials" });
            }


        }
    }

}
