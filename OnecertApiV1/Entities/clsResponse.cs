using System.Security.AccessControl;

namespace OnecertApiV1.Entities
{
    public class clsResponses
    {
        public string Status { get; set; }
        public string StatusMessage { get; set; }
    }

    public class Responses<T>
    {
        public Responses()
        {
        }

        public Responses(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }

        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }

  

    public class PagingResponsesG
    {
        public int totalCount { get; set; }
        public string hasNext { get; set; }
        public object entitlements { get; set; }
    }

    public class PagingResponses
    {
        public int totalCount { get; set; }
        public string hasNext { get; set; }
        public object Accounts { get; set; }
    }

    public class creatAccountResponse
    {
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
    }

    public class getGroupsResponse
    {
        public string entitlementUniqueName { get; set; }
        public string entitlementDisplayName { get; set; }
        public string entitlementDescription { get; set; }
        public string entitlementLink { get; set; }
        public string EntitlementType1 { get; set; }
        public string EntitlementType2 { get; set; }
    }

    public class getGroupsRowResponse2
    {
        public string hasNext { get; set; }
        public long totalCount { get; set; } 
        public List<getGroupsRowResponse> GroupRes { get; set; }
    }
    public class getGroupsRowResponse
    {
        public string EntitlementName { get; set; }
        public string EntitlementDescription { get; set; }
        public string isPrivileged { get; set; }
        public string EntitlementOwner { get; set; }
    }

    public class getAccountResponse
    {
        public string AccountOwner { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
        public string Isprivileged { get; set; }
        public string AccountDescription { get; set; }
        public string CreatedDate { get; set; }
        public string LastLogin { get; set; }
        public string Entitlements { get; set; }
        public string accessRole { get; set; }
        public List<accessRoles> accessRoles { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class accessRoles
    {
        public string? roleName { get; set; }
    }

}
