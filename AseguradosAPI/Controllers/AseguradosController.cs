using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AseguradosAPI.Models;
using AseguradosAPI.Data;

namespace AseguradosAPI.Controllers
{
    // Clase de controlador para el controlador Asegurados
    [Route("api/[controller]")]
    [ApiController]
    public class AseguradosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AseguradosController(AppDbContext context)
        {
            _context = context;
        }

        // ================================================
        // MÉTODO AUXILIAR: Verifica si un asegurado existe
        // ================================================
        private bool AseguradoExiste(int id)
        {
            return _context.Asegurados.Any(e => e.NumeroIdentificacion == id);
        }

        // ================================================
        // 1. CREAR ASEGURADO
        // ================================================
        [HttpPost]
        public async Task<ActionResult<Asegurado>> CrearAsegurado(Asegurado asegurado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Valida los datos según las anotaciones del modelo
            }

            // Verifica si el número de identificación ya existe
            if (_context.Asegurados.Any(a => a.NumeroIdentificacion == asegurado.NumeroIdentificacion))
            {
                return Conflict("El número de identificación ya está registrado.");
            }

            // Creación de la entidad Asegurado en la base de datos
            _context.Asegurados.Add(asegurado);
            await _context.SaveChangesAsync();

            // Retorno de la entidad Asegurado creada
            return CreatedAtAction(nameof(ObtenerAseguradoPorId), new { id = asegurado.NumeroIdentificacion }, asegurado);
        }

        // ================================================
        // 2. OBTENER TODOS LOS ASEGURADOS (CON PAGINACIÓN)
        // ================================================
        [HttpGet]
        // Método GET para obtener todos los asegurados
        public async Task<ActionResult<IEnumerable<Asegurado>>> ObtenerAsegurados(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 10)
        {
            // Validación de parámetros de paginación
            if (pagina < 1 || tamanoPagina < 1)
            {
                return BadRequest("Los parámetros de paginación deben ser mayores a 0.");
            }

            // Obtención de la cantidad total de asegurados
            var totalAsegurados = await _context.Asegurados.CountAsync();
            var asegurados = await _context.Asegurados
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            // Cabeceras personalizadas para información de paginación
            Response.Headers.Add("X-Total-Registros", totalAsegurados.ToString());
            Response.Headers.Add("X-Pagina-Actual", pagina.ToString());
            Response.Headers.Add("X-Tamano-Pagina", tamanoPagina.ToString());

            return Ok(asegurados);
        }

        // ================================================
        // 3. OBTENER UN ASEGURADO POR ID (FILTRO POR NÚMERO DE IDENTIFICACIÓN)
        // ================================================
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Asegurado>> ObtenerAseguradoPorId(int id)
        {
            // Obtención de la entidad Asegurado con el ID especificado
            var asegurado = await _context.Asegurados.FindAsync(id);

            // Validación de la existencia de la entidad Asegurado
            if (asegurado == null)
            {
                return NotFound("Asegurado no encontrado.");
            }

            return Ok(asegurado);
        }

        // ================================================
        // 4. ACTUALIZAR UN ASEGURADO
        // ================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAsegurado(int id, Asegurado asegurado)
        {
            // Validación de que el ID en la URL coincida con el ID del asegurado
            if (id != asegurado.NumeroIdentificacion)
            {
                return BadRequest("El ID en la URL no coincide con el ID del asegurado.");
            }

            // Validación de los datos del asegurado
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Actualización de la entidad Asegurado en la base de datos
            _context.Entry(asegurado).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AseguradoExiste(id))
                {
                    return NotFound("Asegurado no encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204 No Content: Actualización exitosa sin retornar datos
        }

        // ================================================
        // 5. ELIMINAR UN ASEGURADO
        // ================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAsegurado(int id)
        {
            // Obtención de la entidad Asegurado con el ID especificado
            var asegurado = await _context.Asegurados.FindAsync(id);
            if (asegurado == null)
            {
                return NotFound("Asegurado no encontrado.");
            }

            _context.Asegurados.Remove(asegurado);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content: Eliminación exitosa
        }
    }
}