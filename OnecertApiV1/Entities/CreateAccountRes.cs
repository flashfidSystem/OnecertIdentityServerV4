namespace OnecertApiV1.Entities
{

    public class AccessRole
    {
        public string? RoleName { get; set; }
    }

    public class UserAccount
    {
        public string? AccountName { get; set; }
        public string? AccountOwner { get; set; }
        public string? AccountType { get; set; }
        public string? AccountStatus { get; set; }
        public string? AccountDescription { get; set; }
        public string? IsPrivileged { get; set; }
        public string? LastLogin { get; set; }
        public List<AccessRole>? AccessRoles { get; set; }
    }

    public class ApiResponse
    {
        public string? hasNext { get; set; }
        public long totalCount { get; set; }
        public List<UserAccount>? accounts { get; set; }
    }
}
