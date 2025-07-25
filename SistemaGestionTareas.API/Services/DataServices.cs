using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGestionTareas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaGestionTareas.Services
{
    public class DataService : IDataService
    {
        private readonly string _connectionString;

        public DataService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Métodos para usuarios
        public async Task<Usuario?> GetUserByUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Users WHERE Username = @Username";
            return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Username = username });
        }

        public async Task<IEnumerable<Usuario>> GetAllUsersAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Users";
            return await connection.QueryAsync<Usuario>(sql);
        }

        // Métodos para proyectos
        public async Task<IEnumerable<Proyecto>> GetAllProjectsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Projects";
            return await connection.QueryAsync<Proyecto>(sql);
        }

        public async Task<Proyecto?> GetProjectByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Projects WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Proyecto>(sql, new { Id = id });
        }

        public async Task<int> CreateProjectAsync(CreateProjectRequest project, int createdBy)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                INSERT INTO Projects (Name, Description, CreatedBy) 
                VALUES (@Name, @Description, @CreatedBy);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.QuerySingleAsync<int>(sql, new
            {
                project.Name,
                project.Description,
                CreatedBy = createdBy
            });
        }

        // Métodos para tareas
        public async Task<IEnumerable<Tarea>> GetTasksByProjectIdAsync(int projectId)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Tasks WHERE ProjectId = @ProjectId";
            return await connection.QueryAsync<Tarea>(sql, new { ProjectId = projectId });
        }

        public async Task<int> CreateTaskAsync(CreateTaskRequest task, int createdBy)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                INSERT INTO Tasks (Title, Description, Status, Priority, DueDate, ProjectId, CreatedBy) 
                VALUES (@Title, @Description, @Status, @Priority, @DueDate, @ProjectId, @CreatedBy);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.QuerySingleAsync<int>(sql, new
            {
                task.Title,
                task.Description,
                task.Status,
                task.Priority,
                task.DueDate,
                task.ProjectId,
                CreatedBy = createdBy
            });
        }

        public async Task<bool> UpdateTaskAsync(int id, CreateTaskRequest task)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = @"
                UPDATE Tasks 
                SET Title = @Title, Description = @Description, Status = @Status, 
                    Priority = @Priority, DueDate = @DueDate
                WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                task.Title,
                task.Description,
                task.Status,
                task.Priority,
                task.DueDate
            });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Tasks WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
