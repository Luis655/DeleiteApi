using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Deleite.Entity.Models;
using Deleite.Entity.DtoModels;

namespace Deleite.Dal.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity:class
    {

        Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro);
        Task<DtoProduc> obtenerPoducts(int id);
        Task<IQueryable<Producto>> DeleteFalses();
        Task<IQueryable<Producto>> getAll();
        Task<IQueryable<Categoria>> getAllProductos();

        public Task<IQueryable<TEntity>> ObtenerTodos()
        {
            return Consultar();
        }
        Task<TEntity> Crear(TEntity entidad);
        Task<DtoImagenProducto> AddImageProducto(DtoImagenProducto entidad);
        Task<DtoProduco> CrearProducto(DtoProduco entidad); 
        Task<DtoCategorias> CrearCategoria(DtoCategorias entidad); 
        Task<bool> Editar(TEntity entidad);
        Task<bool> Eliminar(TEntity entidad);
        Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro=null);
        Task<IQueryable<ImagenProducto>> Consultarimgs(Expression<Func<ImagenProducto, bool>> filtro=null);
        Task<bool> borrarimagen(int id);
        Task<bool> borrarimagenProducto(Producto entidad);
        Task<bool> EliminarFotoCategiria(Categoria entidad);
        Task<bool> EliminarFotoProducto(ImagenProducto entidad);


         
    }
}