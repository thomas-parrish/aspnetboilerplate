﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.EntityFrameworkCore.Tests.Domain;
using Microsoft.EntityFrameworkCore;

namespace Abp.EntityFrameworkCore.Tests.Ef
{
    [AutoRepositoryTypes(
        typeof(ISupportRepository<>),
        typeof(ISupportRepository<,>),
        typeof(SupportRepositoryBase<>),
        typeof(SupportRepositoryBase<,>),
        typeof(IReadOnlySupportRepository<>),
        typeof(IReadOnlySupportRepository<,>),
        typeof(SupportRepositoryBase<>),
        typeof(SupportRepositoryBase<,>),
        WithDefaultRepositoryInterfaces = true
        )]
    public class SupportDbContext : AbpDbContext
    {
        public DbSet<Ticket> Tickets { get; set; }

        public DbQuery<TicketListItem> TicketListItems { get; set; }

        public const string TicketViewSql = @"CREATE VIEW TicketListItemView AS SELECT Id, EmailAddress, TenantId, IsActive FROM Tickets";

        public SupportDbContext(DbContextOptions<SupportDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Query<TicketListItem>().ToView("TicketListItemView");
        }
    }

    public interface IReadOnlySupportRepository<TEntity, TPrimaryKey> : IReadOnlyRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        //A new custom method
        List<TEntity> GetActiveList();
    }

    public interface IReadOnlySupportRepository<TEntity> : IReadOnlySupportRepository<TEntity, int>, IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity<int>
    {

    }

    public interface ISupportRepository<TEntity, TPrimaryKey> : IReadOnlySupportRepository<TEntity, TPrimaryKey>, IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
    }

    public interface ISupportRepository<TEntity> : ISupportRepository<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {

    }

    public class SupportRepositoryBase<TEntity, TPrimaryKey> : EfCoreRepositoryBase<SupportDbContext, TEntity, TPrimaryKey>, ISupportRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public SupportRepositoryBase(IDbContextProvider<SupportDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //A new custom method
        public List<TEntity> GetActiveList()
        {
            if (typeof(IPassivable).GetTypeInfo().IsAssignableFrom(typeof(TEntity)))
            {
                return GetAll()
                    .Cast<IPassivable>()
                    .Where(e => e.IsActive)
                    .Cast<TEntity>()
                    .ToList();
            }

            return GetAllList();
        }

        //An override of a default method
        public override int Count()
        {
            throw new Exception("can not get count!");
        }
    }

    public class SupportRepositoryBase<TEntity> : SupportRepositoryBase<TEntity, int>, ISupportRepository<TEntity>, IReadOnlySupportRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public SupportRepositoryBase(IDbContextProvider<SupportDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
