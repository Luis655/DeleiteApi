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
            [Route("borrarimagenes/{id}")]
            public async Task<IActionResult> borrarImagenes(int id){
                var borrarImagen = await _IgenericRepository.borrarimagen(id);
                if (borrarImagen==false){
                   return Ok("no se pudo borrar");
                }
                else{
                return Ok("se borro con exito la imagen");
                }
                
            }
            [HttpDelete]
            [Route("borrarimagen/{id}")]
            public async Task<IActionResult> Eliminar(int id)
            {
                var categoriaToDelete = await _IgenericRepository.Obtener(x => x.IdimgProducto == id);
                if (categoriaToDelete == null)
                    return NotFound("La imagen no existe");

                var deleted = await _IgenericRepository.EliminarFotoProducto(categoriaToDelete);
                if (!deleted)
                    return Conflict("El registro no pudo ser eliminado");

                return NoContent();
            }
        
    }
}