using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace SistemaGestionTareas.API.Data
{
    public class AppDbContext
    {
        private readonly string _connectionString;

        public AppDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection Connection => new SqlConnection(_connectionString);
    }
}
