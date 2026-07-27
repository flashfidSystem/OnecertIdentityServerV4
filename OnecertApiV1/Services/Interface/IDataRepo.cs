using OnecertApiV1.Entities;
using System.Data;

namespace OnecertApiV1.Services.Interface
{
    public interface IDataRepo
    {
        Task<TokenResponse> GetToken(string client_id, string grant_type, string client_secret);
        Task<long> LogRequest(string clientId, string method, string requestDetails);
        string GetClientIdFromToken();
        void LogRequestUpdate(long id, string responsedesc);
        Task<string> CreateAccount(string accountOwner, string AccountName, string AccountType, string AccountStatus, string Email, string Branch, List<Entitlement> Entitlements);
        Task<string> UpdateAccount(string accountOwner, string AccountName, string AccountType, string AccountStatus, string Email, string Branch, string password);
        Task<string> DeleteAccount(string accountOwner);
        Task<string> RemoveEntitlement(string accountOwner, string AccountName, List<Entitlement> Entitlements);
        Task<string> EnableAccount(string accountOwner);
        Task<ApiResponse>  AllAccount(PagingParameters req);
        Task<getGroupsRowResponse2> AllGroup(PagingParameters req);
        Task<string> DisableAccount(string accountOwner);
        Task<string> AddEntitlement(string accountOwner, string accountName, List<Entitlement> entitlements);
        Task<List<getAccountResponse>> GetAccount(string accountOwner);
        Task<GetEntitlementById> GetGroups(string groupID);
    }
}
