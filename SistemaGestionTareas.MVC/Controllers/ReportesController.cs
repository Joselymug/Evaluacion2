using Microsoft.AspNetCore.Mvc;
using SistemaGestionTareas.Consumer;

namespace SistemaGestionTareas.MVC.Controllers
{
    public class ReportesController : Controller
    {
        public ActionResult Index()
        {
            var tareas = Crud<Tarea>.GetAll();
            var usuarios = Crud<Usuario>.GetAll();
            var proyectos = Crud<Proyecto>.GetAll();
            var data = new
            {
                Tareas = tareas,
                Usuarios = usuarios,
                Proyectos = proyectos
            };
            return View(data);
        }
    }
}
