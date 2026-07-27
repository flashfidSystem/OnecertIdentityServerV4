namespace OnecertApiV1.Entities
{

    public class CreateAccountRes2
    {

        public class accounts
        {
            public string AccountOwner { get; set; }
            public string AccountName { get; set; }
            public string AccountType { get; set; }
            public string IsPrivileged { get; set; }
            public string AccountStatus { get; set; }
            public string AccountDescription { get; set; }
            public string CreatedDate { get; set; }
            public string LastLogin { get; set; }
            public string ExpiryDate { get; set; }
            public List<AccessRole> AccessRoles { get; set; }
            public string Add { get; set; }
            public int TotalCount { get; set; }
        }

        public class AccessRole
        {
            public string RoleName { get; set; }
        }

        public class ApiResponse
        {
            public List<accounts> accounts { get; set; }
        }

    }

}
