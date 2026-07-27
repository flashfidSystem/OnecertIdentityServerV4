using FlexyBill.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnecertApiV1.Entities;
using OnecertApiV1.Exceptions;
using OnecertApiV1.Services.Interface;
using System.DirectoryServices.ActiveDirectory;
using System.Net;

namespace OnecertApiV1.Controllers
{
    [Authorize]
    [Route("onecert/v1")]
    [ApiController]
    public class UAToneCertController : Controller
    {
        private readonly IDataRepo _dataRepo;
        public UAToneCertController(IDataRepo dataRepo)
        {
            _dataRepo = dataRepo;
        }
        [HttpPost]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccount req)
        {
            try
            {
                // Check if request is not null
                if (!ModelState.IsValid)
                {
                    throw new ValidationException("Invalid input");
                }
                else
                {

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(req.AccountOwner, "Update Account", "requestDetails" + req.AccountName + req.AccountType + req.AccountType + req.Email + req.Branch);

                    string res = await _dataRepo.UpdateAccount(req.AccountOwner, req.AccountName, req.AccountType, req.AccountStatus, req.Email, req.Branch, req.Password);
                    if (res == "1")
                    {
                        _dataRepo.LogRequestUpdate(reqID, "Successful");

                        return Ok(BaseResponse<string>._Success($"User account {req.AccountOwner} got successfully updated", HttpStatusCode.OK, "Success"));

                    }
                    else
                    {
                        throw new NotFoundException($"Error trying to create account from the user {req.AccountOwner}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        [HttpPost]
        [Route("createAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest req)
        {
            try
            {
                // Check if request is not null
                if (!ModelState.IsValid)
                {
                    throw new ValidationException("Invalid input");
                }
                else
                {


                    //string entitlementList = "";
                    //if (req.AccessRoles.Count == 1)
                    //{
                    //    foreach (var ent in req.AccessRoles)
                    //    {
                    //        entitlementList = ent.RoleName;
                    //    }
                    //}
                    //else
                    //{
                    //    return BadRequest(BaseResponse<string>._Failed($"Can only process one entitlement at a time", HttpStatusCode.BadRequest, "Failure"));
                    //}

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(req.AccountOwner, "Create Account", "requestDetails" + req.AccountName + req.AccountStatus + req.AccountType + req.AccessRoles.ToString());

                    string res = await _dataRepo.CreateAccount(req.AccountOwner, req.AccountName, req.AccountType, req.AccountStatus, req.Email, req.Branch, req.AccessRoles);
                    if (res == "1")
                    {
                        _dataRepo.LogRequestUpdate(reqID, "Successful");

                        return Ok(BaseResponse<string>._Success($"User account {req.AccountOwner} got successfully created!.", HttpStatusCode.OK, "Success"));

                    }
                    else
                    {
                        return BadRequest(BaseResponse<string>._Failed(res, HttpStatusCode.BadRequest, "Failure"));
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        [Route("addEntitlement")]
        public async Task<IActionResult> AddEntitlement(AddEntitlement req)
        {
            try
            {
                // Check if request is not null
                if (!ModelState.IsValid)
                {
                    throw new ValidationException("Invalid input");
                }
                else
                {
                 
                    // Log request 
                    long reqID = await _dataRepo.LogRequest(req.AccountOwner, "Add Entitlement", "requestDetails=" + req.AccountOwner + req.AccountName + req.AccessRoles.ToString());



                    string res = await _dataRepo.AddEntitlement(req.AccountOwner, req.AccountName, req.AccessRoles);
                    string entitlementsString = string.Join("/", req.AccessRoles.Select(e => e.RoleName));
                    if (res == "1")
                    {
                        _dataRepo.LogRequestUpdate(reqID, "Successful");

                        return Ok(BaseResponse<string>._Success($"Entitlement {entitlementsString} added to user account {req.AccountOwner} successfully.", HttpStatusCode.OK, "Success"));

                    }
                    else
                    {
                        return BadRequest(BaseResponse<string>._Failed(res, HttpStatusCode.BadRequest, "Failure"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        [HttpGet]
        [Route("testConnection")]
        public async Task<IActionResult> TestConnectionSubAsync()
        {
            try
            {
                string ClientId = _dataRepo.GetClientIdFromToken();

                long res = await _dataRepo.LogRequest(ClientId, "Testing Connections", ClientId);
                if (res >= 0)
                {
                    return StatusCode(200, new { statusCode = "200", status = "Success", message = "Server is reachable" });
                }
                else
                {
                    return StatusCode(500, new { statusCode = "500", status = "Failure", message = "Server is not reachable" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { statusCode = "500", status = "Failure", message = "Server is not reachable" });
            }
        }





        [HttpGet]
        [Route("GetAccount")]
        public async Task<IActionResult> GetallAccounts([FromQuery] PagingParameters req)
        {
            try
            {
                // Check if request is not null
                if (!ModelState.IsValid)
                {

                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid client credentials" });

                }
                else
                {
                    if (req.CurrentPage <= 0)
                    {
                        return StatusCode(404, new { status = "Failure", message = "currentPage can not start with zero." });
                    }
                    // Log request 
                    long reqID = await _dataRepo.LogRequest("get method", "Get all Account", "requestDetails=list all users");

                    var res = await _dataRepo.AllAccount(req);


                    _dataRepo.LogRequestUpdate(reqID, "Successful");

                    return StatusCode(200, new { totalCount = res.totalCount, hasNext = res.hasNext, accounts = res.accounts });


                }
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { status = "Failure", statusCode = "404", message = "System error." });
            }

        }

        [HttpGet]
        [Route("GetGroup")]
        public async Task<IActionResult> GetallGroups([FromQuery] PagingParameters req)
        {
            try
            {
                // Check if request is not null
                if (!ModelState.IsValid)
                {
                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid client credentials" });
                }
                else
                {
                    if (req.CurrentPage <= 0)
                    {
                        return StatusCode(404, new { status = "Failure", message = "currentPage can not start with zero." });
                    }
                    // Log request 
                    long reqID = await _dataRepo.LogRequest("get method", "Get all groups", "requestDetails= got a paged list of all users");

                    getGroupsRowResponse2 res = await _dataRepo.AllGroup(req);


                    _dataRepo.LogRequestUpdate(reqID, "Successful");


                    return StatusCode(200, new { totalCount = res.totalCount, hasNext = res.hasNext, entitlements = res.GroupRes });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { status = "Failure", statusCode = "404", message = "System error." });
            }

        }

        [HttpGet]
        [Route("Account/{accountowner}")]
        public async Task<IActionResult> Account(string accountowner)
        {
            try
            {
                // Check if request is not null
                if (string.IsNullOrEmpty(accountowner))
                {
                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid client credentials" });
                }
                else
                {

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(accountowner, "Get Single Account", "requestDetails=" + accountowner);

                    //_dataRepo.AllAccount(accountowner);
                    var res = await _dataRepo.GetAccount(accountowner);
                    if (res.Count == 0)
                    {
                        return StatusCode(404, new { status = "Failure", statusCode = "404", message = $"Account not found with account name: {accountowner}" });
                    }
                    var Returns = new getAccountResponse();

                    string? IsprivilegedCheckY = null;
                    string? IsprivilegedCheckN = null;

                    string? AccountTypeA = null;
                    string? AccountTypeU = null;
                    Returns.accessRoles = new List<accessRoles>();
                    foreach (var item in res)
                    {
                        if (item.accessRole == null)
                        {
                            return StatusCode(404, new { status = "Failure", statusCode = "404", message = $"Account not found with account name: {accountowner}" });
                        }
                        else
                        {
                            Returns.accessRoles = JsonConvert.DeserializeObject<List<accessRoles>>(item.accessRole); 
                        }

                        Returns.AccountName = item.AccountName;
                        Returns.AccountOwner = item.AccountOwner;
                        Returns.AccountStatus = item.AccountStatus;
                        Returns.AccountDescription = item.AccountDescription;
                        Returns.CreatedDate = item.CreatedDate;
                        Returns.LastLogin = item.LastLogin;
                        Returns.ExpiryDate = item.ExpiryDate;

                        if (item.Isprivileged == "YES")
                        {
                            IsprivilegedCheckY = item.Isprivileged;
                        }
                        else
                        {
                            IsprivilegedCheckN = item.Isprivileged;
                        }

                        if (item.AccountType.Contains("Admin"))
                        {
                            AccountTypeA = item.AccountType;
                        }
                        else
                        {
                            AccountTypeU = item.AccountType;
                        }

                    }

                    Returns.Isprivileged = IsprivilegedCheckY ?? IsprivilegedCheckN;
                    Returns.AccountType = AccountTypeA ?? AccountTypeU;
                    //if (res.Entitlements != "")
                    //{
                    //    res.accessRoles = new List<accessRoles>();
                    //    res.accessRoles.Add(new accessRoles { roleName = res.Entitlements.ToString() });


                    //}
                    _dataRepo.LogRequestUpdate(reqID, "Successful");

                    // If user exist 
                    return StatusCode(200, new { accountName = Returns.AccountName, accountOwner = Returns.AccountOwner, accountType = Returns.AccountType, accountStatus = Returns.AccountStatus, accountDescription = Returns.AccountDescription, isPrivileged = Returns.Isprivileged, lastLogin = Returns.LastLogin, accessRoles = Returns.accessRoles });
                    //return Ok(BaseResponse<getAccountResponse>._Success($"", HttpStatusCode.OK, "Success"));

                }
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { status = "Failure", statusCode = "404", message = "System error." });
            }

        }
        [HttpGet]
        [Route("entitlement/{groupid}")]
        public async Task<IActionResult> entitlement(string groupid)
        {
            try
            {
                // Check if request is not null
                if (string.IsNullOrEmpty(groupid))
                {
                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid client credentials" });
                }
                else
                {

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(groupid, "Get single entitlement", "requestDetails=" + groupid);

                    //_dataRepo.AllAccount(accountowner);
                    var res = await _dataRepo.GetGroups(groupid);

                    _dataRepo.LogRequestUpdate(reqID, "Successful");

                    if (res != null)
                    {
                        return StatusCode(200, new { entitlementName = res.EntitlementName, entitlementDescription = res.EntitlementDescription, isPrivileged = res.IsPriviledge });

                    }
                    return StatusCode(404, new { status = "Failure", statusCode = "404", message = $"Entitlement not found with entitlement name: {groupid}" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { status = "Failure", statusCode = "404", message = "System error." });
            }

        }


        [HttpDelete("DeleteAccount/{accountowner}")]
        public async Task<IActionResult> DeleteAccount(string? accountowner)
        {
            try
            {
                // Check if request is not null
                if (string.IsNullOrEmpty(accountowner))
                {
                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid input" });
                }
                else
                {

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(accountowner, "Delete Account", "requestDetails=" + accountowner);

                    //_dataRepo.AllAccount(accountowner);
                    string res = await _dataRepo.DeleteAccount(accountowner);
                    if (res == "1")
                    {
                        _dataRepo.LogRequestUpdate(reqID, "Successful");

                        return Ok(BaseResponse<string>._Success($"User account {accountowner} got deleted successfully.", HttpStatusCode.OK, "Success"));

                    }
                    else
                    {
                        return BadRequest(BaseResponse<string>._Failed(res, HttpStatusCode.BadRequest, "Failure"));
                    }

                }
            }
            catch (Exception ex)
            {
                return NotFound(BaseResponse<string>._Failed(ex.Message, HttpStatusCode.NotFound, "Failure"));

            }

        }
        
        [HttpPost("DeleteAccount/{accountowner}")]
        public async Task<IActionResult> DeleteAccountPost(string? accountowner)
        {
            try
            {
                // Check if request is not null
                if (string.IsNullOrEmpty(accountowner))
                {
                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid client credentials" });
                }
                else
                {

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(accountowner, "Delete Account", "requestDetails=" + accountowner);

                    //_dataRepo.AllAccount(accountowner);
                    string res = await _dataRepo.DeleteAccount(accountowner);
                    if (res == "1")
                    {
                        _dataRepo.LogRequestUpdate(reqID, "Successful");

                        return Ok(BaseResponse<string>._Success($"User account {accountowner} got deleted successfully.", HttpStatusCode.OK, "Success"));

                    }
                    else
                    {
                        throw new NotFoundException($"Error deleting User with the UserID {accountowner}");
                    }

                }
            }
            catch (Exception ex)
            {
                return NotFound(BaseResponse<string>._Failed(ex.Message, HttpStatusCode.NotFound, "Failure"));

            }

        }

         

        [HttpPost]
        [Route("removeEntitlement")]
        public async Task<IActionResult> RemoveEntitlement(RemoveEntitlement req)
        {
            try
            {
                // Check if request is not null
                if (!ModelState.IsValid)
                {
                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid client credentials" });
                }
                else
                {
                    

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(req.AccountOwner, "Remove Entitlement", "requestDetails" + req.AccountName + req.AccountOwner + req.AccessRoles.ToString());

                    string res = await _dataRepo.RemoveEntitlement(req.AccountOwner, req.AccountName, req.AccessRoles);
                    string entitlementsString = string.Join("/", req.AccessRoles.Select(e => e.RoleName));
                    if (res == "1")
                    {
                        _dataRepo.LogRequestUpdate(reqID, "Successful");

                        return Ok(BaseResponse<string>._Success($"Entitlement {entitlementsString} removed from the user {req.AccountOwner} account successfully.", HttpStatusCode.OK, "Success"));

                    }
                    else
                    {
                        return BadRequest(BaseResponse<string>._Failed(res, HttpStatusCode.BadRequest, "Failure"));
                    }

                }
            }
            catch (Exception ex)
            {
                return NotFound(BaseResponse<string>._Failed(ex.Message, HttpStatusCode.NotFound, "Failure"));
            }

        }




        [HttpGet]
        [Route("DisableAccount/{accountowner}")]
        public async Task<IActionResult> DisableAccount(string accountowner)
        {
            try
            {
                // Check if request is not null
                if (string.IsNullOrEmpty(accountowner))
                {
                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid client credentials" });
                }
                else
                {

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(accountowner, "Disable Account", "requestDetails=" + accountowner);

                    //_dataRepo.AllAccount(accountowner);
                    string res = await _dataRepo.DisableAccount(accountowner);
                    if (res == "1")
                    {
                        _dataRepo.LogRequestUpdate(reqID, "Successful");

                        return Ok(BaseResponse<string>._Success($"User account {accountowner} got disable successfully.", HttpStatusCode.OK, "Success"));

                    }
                    else
                    {
                        throw new NotFoundException($"Error trying to disable User with the UserID {accountowner}");
                    }

                }
            }
            catch (Exception ex)
            {
                return NotFound(BaseResponse<string>._Failed(ex.Message, HttpStatusCode.NotFound, "Failure"));

            }
        }

        [HttpGet]
        [Route("EnableAccount/{accountowner}")]
        public async Task<IActionResult> EnableAccount(string accountowner)
        {
            try
            {
                // Check if request is not null
                if (string.IsNullOrEmpty(accountowner))
                {
                    return StatusCode(401, new { status = "Failure", statusCode = "401", message = "Invalid client credentials" });
                }
                else
                {

                    // Log request 
                    long reqID = await _dataRepo.LogRequest(accountowner, "Enable Account", "requestDetails=" + accountowner);

                    //_dataRepo.AllAccount(accountowner);
                    string res = await _dataRepo.EnableAccount(accountowner);
                    if (res == "1")
                    {
                        _dataRepo.LogRequestUpdate(reqID, "Successful");

                        return Ok(BaseResponse<string>._Success($"User account {accountowner} got enabled successfully.", HttpStatusCode.OK, "Success"));
                    }
                    else
                    {
                        throw new NotFoundException($"Error trying to enable User with the UserID {accountowner}");
                    }

                }
            }
            catch (Exception ex)
            {
                return NotFound(BaseResponse<string>._Failed(ex.Message, HttpStatusCode.NotFound, "Failure"));

            }

        }
    }
}
