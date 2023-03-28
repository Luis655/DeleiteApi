using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Deleite.Entity.DtoModels;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace Deleite.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalificacionController : ControllerBase
{

    private readonly IGenericRepository<Calificacione> _dbcontext;
    private readonly IHttpContextAccessor _httpContext;
    private readonly DeleitebdContext deleitebdContext;
    private readonly IMapper mapper;

    public CalificacionController(IGenericRepository<Calificacione> dbcontext, IHttpContextAccessor httpContext, DeleitebdContext deleitebdContext, IMapper mapper)
    {
        _dbcontext = dbcontext;
        _httpContext = httpContext;
        this.deleitebdContext = deleitebdContext;
        this.mapper = mapper;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> create([FromBody] Calificacione tematica)
    {
        var createadd = await _dbcontext.Crear(tematica);
        if (createadd == null)
            return Conflict("El registro no pudo ser realizado");
        var result = $"https://{_httpContext.HttpContext.Request.Host.Value}/api/artesania/{createadd.Idcalificacion}";
        return Created(result, createadd.Idcalificacion);
    }
}