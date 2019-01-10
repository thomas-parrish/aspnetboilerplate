using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace Abp.Domain.Repositories
{
    /// <summary>
    /// A shortcut of <see cref="IReadOnlyRepository{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public interface IReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity, int> where TEntity : class, IEntity<int>
    {

    }
}
