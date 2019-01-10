using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Abp.EntityFrameworkCore.Repositories
{
    public class EfCoreReadOnlyRepositoryBase<TDbContext, TEntity> : EfCoreReadOnlyRepositoryBase<TDbContext, TEntity, int>, IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
    {
        public EfCoreReadOnlyRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}