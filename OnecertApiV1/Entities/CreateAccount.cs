namespace OnecertApiV1.Entities
{

    public class GetAccount
    {
        public string RequestID { get; set; }
        public string ClientID { get; set; }
        public string TotalCount { get; set; }
        public string HasNext { get; set; }
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public string IsPrivileged { get; set; }
        public string AccountDescription { get; set; }
        public string CreatedDate { get; set; }
        public string LastLogin { get; set; }
        public string ExpiryDate { get; set; }
        public string Entitlements { get; set; }
    }

    public class GetGroup
    {
        public string EntitlementName { get; set; }
    }
    public class GetAccountList2
    {
        public string hasNext { get; set; }
        public long totalCount { get; set; }
        public List<GetAccountList> AccountRes { get; set; }
    }
    public class GetAccountList
    {
        public string? AccountOwner { get; set; }
        public string? AccountName { get; set; }
        public string? AccountType { get; set; }
        public string? IsPriviledge { get; set; }
        public string? AccountStatus { get; set; }
        public string? AccountDescription { get; set; }
        public string? CreatedDate { get; set; }
        public string? LastLogin { get; set; }
        public string? Entitlements { get; set; }
        public string? ExpiryDate { get; set; }
        public List<Entitlement>? accessRoles { get; set; }
    }
    public class GetEntitlementById
    {
        public string? EntitlementName { get; set; }
        public string? EntitlementDescription { get; set; }
        public string? IsPriviledge { get; set; }
        public string? EntitlementOwner { get; set; }
    }

    public class PagingParameters
    {
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        //public string? OrderBy { get; set; }
        //public string? AttrName { get; set; }
    }

    public class UpdateAccount
    {
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Branch { get; set; }
    }

    public class APILogin
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
    }

    public class CreateAccountRequest
    {
        public string AccountName { get; set; }
        public string AccountOwner { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public string Email { get; set; }
        public string Branch { get; set; }
        //public string IsPrivileged { get; set; }
      //  public string AccountDescription { get; set; }
       // public int ExpiryDate { get; set; }
        //public string Password { get; set; }
        public List<Entitlement> AccessRoles { get; set; }
    }
    
    public class CreateAccountRes
    {
        public string AccountName { get; set; }
        public string AccountOwner { get; set; }
    }

    public class DeleteAccount
    {
        public string RequestID { get; set; }
        public string ClientID { get; set; }
        public string TotalCount { get; set; }
        public string HasNext { get; set; }
        public string Accounts { get; set; }
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public string IsPrivileged { get; set; }
        public string AccountDescription { get; set; }
        public string CreateDate { get; set; }
        public string LastLogin { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class EnableAccount
    {
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
    }

    public class Entitlement
    {
        public string RoleName { get; set; }
    }

    public class AddEntitlement
    {
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public List<Entitlement> AccessRoles { get; set; }
    }

    public class RemoveEntitlement
    {
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public List<Entitlement> AccessRoles { get; set; }
    }

    public class APIResponse
    {
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
    }

    public class GetServicesRequest
    {
        public string ClientID { get; set; }
        public string Hash { get; set; }
    }


}
