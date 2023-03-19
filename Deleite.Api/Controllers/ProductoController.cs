using System.Linq;
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
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Deleite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductoController : ControllerBase {
    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly IGenericRepository<Producto> _dbcontext;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ILoginToken _Idbcontext;
    private readonly DeleitebdContext deleitebdContext;

    public ProductoController(IWebHostEnvironment hostingEnvironment, IGenericRepository<Producto> dbcontext, IHttpContextAccessor httpContext, ILoginToken Idbcontext, DeleitebdContext deleitebdContext ) {
        _hostingEnvironment = hostingEnvironment;
        _dbcontext = dbcontext;
        _httpContext = httpContext;
        _Idbcontext = Idbcontext;
        this.deleitebdContext = deleitebdContext;
    }
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> getAll() {
        var result = await _dbcontext.getAll();
        var host = _httpContext.HttpContext.Request.Host.Value;

        List<DtoresultP> resultados = new List<DtoresultP>();
        foreach (var item in result)
        {
            var imagenExiste = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", item.ImagenPrincipal);
            var imagenPredetermindad = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "imagenPredetermindad.png");
            var urlFoto = System.IO.File.Exists(imagenExiste) ? Url.Content($"~/{item.ImagenPrincipal}") : Url.Content($"~/imagenPredetermindad.png");


            //var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "imagenPredetermindad.png");
            //var urlFoto = Url.Content($"~/{"imagenPredetermindad.png"}");
            //var bytess = System.IO.File.Exists(rutas) ? System.IO.File.ReadAllBytes(rutas) : System.IO.File.ReadAllBytes(rutas2);
            //var base64Strings = Convert.ToBase64String(bytess);
            var resultado2 = new DtoresultP {
                IdProducto = item.IdProducto,
                IdConfirmacionT = item.IdConfirmacionT,
                Base64 = "https://" + host + urlFoto,
                Nombre = urlFoto,
                Tipo = "image/png",
                NombreP = item.NombreP,
                DescripcionP = item.DescripcionP,
                Precio = item.Precio,
                Ingredienteselect = item.Ingredienteselect,
                NombreTematica = item.IdTematicaNavigation.NombreT,
                NombreCategoria = item.IdCategoriaNavigation.Nombre
            };

            resultados.Add(resultado2);

        }
        return Ok(resultados);
    }
    [HttpGet]
    [Route("get/{id}")]
    public async Task<IActionResult> GetByFilter(int id)
    {
        var result = await _dbcontext.Obtener(x => x.IdProducto.Equals(id));
        return Ok(result);
    }
    [HttpGet]
    [Route("mostrarimg")]
    public async Task<IActionResult> mostrarimagenes() {


        var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "34567154-7ab3-456b-90b6-0eb1ba52ea96.png");
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
        var host = _httpContext.HttpContext.Request.Host.Value;

        List<resultado> resultados = new List<resultado>();
        foreach (var item in result)
        {


            var imagenExiste = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", item.NombreFoto);
            var imagenExistePrincipal = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", item.IdProductoNavigation.ImagenPrincipal);

            var imagenPredetermindad = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "imagenPredetermindad.png");

            var urlFoto = System.IO.File.Exists(imagenExiste) ? Url.Content($"~/{item.NombreFoto}") : Url.Content($"~/imagenPredetermindad.png");
            var urlFotoPrincipal = System.IO.File.Exists(imagenExistePrincipal) ? Url.Content($"~/{item.IdProductoNavigation.ImagenPrincipal}") : Url.Content($"~/imagenPredetermindad.png");

            var resultado2 = new resultado {
                Base64Origihal = "https://" + host + urlFotoPrincipal,
                Base64 = "https://" + host + urlFoto,
                Nombre = "3bcddd15-ad6c-4699-a40d-c984e890fb9c.png",
                Tipo = "image/png",
                NombreP = item.IdProductoNavigation.NombreP,
                DescripcionP = item.IdProductoNavigation.DescripcionP,
                Precio = item.IdProductoNavigation.Precio,
                categoria = item.IdProductoNavigation.IdCategoriaNavigation.Nombre,
                tematica = item.IdProductoNavigation.IdTematicaNavigation.NombreT,
                Popular = item.IdProductoNavigation.Popular,
                Ingredienteselect = item.IdProductoNavigation.Ingredienteselect,
                Saludable = item.IdProductoNavigation.Saludable
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

    [HttpGet("filtrar/nombre")]
    public async Task<IEnumerable<Producto>> FiltrarNombre(string nombre)
    {
        return await deleitebdContext.Productos.Where(
            p => p.NombreP.Contains(nombre)).ToListAsync();
    }

    [HttpGet("filtrar/tematica")]
    public async Task<List<Tematica>> FiltrarTematica(string tematica)
    {
        return await deleitebdContext.Tematicas.Where(
          p => p.NombreT.Contains(tematica)).ToListAsync();
    }




    [HttpPost]
    [Route("create")]
    
    public async Task<IActionResult> create([FromBody] DtoProduco producto)
    {

        var createadd = await _dbcontext.CrearProducto(producto);
        if (createadd == null)
            return Conflict("El registro no pudo ser realizado");
        //var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/Producto/{createadd.IdProducto}";
        //return Created(result, createadd.IdProducto);
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent("El usuario con el id " + createadd.IdProducto + "fue creado o actualizado");
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
    public async Task<IActionResult> AgregarFotosProducto([FromBody] DtoImagenProducto imgs){
         var createadd = await _dbcontext.AddImageProducto(imgs);
        if (createadd == null)
            return Conflict("El registro no pudo ser realizado");
        //var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/Producto/{createadd.IdProducto}";
        //return Created(result, createadd.IdProducto);
        return Ok(createadd);
    }
    /*[HttpDelete]
    [Route("DeleteImg/{id}")]
    public async Task<IActionResult>BorrarFoto(int id)
    {
        var delteimg = await _dbcontext.borrarimagen(id);
        if (!delteimg)
            return NotFound("imagen no encontrada");
        return Ok("imagen borrada con exito");
    }*/

    [HttpPut]
    [Route("update/{id}")]
    public async Task<IActionResult> Editar(int id, [FromForm] Producto producto)
    {
        var productoToUpdate = await _dbcontext.Obtener(x => x.IdProducto == id);
        if (productoToUpdate == null)
            return NotFound("El producto no existe");

        productoToUpdate.IdProducto = producto.IdProducto;

        var updated = await _dbcontext.Editar(productoToUpdate);
        if (!updated)
            return Conflict("El registro no pudo ser actualizado");
        return Ok("EXITO");
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var categoriaToDelete = await _dbcontext.Obtener(x => x.IdProducto == id);
        if (categoriaToDelete == null)
            return NotFound("La categoria no existe");

        var deleted = await _dbcontext.borrarimagenProducto(categoriaToDelete);
        if (!deleted)
            return Conflict("El registro no pudo ser eliminado");
        return Ok("Producto borrado exitosamente");
    }
    [HttpGet]
    [Route("fotos")]
    public dynamic NumeroDeFotos() {
        var rutas = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        int count = Directory.GetFiles(rutas).Length;
        return new {
            succes = true,
            message = "hay un total de " + count + "en el proyecto",
            result =count
        };
    }
    
    [HttpPost]
    [Route("Token/{id}")]
    [Authorize]
    public dynamic Token(int Id) 
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
