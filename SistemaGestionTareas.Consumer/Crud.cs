//using Dapper;
//using SistemaGestionTareas.Consumer.SistemaGestionTareas;
//using System;
//using System.Collections.Generic;
//using System.Data;

//namespace SistemaGestionTareas.Consumer
//{
//    namespace SistemaGestionTareas.API.Data
//    {
//        public class CrudService<T> where T : class
//        {
//            private readonly AppDbContext _context;

//            public CrudService(AppDbContext context)
//            {
//                _context = context;
//            }

//            // Obtener todos los registros
//            public IEnumerable<T> GetAll()
//            {
//                using (var connection = _context.Connection)
//                {
//                    connection.Open();
//                    var tableName = typeof(T).Name + "s"; // Obtiene el nombre de la tabla (tareas, proyectos, usuarios)
//                    var query = $"SELECT * FROM {tableName}";
//                    return connection.Query<T>(query).ToList();
//                }
//            }

//            // Obtener por ID
//            public T GetById(int id)
//            {
//                using (var connection = _context.Connection)
//                {
//                    connection.Open();
//                    var tableName = typeof(T).Name + "s"; // Obtiene el nombre de la tabla (tareas, proyectos, usuarios)
//                    var query = $"SELECT * FROM {tableName} WHERE Id = @Id";
//                    return connection.QuerySingleOrDefault<T>(query, new { Id = id });
//                }
//            }

//            // Crear un nuevo registro
//            public void Create(T entity)
//            {
//                using (var connection = _context.Connection)
//                {
//                    connection.Open();
//                    var tableName = typeof(T).Name + "s"; // Obtiene el nombre de la tabla
//                    var columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name)); // Nombres de las columnas
//                    var values = string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name)); // Valores para insertar
//                    var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
//                    connection.Execute(query, entity);
//                }
//            }


//            public T Create(T entity, IDbTransaction transaction)
//            {
//                var tableName = typeof(T).Name + "s"; // Obtiene el nombre de la tabla
//                var columns = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name)); // Nombres de las columnas
//                var values = string.Join(", ", typeof(T).GetProperties().Select(p => "@" + p.Name)); // Valores para insertar
//                var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
//                _context.Connection.Execute(query, entity, transaction);
//                return entity; // Retorna el objeto creado
//            }



//            // Actualizar un registro
//            public void Update(T entity)
//            {
//                using (var connection = _context.Connection)
//                {
//                    connection.Open();
//                    var tableName = typeof(T).Name + "s"; // Obtiene el nombre de la tabla
//                    var setColumns = string.Join(", ", typeof(T).GetProperties().Select(p => $"{p.Name} = @{p.Name}")); // Columnas a actualizar
//                    var query = $"UPDATE {tableName} SET {setColumns} WHERE Id = @Id";
//                    connection.Execute(query, entity);
//                }
//            }

//            // Eliminar un registro
//            public void Delete(int id)
//            {
//                using (var connection = _context.Connection)
//                {
//                    connection.Open();
//                    var tableName = typeof(T).Name + "s"; // Obtiene el nombre de la tabla
//                    var query = $"DELETE FROM {tableName} WHERE Id = @Id";
//                    connection.Execute(query, new { Id = id });
//                }
//            }
//        }
//    }


//}
using Newtonsoft.Json;
using System.Text;

namespace SistemaGestionTareas.Consumer
{
    public static class Crud<T>
    {
        public static string EndPoint { get; set; }

        // Método asincrónico para obtener todos los registros
        public static List<T> GetAll()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(EndPoint).Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }
        //obtener por id
        public static T GetById(int id)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{EndPoint}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }

        public static List<T> GetBy(string campo, int id)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{EndPoint}/{campo}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<T>>(json);

                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }

        }


        //crear nuevo
        public static T Create(T item)
        {
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(
                        EndPoint,
                        new StringContent(
                            JsonConvert.SerializeObject(item),
                            Encoding.UTF8,
                            "application/json"
                        )
                    ).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }
        //editar un registro    
        public static bool Update(int id, T item)
        {
            using (var client = new HttpClient())
            {
                var response = client.PutAsync(
                        $"{EndPoint}/{id}",
                        new StringContent(
                            JsonConvert.SerializeObject(item),
                            Encoding.UTF8,
                            "application/json"
                        )
                    ).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }
        //eliminar un registro
        public static bool Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{EndPoint}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
        }
    }
}