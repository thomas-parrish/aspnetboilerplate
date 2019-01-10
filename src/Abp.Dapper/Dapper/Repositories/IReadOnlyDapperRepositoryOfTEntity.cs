using Abp.Domain.Entities;

namespace Abp.Dapper.Repositories
{
    public interface IReadOnlyDapperRepository<TEntity> : IReadOnlyDapperRepository<TEntity, int> where TEntity : class, IEntity<int>
    {
    }
}
