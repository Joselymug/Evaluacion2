using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SistemaGestionTareas.API.Data;


namespace SistemaGestionTareas.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSingleton<AppDbContext>(sp =>
                new AppDbContext(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configuracion del servicio CrudService con los tipos apropiados
            builder.Services.AddScoped<CrudService<Tarea>>();
            builder.Services.AddScoped<CrudService<Proyecto>>();
            builder.Services.AddScoped<CrudService<Usuario>>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Realiza la prueba de conexión al iniciar la aplicación (si lo necesitas)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //var dbTest = services.GetRequiredService<DbTest>();
                //dbTest.TestConnection();  // Llamar a la prueba de conexión
            }

            app.MapControllers();

            app.Run();
        }
    }
}
