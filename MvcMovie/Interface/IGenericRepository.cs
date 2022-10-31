using System.Linq.Expressions;

namespace MvcMovie.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate = null);       
        Task<TEntity> GetById(int? entityId);
        Task Insert(TEntity entity);
        void Update(TEntity entity);
        Task DeleteById(int? entityId);
    }
}
