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

        public UsuariosController(CrudService<Usuario> crudService)
        {
            _crudService = crudService;
        }

        // GET: api/usuarios
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var usuarios = _crudService.GetAll();  // Obtiene todos los usuarios
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var usuario = _crudService.GetById(id);  // Obtiene el usuario por ID
                if (usuario == null)
                {
                    return NotFound();  // Si no se encuentra el usuario, devuelve 404
                }
                return Ok(usuario);  // Devuelve el usuario encontrado
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/usuarios
        [HttpPost]
        public IActionResult Post([FromBody] Usuario usuario)
        {
            try
            {
                _crudService.Create(usuario);  // Crea un nuevo usuario
                return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);  // Devuelve el usuario creado con un 201
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/usuarios/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (id != usuario.Id)
                {
                    return BadRequest();  // Si los IDs no coinciden, devuelve un 400
                }
                _crudService.Update(usuario);  // Actualiza el usuario
                return NoContent();  // Devuelve 204 si todo salió bien
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _crudService.Delete(id);  // Elimina el usuario
                return NoContent();  // Devuelve 204 si todo salió bien
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
