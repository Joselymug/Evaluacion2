using Microsoft.AspNetCore.Mvc;
using SistemaGestionTareas;
using SistemaGestionTareas.API.Data;

namespace SistemaGestionTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectosController : ControllerBase
    {
        private readonly CrudService<Proyecto> _crudService;

        // Constructor
        public ProyectosController(CrudService<Proyecto> crudService)
        {
            _crudService = crudService;
        }

        // GET: api/proyectos
        [HttpGet]
        public IActionResult Get()
        {
            var proyectos = _crudService.GetAll();
            return Ok(proyectos);
        }

        // GET: api/proyectos/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var proyecto = _crudService.GetById(id);
            if (proyecto == null)
            {
                return NotFound(); 
            }
            return Ok(proyecto);
        }

        // POST: api/proyectos
        [HttpPost]
        public IActionResult Post([FromBody] Proyecto proyecto)
        {
            _crudService.Create(proyecto); 
            return CreatedAtAction(nameof(GetById), new { id = proyecto.Id }, proyecto); 
        }

        // PUT: api/proyectos/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Proyecto proyecto)
        {
            if (id != proyecto.Id)
            {
                return BadRequest(); 
            }

            _crudService.Update(proyecto); 
            return NoContent();
        }

        // DELETE: api/proyectos/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _crudService.Delete(id); 
            return NoContent();
        }
    }
}
