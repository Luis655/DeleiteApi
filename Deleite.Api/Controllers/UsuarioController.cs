using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Deleite.Api.Controllers;

[ApiController]
[Route("usuario")]
public class UsuarioController : ControllerBase
{
    public IConfiguration _configuration;
    public UsuarioController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private readonly IGenericRepository<Usuario> _dbcontext;
    private readonly IHttpContextAccessor _httpContext;
    public UsuarioController(IGenericRepository<Usuario> dbcontext, IHttpContextAccessor httpContext)
    {
        _dbcontext = dbcontext;
        _httpContext = httpContext;
    }


    [HttpPost]
    [Route("login")]
    public dynamic IniciarSesion([FromBody] Object optData)
    {
        var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

        string user = data.Nombre.ToString();
        string password = data.Password.ToString();

        Usuario usuario = Usuario.DB().Where(x => x.Nombre == user && x.Contraseña == password).FirstOrDefault();

        if (usuario == null)
        {
            return new
            {
                success = false,
                message = "Credenciales incorrectas",
                result = ""
            };
        }

        var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
           // new Claim("IdUsuario", usuario.IdUsuario),
            new Claim("Nombre", usuario.Nombre),
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
            message= "exito",
            result= new JwtSecurityTokenHandler().WriteToken(token)
        };
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


    [HttpPut]
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
    }
}
