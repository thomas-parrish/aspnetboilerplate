using System.Data.Entity;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.EntityFramework.Repositories
{
    public class EfReadOnlyRepositoryBase<TDbContext, TEntity> : EfReadOnlyRepositoryBase<TDbContext, TEntity, int>, IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        public EfReadOnlyRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}