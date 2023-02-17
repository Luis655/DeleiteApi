using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Deleite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriaController : ControllerBase
{

    private readonly IGenericRepository<Categoria> _dbcontext;
    private readonly IHttpContextAccessor _httpContext;
    public CategoriaController(IGenericRepository<Categoria> dbcontext, IHttpContextAccessor httpContext)
    {
        _dbcontext = dbcontext;
        _httpContext = httpContext;
    }

    /*[HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Ok("200");
    }*/
    /**
    *
    * Controller Categorias
    *
    * This is for the metods of Categorias of products
    * 
    * @package	.net 6 webapi
    * @category	controller
    * @author    luis macias <luissmh150@gmail.com>
    * @link      /
    * @param     @Categorias...
    * @return    url...
    *
    */

    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _dbcontext.ObtenerTodos();
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<Categoria>> GetByFilter(int id)
    {
        var result = await _dbcontext.Obtener(x => x.IdCategoria.Equals(id));
        if( result == null)
            return NotFound();
        return Ok(result);
      
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> create([FromBody] Categoria categoria){
        var createadd = await _dbcontext.Crear(categoria);
        if(createadd==null)
            return Conflict("El registro no pudo ser realizada");
        var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/artesania/{createadd.IdCategoria}";
        return Created(result, createadd.IdCategoria);
    }
    /* [HttpPut]
     [Route("update/{id}")]
     public async Task<IActionResult> Editar(int id, [FromBody] Categoria categoria)
     {
         var categoriaToUpdate = await _dbcontext.Obtener(x => x.IdCategoria == id);
         if (categoriaToUpdate == null)
             return NotFound("La categoria no existe");

         categoriaToUpdate.IdCategoria = categoria.IdCategoria;

         var updated = await _dbcontext.Editar(categoriaToUpdate);
         if (!updated)
             return Conflict("El registro no pudo ser actualizado");

         return NoContent();
     }*/

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Categoria categoria)
    {
        if (categoria.IdCategoria != id)
        {
            return BadRequest("El id de la categoria proporcionada no coincide con el id de la URL.");
        }

        var existingCategoria = await _dbcontext.Obtener(x => x.IdCategoria.Equals(id));
        if (existingCategoria == null)
        {
            return NotFound();
        }

        existingCategoria.Nombre = categoria.Nombre;
        existingCategoria.Imagen = categoria.Imagen;
        // Agregar todas las propiedades que se deseen actualizar

        var updatedCategoria = await _dbcontext.Editar(existingCategoria);
        if (updatedCategoria == null)
        {
            return Conflict("La actualización no se pudo realizar.");
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var categoriaToDelete = await _dbcontext.Obtener(x => x.IdCategoria == id);
        if (categoriaToDelete == null)
            return NotFound("La categoria no existe");

        var deleted = await _dbcontext.Eliminar(categoriaToDelete);
        if (!deleted)
            return Conflict("El registro no pudo ser eliminado");

        return NoContent();
    }


}
