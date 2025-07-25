using Microsoft.AspNetCore.Mvc;
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
            var tareas = _crudService.GetAll();
            return Ok(tareas);
        }

        // GET: api/tareas/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var tarea = _crudService.GetById(id);
            if (tarea == null)
            {
                return NotFound();
            }
            return Ok(tarea);
        }

        // POST: api/tareas
        [HttpPost]
        public IActionResult Post([FromBody] Tarea tarea)
        {
            _crudService.Create(tarea);
            return CreatedAtAction(nameof(GetById), new { id = tarea.Id }, tarea);
        }

        // PUT: api/tareas/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Tarea tarea)
        {
            if (id != tarea.Id)
            {
                return BadRequest();
            }

            _crudService.Update(tarea);
            return NoContent();
        }

        // DELETE: api/tareas/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _crudService.Delete(id);
            return NoContent();
        }
    }
}
