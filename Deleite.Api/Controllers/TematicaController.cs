using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Deleite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TematicaController : ControllerBase
{

    private readonly IGenericRepository<Tematica> _dbcontext;
    private readonly IHttpContextAccessor _httpContext;
    private readonly DeleitebdContext deleitebdContext;

    public TematicaController(IGenericRepository<Tematica> dbcontext, IHttpContextAccessor httpContext, DeleitebdContext deleitebdContext)
    {
        _dbcontext = dbcontext;
        _httpContext = httpContext;
        this.deleitebdContext = deleitebdContext;
    }

    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _dbcontext.ObtenerTodos();
        return Ok(result);
    }
    [HttpGet]
    [Route("tematica/productos")]

    public async Task<ActionResult> GetTematicaConProductos()
    {
        var result = await deleitebdContext.Tematicas
            .Include(c => c.Productos)   
               .ToListAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Tematica>> GetByFilter(int id)
    {
        var result = await _dbcontext.Obtener(x => x.IdTematica.Equals(id));
        if (result == null)
            return NotFound();
        return Ok(result);

    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> create([FromBody] Tematica tematica)
    {
        var createadd = await _dbcontext.Crear(tematica);
        if (createadd == null)
            return Conflict("El registro no pudo ser realizado");
        var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/artesania/{createadd.IdTematica}";
        return Created(result, createadd.IdTematica);
    }

    /*    [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Editar(int id, [FromBody] Tematica tematica)
        {
            var tematicaToUpdate = await _dbcontext.Obtener(x => x.IdTematica == id);
            if (tematicaToUpdate == null)
                return NotFound("La tematica no existe");

            tematicaToUpdate.IdTematica = tematica.IdTematica;

            var updated = await _dbcontext.Editar(tematicaToUpdate);
            if (!updated)
                return Conflict("El registro no pudo ser actualizado");

            return NoContent();
        }*/


    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Tematica tematica)
    {
        if (tematica.IdTematica != id)
        {
            return BadRequest("El id de la temática proporcionada no coincide con el id de la URL.");
        }

        var existingTematica = await _dbcontext.Obtener(x => x.IdTematica.Equals(id));
        if (existingTematica == null)
        {
            return NotFound();
        }

        existingTematica.NombreT = tematica.NombreT;
        // Agregar todas las propiedades que se deseen actualizar

        var updatedTematica = await _dbcontext.Editar(existingTematica);
        if (updatedTematica == null)
        {
            return Conflict("La actualización no se pudo realizar.");
        }

        return NoContent();
    }


    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var tematicaToDelete = await _dbcontext.Obtener(x => x.IdTematica == id);
        if (tematicaToDelete == null)
            return NotFound("La tematica no existe");

        var deleted = await _dbcontext.Eliminar(tematicaToDelete);
        if (!deleted)
            return Conflict("El registro no pudo ser eliminado");

        return NoContent();
    }

}
