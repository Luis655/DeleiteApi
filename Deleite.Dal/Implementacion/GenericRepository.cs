using System.Security.Principal;
using System.Net.Http.Headers;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Deleite.Dal.DBContext;
using Deleite.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Deleite.Entity.Models;
using System.IO;
using Deleite.Entity.DtoModels;

namespace Deleite.Dal.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly DeleitebdContext _dbcontext;
        public GenericRepository(DeleitebdContext dbcontext)
        {
            _dbcontext = dbcontext;
            
        }
        public async Task<TEntity>Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entitidad = await _dbcontext.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entitidad;
            }
            catch 
            {
                 throw;
            }
        }
        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                _dbcontext.Set<TEntity>().Add(entidad);
                await _dbcontext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                 throw;
            }
        }

        public async Task<DtoProduco> CrearProducto(DtoProduco producto)
        {
            try
            {
                if(producto.ImagenPrincipalchar !=null){
                    // Extraer la imagen de la persona.
                    //var imagenBytes = Convert.FromBase64String(producto.ImagenPrincipal);
                    // Generar un nombre único para la imagen.
                    var nombreImagen = Guid.NewGuid().ToString() + ".png";
                    // Obtener la ruta donde se guardarán las imágenes.
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "fotos");
                    Console.WriteLine(ruta);
                    // Si la carpeta no existe, crearla.
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }
                    // Guardar la imagen en la ruta.
                    var rutaImagen = Path.Combine(ruta, nombreImagen);
                    using (var stream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        await stream.WriteAsync(producto.ImagenPrincipalchar, 0, producto.ImagenPrincipalchar.Length);
                    }

                    // Guardar el nombre de archivo en la base de datos.
                    var nuevoProducto = new Producto
                    {

                        IdCategoria = producto.IdCategoria,
                        IdTematica = producto.IdTematica,
                        NombreP = producto.NombreP,
                        DescripcionP = producto.DescripcionP,
                        Precio = producto.Precio,
                        ImagenPrincipal =nombreImagen,
                        Popular = producto.Popular,
                        Ingredienteselect = producto.Ingredienteselect,
                        Saludable = producto.Saludable,

                    };



                    _dbcontext.Set<Producto>().Add(nuevoProducto);
                    await _dbcontext.SaveChangesAsync();
                    producto.DescripcionP = ruta;
                    return producto;
                }else{
                    return producto;
                }
            }
            catch
            {
                 throw;
            }
        }
         public async Task<DtoImagenProducto> AddImageProducto(DtoImagenProducto producto)
        {
            try
            {
                if(producto.ImagenPrincipalchar !=null){
                    // Extraer la imagen de la persona.
                    //var imagenBytes = Convert.FromBase64String(producto.ImagenPrincipal);
                    // Generar un nombre único para la imagen.
                    var nombreImagen = Guid.NewGuid().ToString() + ".png";
                    // Obtener la ruta donde se guardarán las imágenes.
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "fotos");
                    Console.WriteLine(ruta);
                    // Si la carpeta no existe, crearla.
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }
                    // Guardar la imagen en la ruta.
                    var rutaImagen = Path.Combine(ruta, nombreImagen);
                    using (var stream = new FileStream(rutaImagen, FileMode.Create))
                    {
                        await stream.WriteAsync(producto.ImagenPrincipalchar, 0, producto.ImagenPrincipalchar.Length);
                    }
                    // Guardar el nombre de archivo en la base de datos.
                    var nuevaImagen = new ImagenProducto
                    {
                       NombreFoto = nombreImagen,
                       IdProducto = producto.IdProducto
                    };
                    _dbcontext.Set<ImagenProducto>().Add(nuevaImagen);
                    await _dbcontext.SaveChangesAsync();
                    return producto;
                }else{
                    return producto;
                }
            }
            catch
            {
                 throw;
            }
        }

        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                _dbcontext.Update(entidad);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                 throw;
            }
        }

        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                _dbcontext.Remove(entidad);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch
            {
                 throw;
            }
        }

        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null)
        {
            try
            {
                 IQueryable<TEntity> queryEntidad = filtro == null ? _dbcontext.Set<TEntity>() : _dbcontext.Set<TEntity>().Where(filtro);
                return queryEntidad;
            }
            catch (System.Exception)
            {
                
                throw;
            }
           
        }
        public async Task<IQueryable<ImagenProducto>> Consultarimgs(Expression<Func<ImagenProducto, bool>> filtro=null){
            IQueryable<ImagenProducto> queryEntidad = filtro == null ? _dbcontext.Set<ImagenProducto>() : _dbcontext.Set<ImagenProducto>().Where(filtro);
            return queryEntidad;
        }

        public async Task<bool>borrarimagen(ImagenProducto entidad)
        {
            try
            {
                var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "fotos", entidad.NombreFoto);
                File.Delete(rutaImagen);
                _dbcontext.Remove(entidad);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                 throw new Exception(e.Message);
            }
        }

        public async Task<bool>borrarimagenProducto(Producto entidad)
        {
            try
            {
                if (entidad.ImagenPrincipal!=null)
                {
                    var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "fotos", entidad.ImagenPrincipal);
                    File.Delete(rutaImagen);
                }
                _dbcontext.Remove(entidad);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                 throw new Exception(e.Message);
            }
        }

    }
}