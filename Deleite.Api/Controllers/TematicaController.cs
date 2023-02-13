using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
namespace Deleite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TematicaController : ControllerBase
{

    private readonly IGenericRepository<Tematica> _dbcontext;
    private readonly IHttpContextAccessor _httpContext;
    public TematicaController(IGenericRepository<Tematica> dbcontext, IHttpContextAccessor httpContext)
    {
        _dbcontext = dbcontext;
        _httpContext = httpContext;
    }

    [HttpGet]
    [Route("get/{filtro}")]
    public async Task<IActionResult> GetByFilter(string filtro)
    {
        var result = await _dbcontext.Obtener(x => x.IdTematica.Equals(filtro));
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

    [HttpPut]
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
