//using Microsoft.AspNetCore.Mvc;
//using SistemaGestionTareas;
//using SistemaGestionTareas.API.Data;
//using SistemaGestionTareas.Consumer;

//namespace SistemaGestionTareas.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProyectosController : ControllerBase
//    {
//        private readonly CrudService<Proyecto> _crudService;
//        private readonly AppDbContext _context;

//        // Constructor
//        public ProyectosController(CrudService<Proyecto> crudService)
//        {
//            _crudService = crudService;
//        }

//        // GET: api/proyectos
//        [HttpGet]
//        public IActionResult Get()
//        {
//            var proyectos = _crudService.GetAll();
//            return Ok(proyectos);
//        }

//        // GET: api/proyectos/1
//        [HttpGet("{id}")]
//        public IActionResult GetById(int id)
//        {
//            var proyecto = _crudService.GetById(id);
//            if (proyecto == null)
//            {
//                return NotFound(); 
//            }
//            return Ok(proyecto);
//        }

//        // POST: api/proyectos
//        [HttpPost]
//        public IActionResult Post([FromBody] Proyecto proyecto)
//        {
//            _crudService.Create(proyecto); 
//            return CreatedAtAction(nameof(GetById), new { id = proyecto.Id }, proyecto); 
//        }

//        // PUT: api/proyectos/1
//        [HttpPut("{id}")]
//        public IActionResult Put(int id, [FromBody] Proyecto proyecto)
//        {
//            if (id != proyecto.Id)
//            {
//                return BadRequest(); 
//            }

//            _crudService.Update(proyecto); 
//            return NoContent();
//        }

//        // DELETE: api/proyectos/1
//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {
//            _crudService.Delete(id); 
//            return NoContent();
//        }
//    }
//}

//using Microsoft.AspNetCore.Mvc;
//using SistemaGestionTareas;
//using SistemaGestionTareas.API.Data;
//using SistemaGestionTareas.Consumer;

//namespace SistemaGestionTareas.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProyectosController : ControllerBase
//    {
//        private readonly CrudService<Proyecto> _crudService;
//        private readonly AppDbContext _context;

//        // Constructor
//        public ProyectosController(CrudService<Proyecto> crudService)
//        {
//            _crudService = crudService ?? throw new ArgumentNullException(nameof(crudService)); // Verifica que el servicio esté inyectado correctamente
//        }

//        // GET: api/proyectos
//        [HttpGet]
//        public IActionResult Get()
//        {
//            var proyectos = _crudService.GetAll();
//            return Ok(proyectos);
//        }

//        // GET: api/proyectos/1
//        [HttpGet("{id}")]
//        public IActionResult GetById(int id)
//        {
//            var proyecto = _crudService.GetById(id);
//            if (proyecto == null)
//            {
//                return NotFound();
//            }
//            return Ok(proyecto);
//        }

//        // POST: api/proyectos
//        [HttpPost]
//        public IActionResult Post([FromBody] Proyecto proyecto)
//        {
//            _crudService.Create(proyecto);
//            return CreatedAtAction(nameof(GetById), new { id = proyecto.Id }, proyecto);
//        }

//        // PUT: api/proyectos/1
//        [HttpPut("{id}")]
//        public IActionResult Put(int id, [FromBody] Proyecto proyecto)
//        {
//            if (id != proyecto.Id)
//            {
//                return BadRequest();
//            }

//            _crudService.Update(proyecto);
//            return NoContent();
//        }

//        // DELETE: api/proyectos/1
//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {
//            _crudService.Delete(id);
//            return NoContent();
//        }
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaGestionTareas;
using SistemaGestionTareas.Services;
using System.Security.Claims;

namespace SistemaGestionTareas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProyectosController : ControllerBase
    {
        private readonly IDataService _dataService;

        public ProyectosController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _dataService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _dataService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var projectId = await _dataService.CreateProjectAsync(request, userId);
            var project = await _dataService.GetProjectByIdAsync(projectId);
            return CreatedAtAction(nameof(GetProject), new { id = projectId }, project);
        }

        [HttpGet("{id}/tasks")]
        public async Task<IActionResult> GetProjectTasks(int id)
        {
            var tasks = await _dataService.GetTasksByProjectIdAsync(id);
            return Ok(tasks);
        }
    }
}
