using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Interface;
using System.Linq.Expressions;

namespace MvcMovie.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly MvcMovieContext _mvcMovieContext;
        public GenericRepository(MvcMovieContext mvcMovieContext)
        {
            _mvcMovieContext = mvcMovieContext;
        }

        public async Task DeleteById(int? entityId)
        {
            try
            {
                TEntity entity = await GetById(entityId);
                _mvcMovieContext.Remove(entity);
            }
            catch (NullReferenceException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
        {
            try
            {
                if (predicate is not null)
                {
                    return await _mvcMovieContext.Set<TEntity>().Where(predicate).ToListAsync();
                }

                return await _mvcMovieContext.Set<TEntity>().ToListAsync();
            }

            catch (Exception)
            {

                throw;
            }
        }        

        public async Task<TEntity> GetById(int? entityId)
        {
            try
            {
                return await _mvcMovieContext.Set<TEntity>().FindAsync(entityId);
            }
            catch (NullReferenceException)
            {

                throw;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task Insert(TEntity entity)
        {
            try
            {
                await _mvcMovieContext.AddAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Update(TEntity entity)
        {
            try
            {
                _mvcMovieContext.Update(entity);
            }
            catch (NullReferenceException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
