

using Microsoft.Data.SqlClient;
using System.Data;

namespace OnecertApiV1.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Read the connection string fresh on every call (instead of caching it once in the
        // constructor) so that a Vault credential refresh - which updates
        // ConnectionStrings:SqlConnection in configuration at runtime - is picked up immediately,
        // the same way the rest of the repository (which reads IConfiguration per call) already does.
        public IDbConnection CreateConnection()
            => new SqlConnection(_configuration.GetConnectionString("SqlConnection"));
    }
}
