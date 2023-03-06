using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Deleite.Dal.DBContext;
using FluentValidation;
using Deleite.Entity.DtoModels;

namespace Deleite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    public IConfiguration _configuration;
    private readonly IGenericRepository<Usuario> _dbcontext;
    private readonly ILoginToken _dbrepo;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IValidator<DtoUserLogin> _createValidator;
    public UsuarioController(IGenericRepository<Usuario> dbcontext, ILoginToken dbrepo, IHttpContextAccessor httpContext, IConfiguration configuration, IValidator<DtoUserLogin> CreateValidator)
    {
        _createValidator = CreateValidator;
        _dbcontext = dbcontext;
        _httpContext = httpContext;
        _configuration = configuration;
        _dbrepo =dbrepo;
    }

    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _dbcontext.ObtenerTodos();
        return Ok(result);
    }


    [HttpPost]
    [Route("login")]
    public async Task<dynamic> IniciarSesion([FromBody] DtoUserLogin usuario)
    {
        var ValidationResult = await _createValidator.ValidateAsync(usuario);
        if(!ValidationResult.IsValid)
            return UnprocessableEntity(ValidationResult.Errors.Select(x => $"Error: {x.ErrorMessage}"));
        /*var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
        if (data==null)return new{message="ocurrio un error"};        

        string user = data.Nombre.ToString();
        string password = data.Contraseña.ToString();*/

        var user = _dbrepo.getuserDto(usuario);
        

        //Usuario usuario = Usuario.DB().Where(x => x.Nombre == user && x.Contraseña == password).FirstOrDefault();

        if (user.Result==null)
        {
            return new
            {
                success = false,
                message = "Credenciales incorrectas",
                result = ""
            };
        }else
            {

                var jwt = _configuration.GetSection("Jwt").Get<JwtModel>();

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Correo", usuario.Correo.ToString()),
                    //new Claim("Nombre", usuario.Nombre)
                };

                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var singIn = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: singIn
                    );


                return new
                {
                    success = true,
                    message= "Exito",
                    result= new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
    }


    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> create([FromBody] Usuario usuario)
    {
        var createadd = await _dbcontext.Crear(usuario);
        if (createadd == null)
            return Conflict("El registro no pudo ser realizado");
        var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/artesania/{createadd.IdUsuario}";
        return Created(result, createadd.IdUsuario);
    }


    /* [HttpPut]
     [Route("update/{id}")]
     public async Task<IActionResult> Editar(int id, [FromBody] Usuario usuario)
     {
         var usuarioToUpdate = await _dbcontext.Obtener(x => x.IdUsuario == id);
         if (usuarioToUpdate == null)
             return NotFound("El usuario no existe");

         usuarioToUpdate.IdUsuario = usuario.IdUsuario;

         var updated = await _dbcontext.Editar(usuarioToUpdate);
         if (!updated)
             return Conflict("El registro no pudo ser actualizado");

         return NoContent();
     }*/


    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Usuario usuario)
    {
        if (usuario.IdUsuario != id)
        {
            return BadRequest("El id del usuario proporcionado no coincide con el id de la URL.");
        }

        var existingUsuario = await _dbcontext.Obtener(x => x.IdUsuario.Equals(id));
        if (existingUsuario == null)
        {
            return NotFound();
        }

        existingUsuario.Nombre = usuario.Nombre;
        existingUsuario.Contraseña  = usuario.Contraseña;
        // Agregar todas las propiedades que se deseen actualizar

        var updatedUsuario = await _dbcontext.Editar(existingUsuario);
        if (updatedUsuario == null)
        {
            return Conflict("La actualización no se pudo realizar.");
        }

        return NoContent();
    }

}
