using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Deleite.Bll.Jwt;
using static Deleite.Bll.Jwt.Jwt;

namespace Deleite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductoController : ControllerBase
{

    private readonly IGenericRepository<Producto> _dbcontext;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ILoginToken _Idbcontext;
    public ProductoController(IGenericRepository<Producto> dbcontext, IHttpContextAccessor httpContext, ILoginToken Idbcontext)
    {
        _dbcontext = dbcontext;
        _httpContext = httpContext;
        _Idbcontext = Idbcontext;
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
        var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/Producto/{createadd.IdProducto}";
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
    [Route("eliminar/{id}")]
    //[Authorize]
    public dynamic eliminarProducto(int Id) 
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        //var rToken = Jwt.validarToken(identity);
        //var rToken = (dynamic) _jwt.validarToken(identity);

        Jwt token = new Jwt(_Idbcontext);
        var rToken = token.validarToken(identity);
        
        if(!rToken.success) 
            return rToken;

        //Usuario usuario = rToken.result;
        string l = "LUIS";
        if(l != "LUIS")
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
                result = Id
            };
    }

}
