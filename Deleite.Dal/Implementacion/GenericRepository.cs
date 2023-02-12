using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Deleite.Dal.DBContext;
using Deleite.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;


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
            IQueryable<TEntity> queryEntidad = filtro == null ? _dbcontext.Set<TEntity>() : _dbcontext.Set<TEntity>().Where(filtro);
            return queryEntidad;
        }



    }
}