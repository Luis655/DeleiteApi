using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Deleite.Bll.Jwt;
using static Deleite.Bll.Jwt.Jwt;
using Deleite.Entity.DtoModels;

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
    [HttpGet]
    [Route("mostrarimg")]
    public async Task<IActionResult> mostrarimagenes(){


        var ruta = Path.Combine(Directory.GetCurrentDirectory(), "fotos", "3bcddd15-ad6c-4699-a40d-c984e890fb9c.png");
        var bytes = System.IO.File.ReadAllBytes(ruta);
        var base64String = Convert.ToBase64String(bytes);
        var resultado = new {
            Base64 = base64String,
            Nombre = "3bcddd15-ad6c-4699-a40d-c984e890fb9c.png",
            Tipo = "image/png"
            };

        return Ok(resultado);

    }
    
    /**
    *
    * Controller getallimages
    *
    * este controlador llama a todas las imagenes que perteneces a un producto
    * 
    * @package	.net 6 webapi
    * @category	controller
    * @author    luis macias <luissmh150@gmail.com>
    * @link      /api/Producto/getimages/id
    * @param     @id, es el id del producto
    * @return    images base64 list json.
    *
    */
    [HttpGet]
    [Route("getimages/{id}")]
    public async Task<IActionResult> getallimages(int id)
    {
        
        var result = await _dbcontext.Consultarimgs(x => x.IdProducto == id);
        List<resultado> resultados = new List<resultado>();
        foreach (var item in result)
        {
            var rutas = Path.Combine(Directory.GetCurrentDirectory(), "fotos", item.NombreFoto);
            var bytess = System.IO.File.ReadAllBytes(rutas);
            var base64Strings = Convert.ToBase64String(bytess);
            var resultado2 = new resultado {
            Base64 = base64Strings,
            Nombre = "3bcddd15-ad6c-4699-a40d-c984e890fb9c.png",
            Tipo = "image/png"
            };

            resultados.Add(resultado2);

        }
        return Ok(resultados);
    }
        /**
    *
    * Controller create
    *
    * crea un nuevo producto, se puede agregar 1 sola imagen en este controlador
    * 
    * @package	.net 6 webapi
    * @category	controller
    * @author    luis macias <luissmh150@gmail.com>
    * @link      /api/Producto/create
    * @param     @DtoProducto, devuelve un objeto de tipo append en el front y se recibe como [FromForm], 
    *            con los datos del producto, la imagen se manda como base64.
    * @return    image base64 json.
    *
    */
    [HttpPost]
    [Route("create")]
    
    public async Task<IActionResult> create([FromForm] DtoProduco producto)
    {

        var createadd = await _dbcontext.CrearProducto(producto);
        if (createadd == null)
            return Conflict("El registro no pudo ser realizado");
        //var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/Producto/{createadd.IdProducto}";
        //return Created(result, createadd.IdProducto);
        return Ok(createadd);
    }
            /**
    *
    * Controller addImage
    *
    * permite agregar multiples fotos a un producto, una consulta por foto.
    * 
    * @package	.net 6 webapi
    * @category	 controller
    * @author    luis macias <luissmh150@gmail.com>
    * @link      /api/Producto/addImage
    * @param     @DtoImagenProducto, devuelve un objeto de tipo append en el front y se recibe como [FromForm], 
    *            con los datos de la imagen del producto, la imagen se manda como base64.
                 @idProducto, @imagenBase64.
    * @return    images base64 json.
    *
    */
    [HttpPost]
    [Route("addImage")]
    public async Task<IActionResult> AgregarFotosProducto([FromForm] DtoImagenProducto imgs){
         var createadd = await _dbcontext.AddImageProducto(imgs);
        if (createadd == null)
            return Conflict("El registro no pudo ser realizado");
        //var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/Producto/{createadd.IdProducto}";
        //return Created(result, createadd.IdProducto);
        return Ok(createadd);
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
    [Authorize]
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
