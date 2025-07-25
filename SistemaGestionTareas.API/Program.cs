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

            // Configuraci�n de la cadena de conexi�n a la base de datos
            builder.Services.AddSingleton<AppDbContext>(sp =>
                new AppDbContext(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configuraci�n del servicio CRUD para los modelos
            builder.Services.AddScoped<CrudService<Tarea>>();   // Para las tareas
            builder.Services.AddScoped<CrudService<Proyecto>>(); // Para los proyectos
            builder.Services.AddScoped<CrudService<Usuario>>();  // Para los usuarios

            // Aprende m�s sobre la configuraci�n de Swagger/OpenAPI en https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); // Habilita Swagger para la documentaci�n

            // Crear la aplicaci�n
            var app = builder.Build();

            // Configuraci�n de Swagger en el entorno de desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); // Interfaz de Swagger para probar la API
            }

            // Configurar el pipeline de solicitudes HTTP
            app.UseHttpsRedirection(); // Redirecci�n a HTTPS
            app.UseAuthorization(); // Autorizaci�n para los controladores

            // Aqu� puedes hacer la prueba de conexi�n si lo deseas
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                // Descomenta para hacer la prueba de conexi�n si es necesario
                // var dbTest = services.GetRequiredService<DbTest>();
                // dbTest.TestConnection();  // Llamar a la prueba de conexi�n
            }

            app.MapControllers(); // Mapea los controladores a las rutas HTTP

            // Ejecutar la aplicaci�n
            app.Run();
        }
    }
}
