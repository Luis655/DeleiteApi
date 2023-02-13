using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Deleite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductoController : ControllerBase
{

    private readonly IGenericRepository<Producto> _dbcontext;
    private readonly IHttpContextAccessor _httpContext;
    public ProductoController(IGenericRepository<Producto> dbcontext, IHttpContextAccessor httpContext)
    {
        _dbcontext = dbcontext;
        _httpContext = httpContext;
    }
    [HttpGet]
    [Route("get/{filtro}")]
    public async Task<IActionResult> GetByFilter(string filtro)
    {
        var result = await _dbcontext.Obtener(x => x.IdProducto.Equals(filtro));
        return Ok(result);
    }
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> create([FromBody] Producto producto)
    {
        var createadd = await _dbcontext.Crear(producto);
        if (createadd == null)
            return Conflict("El registro no pudo ser realizado");
        var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/artesania/{createadd.IdProducto}";
        return Created(result, createadd.IdProducto);
    }

    [HttpPut]
    [Route("update/{id}")]
    public async Task<IActionResult> Editar(int id, [FromBody] Producto producto)
    {
        var productoToUpdate = await _dbcontext.Obtener(x => x.IdProducto == id);
        if (productoToUpdate == null)
            return NotFound("El producto no existe");

        productoToUpdate.IdProducto = producto.IdProducto;

        var updated = await _dbcontext.Editar(productoToUpdate);
        if (!updated)
            return Conflict("El registro no pudo ser actualizado");

        return NoContent();
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var categoriaToDelete = await _dbcontext.Obtener(x => x.IdProducto == id);
        if (categoriaToDelete == null)
            return NotFound("La categoria no existe");

        var deleted = await _dbcontext.Eliminar(categoriaToDelete);
        if (!deleted)
            return Conflict("El registro no pudo ser eliminado");

        return NoContent();
    }

    [HttpPost]
    [Route("eliminar")]
    [Authorize]

    public dynamic eliminarProducto(Producto producto) 
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        var rToken = Jwt.validarToken(identity);

        if(!rToken.success) return rToken;

        Usuario usuario = rToken.result;

        if(usuario.rol != "Administrador")
        {
            return new
            {
                success = true,
                message = "No tienes permiso para eliminar productos",
                result = ""
            };
        }

            return new
            {
                success = true,
                message = "producto eliminado",
                result = producto
            };
    }

}
