using System.Linq.Expressions;
using Deleite.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Deleite.Entity.Models;
using Deleite.Entity.DtoModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Deleite.Dal.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly DeleitebdContext _dbcontext;
        public GenericRepository(DeleitebdContext dbcontext)
        {
            _dbcontext = dbcontext;

        }
        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
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
                if (producto.IdProducto == null)
                {
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

                        var rutas = Path.Combine(Directory.GetCurrentDirectory(), "fotos", "imagenPredetermindad.png");
                        var bytes = System.IO.File.ReadAllBytes(rutas);
                        var base64String = Convert.ToBase64String(bytes);
                        var imagenBytes = Convert.FromBase64String(base64String);


                        await stream.WriteAsync(imagenBytes, 0, imagenBytes.Length);
                    }
                    using (var transaction = _dbcontext.Database.BeginTransaction())
                    {
                        try
                        {
                            // Guardar el nombre de archivo en la base de datos.
                            var nuevoProducto = new Producto
                            {

                                IdProducto = producto.IdProducto,
                                IdConfirmacionT = false,
                                IdCategoria = producto.IdCategoria == null ? 1 : producto.IdCategoria,
                                IdTematica = producto.IdTematica == null ? 1 : producto.IdTematica,
                                NombreP = producto.NombreP == null ? "N/A" : producto.NombreP,
                                DescripcionP = producto.DescripcionP == null ? "N/A" : producto.DescripcionP,
                                Precio = producto.Precio == null ? "N/A" : producto.Precio,
                                ImagenPrincipal = nombreImagen == null ? Path.Combine(Directory.GetCurrentDirectory(), "fotos", "imagenPredetermindad.png") : nombreImagen,
                                Popular = producto.Popular == null ? false : producto.Popular,
                                Ingredienteselect = producto.Ingredienteselect == null ? "N/A" : producto.Ingredienteselect,
                                Saludable = producto.Saludable == null ? false : producto.Saludable,

                            };



                            _dbcontext.Set<Producto>().Add(nuevoProducto);
                            await _dbcontext.SaveChangesAsync();
                            transaction.Commit();
                            int? prod = nuevoProducto.IdProducto;
                            producto = new DtoProduco
                            {
                                IdProducto = prod
                            };
                            return producto;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }

                else
                {
                    var data = await _dbcontext.Productos.FirstOrDefaultAsync(x => x.IdProducto == producto.IdProducto);
                    if (data != null)
                    {

                        using (var transaction = _dbcontext.Database.BeginTransaction())
                        {
                            try
                            {

                                var nombreImagen="";
                                if(producto.ImagenPrincipalchar != null){
                                nombreImagen = Guid.NewGuid().ToString() + ".png";
                                // Obtener la ruta donde se guardarán las imágenes.
                                var ruta = Path.Combine(Directory.GetCurrentDirectory(), "fotos");
                                if (data.ImagenPrincipal != null)
                                {
                                    var rutaImagenAnterior = Path.Combine(ruta, data.ImagenPrincipal);
                                    File.Delete(rutaImagenAnterior);
                                }
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
                                }

                                //setear los valores con los nuevos
                                data.IdCategoria = producto.IdCategoria == null ? 1 : producto.IdCategoria;
                                data.IdConfirmacionT = true;
                                data.IdTematica = producto.IdTematica == null ? 1 : producto.IdTematica;
                                data.NombreP = producto.NombreP == null ? "N/A" : producto.NombreP;
                                data.DescripcionP = producto.DescripcionP == null ? "N/A" : producto.DescripcionP;
                                data.Precio = producto.Precio == null ? "N/A" : producto.Precio;
                                data.ImagenPrincipal = nombreImagen == "" ? Path.Combine(Directory.GetCurrentDirectory(), "fotos", data.ImagenPrincipal) : nombreImagen;
                                data.Popular = producto.Popular == null ? null : producto.Popular;
                                data.Ingredienteselect = producto.Ingredienteselect == null ? "N/A" : producto.Ingredienteselect;
                                data.Saludable = producto.Saludable == null ? null : producto.Saludable;
                                //gusradr los cambios en la base de datos
                                _dbcontext.SaveChanges();
                                transaction.Commit();
                                producto.IdProducto = data.IdProducto;
                                producto.NombreP = data.NombreP;
                                return producto;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception("este error sucedio en GenericRepository en la linea 173" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        return producto;
                    }
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
                if (producto.ImagenPrincipalchar != null)
                {
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
                }
                else
                {
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
        public async Task<IQueryable<Producto>> getAll()
        {

            var producto = await _dbcontext.Productos
            .Include(x => x.IdCategoriaNavigation)
            .Include(x => x.IdTematicaNavigation)
            .AsQueryable<Producto>().AsNoTracking().ToListAsync();


            return producto.AsQueryable();
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
        public async Task<IQueryable<ImagenProducto>> Consultarimgs(Expression<Func<ImagenProducto, bool>> filtro = null)
        {
            IQueryable<ImagenProducto> queryEntidad = filtro == null ? _dbcontext.Set<ImagenProducto>() : _dbcontext.Set<ImagenProducto>().Where(filtro);
            return queryEntidad;
        }

        public async Task<bool> borrarimagen(int id)
        {
            try
            {
                var entidad = await _dbcontext.ImagenProductos.Where(x => x.IdProducto == id).ToListAsync();
                foreach (var item in entidad)
                {
                    var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "fotos", item.NombreFoto);
                    File.Delete(rutaImagen);
                }

                //_dbcontext.Remove(entidad);
                _dbcontext.ImagenProductos.RemoveRange(entidad);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> borrarimagenProducto(Producto entidad)
        {
            try
            {
                if (entidad.ImagenPrincipal != null)
                {
                    var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "fotos", entidad.ImagenPrincipal);
                    File.Delete(rutaImagen);
                }
                _dbcontext.Remove(entidad);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}