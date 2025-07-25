using Microsoft.AspNetCore.Mvc;
using SistemaGestionTareas;
using SistemaGestionTareas.API.Data;

namespace SistemaGestionTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly CrudService<Tarea> _crudService;

        public TareasController(CrudService<Tarea> crudService)
        {
            _crudService = crudService;
        }

        // GET: api/tareas
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var tareas = _crudService.GetAll();  // Obtiene todas las tareas
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/tareas/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var tarea = _crudService.GetById(id);  // Obtiene la tarea por ID
                if (tarea == null)
                {
                    return NotFound();  // Si no se encuentra, devuelve 404
                }
                return Ok(tarea);  // Devuelve la tarea encontrada
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/tareas
        [HttpPost]
        public IActionResult Post([FromBody] Tarea tarea)
        {
            try
            {
                _crudService.Create(tarea);  // Crea una nueva tarea
                return CreatedAtAction(nameof(GetById), new { id = tarea.Id }, tarea);  // Devuelve el proyecto creado con un 201
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/tareas/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Tarea tarea)
        {
            try
            {
                if (id != tarea.Id)
                {
                    return BadRequest();  // Si los IDs no coinciden, devuelve un 400
                }
                _crudService.Update(tarea);  // Actualiza la tarea
                return NoContent();  // Devuelve 204 si todo salió bien
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/tareas/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _crudService.Delete(id);  // Elimina la tarea
                return NoContent();  // Devuelve 204 si todo salió bien
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
