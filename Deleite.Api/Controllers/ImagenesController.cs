using System.ComponentModel;
using Deleite.Dal.Interfaces;
using Deleite.Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deleite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenesController : ControllerBase
    {
        public readonly IGenericRepository<ImagenProducto> _IgenericRepository;
        public ImagenesController(IGenericRepository<ImagenProducto> IgenericRepository)
        {
            _IgenericRepository = IgenericRepository;
            
            
        }
            [HttpDelete]
            [Route("delete/{id}")]
            public async Task<IActionResult> borrarImagen(int id){
                var borrarImagen = await _IgenericRepository.borrarimagen(id);
                if (!borrarImagen)
                   return Ok("no se pudo borrar");
                return Ok("se borro con exito la imagen");
                
            }
        
    }
}