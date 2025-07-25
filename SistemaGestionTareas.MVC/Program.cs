using SistemaGestionTareas.Consumer;
using SistemaGestionTareas;

namespace SistemaGestionTareas.MVC
    
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Crud<Proyecto>.EndPoint = "https://localhost:7056/api/Proyectos";
            Crud<Tarea>.EndPoint = "https://localhost:7056/api/Tareas";
            Crud<Usuario>.EndPoint = "https://localhost:7056/api/Usuarios";
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
