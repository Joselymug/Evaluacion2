using Dapper;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using SistemaGestionTareas;
using System.Data.SqlClient;


namespace SistemaGestionTareas.API.Data
{
    public class CrudService<T> where T : class
    {
        private readonly string _connectionString;

        public CrudService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Obtener todos los registros
        public IEnumerable<T> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(T).Name + "s"; // Nombre de la tabla dinámico
                var query = $"SELECT * FROM {tableName}"; // Consulta SQL
                return connection.Query<T>(query).ToList();
            }
        }

        // Obtener por ID
        public T GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(T).Name + "s"; // Nombre de la tabla dinámico
                var query = $"SELECT * FROM {tableName} WHERE Id = @Id"; // Consulta SQL
                return connection.QuerySingleOrDefault<T>(query, new { Id = id });
            }
        }

        // Crear un nuevo registro
        public void Create(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(T).Name + "s"; // Nombre de la tabla dinámico
                var columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name)); // Nombres de las columnas
                var values = string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name)); // Valores para insertar
                var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})"; // Consulta SQL
                connection.Execute(query, entity); // Ejecuta la consulta
            }
        }

        // Actualizar un registro
        public void Update(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(T).Name + "s"; // Nombre de la tabla dinámico
                var setColumns = string.Join(", ", typeof(T).GetProperties().Select(p => $"{p.Name} = @{p.Name}")); // Columnas a actualizar
                var query = $"UPDATE {tableName} SET {setColumns} WHERE Id = @Id"; // Consulta SQL
                connection.Execute(query, entity); // Ejecuta la consulta
            }
        }

        // Eliminar un registro
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(T).Name + "s"; // Nombre de la tabla dinámico
                var query = $"DELETE FROM {tableName} WHERE Id = @Id"; // Consulta SQL
                connection.Execute(query, new { Id = id }); // Ejecuta la consulta
            }
        }
    }
}
