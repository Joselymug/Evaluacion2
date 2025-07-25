using System.Data.SqlClient;
using System.Data;

namespace SistemaGestionTareas.API.Data
{
    public class AppDbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection => new SqlConnection(_connectionString);
    }
}
