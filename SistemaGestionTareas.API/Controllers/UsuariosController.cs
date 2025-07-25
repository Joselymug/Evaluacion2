using Microsoft.AspNetCore.Mvc;
using SistemaGestionTareas;
using SistemaGestionTareas.API.Data;

namespace SistemaGestionTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly CrudService<Usuario> _crudService;

        // Constructor
        public UsuariosController(CrudService<Usuario> crudService)
        {
            _crudService = crudService;
        }

        // GET: api/usuarios
        [HttpGet]
        public IActionResult Get()
        {
            var usuarios = _crudService.GetAll();
            return Ok(usuarios);
        }

        // GET: api/usuarios/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var usuario = _crudService.GetById(id);
            if (usuario == null)
            {
                return NotFound(); 
            }
            return Ok(usuario); 
        }

        // POST: api/usuarios
        [HttpPost]
        public IActionResult Post([FromBody] Usuario usuario)
        {
            _crudService.Create(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario); 
        }

        // PUT: api/usuarios/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest(); 
            }

            _crudService.Update(usuario); 
            return NoContent(); 
        }

        // DELETE: api/usuarios/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _crudService.Delete(id); 
            return NoContent(); 
        }
    }
}
