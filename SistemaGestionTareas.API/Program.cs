using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SistemaGestionTareas.API.Data;
using SistemaGestionTareas.API.Repositories;
using SistemaGestionTareas.API.Repositories.Interfaces;

namespace SistemaGestionTareas.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Configuración de la cadena de conexión a la base de datos
            builder.Services.AddSingleton<AppDbContext>(sp =>
                new AppDbContext(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configuración del servicio CRUD para los modelos
            builder.Services.AddScoped<CrudService<Tarea>>();   // Para las tareas
            builder.Services.AddScoped<CrudService<Proyecto>>(); // Para los proyectos
            builder.Services.AddScoped<CrudService<Usuario>>();  // Para los usuarios

            // Aprende más sobre la configuración de Swagger/OpenAPI en https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); // Habilita Swagger para la documentación

            // Crear la aplicación
            var app = builder.Build();

            // Configuración de Swagger en el entorno de desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); // Interfaz de Swagger para probar la API
            }

            // Configurar el pipeline de solicitudes HTTP
            app.UseHttpsRedirection(); // Redirección a HTTPS
            app.UseAuthorization(); // Autorización para los controladores

            // Aquí puedes hacer la prueba de conexión si lo deseas
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                // Descomenta para hacer la prueba de conexión si es necesario
                // var dbTest = services.GetRequiredService<DbTest>();
                // dbTest.TestConnection();  // Llamar a la prueba de conexión
            }

            app.MapControllers(); // Mapea los controladores a las rutas HTTP

            // Ejecutar la aplicación
            app.Run();
        }
    }
}
