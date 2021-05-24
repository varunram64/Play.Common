using Play.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Play.Common.IRepository
{
    public interface IMongoRepository<T> where T : IEntity
    {
        Task<IReadOnlyCollection<T>> GetAll();

        Task<IReadOnlyCollection<T>> GetAll(Expression<Func<T,bool>> filter);

        Task<T> Get(Guid id);

        Task<T> Get(Expression<Func<T, bool>> filter);

        Task Create(T entity);

        Task Update(T entity);

        Task Delete(Guid id);
    }
}
