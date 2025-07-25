using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

using System.Data.SqlClient;
using System.Data;
using SqlConnection = System.Data.SqlClient.SqlConnection;

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

