using System;
using System.Reflection;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Reflection.Extensions;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;

namespace Abp.EntityFramework.Repositories
{
    public class EfGenericRepositoryRegistrar : IEfGenericRepositoryRegistrar, ITransientDependency
    {
        public ILogger Logger { get; set; }

        private readonly IDbContextEntityFinder _dbContextEntityFinder;

        public EfGenericRepositoryRegistrar(IDbContextEntityFinder dbContextEntityFinder)
        {
            _dbContextEntityFinder = dbContextEntityFinder;
            Logger = NullLogger.Instance;
        }

        public void RegisterForDbContext(
            Type dbContextType, 
            IIocManager iocManager, 
            AutoRepositoryTypesAttribute defaultAutoRepositoryTypesAttribute)
        {
            var autoRepositoryAttr = dbContextType.GetTypeInfo().GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>() ?? defaultAutoRepositoryTypesAttribute;

            RegisterForDbContext(
                dbContextType,
                iocManager,
                autoRepositoryAttr.RepositoryInterface,
                autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey,
                autoRepositoryAttr.RepositoryImplementation,
                autoRepositoryAttr.RepositoryImplementationWithPrimaryKey,
                autoRepositoryAttr.ReadOnlyRepositoryInterface,
                autoRepositoryAttr.ReadOnlyRepositoryInterfaceWithPrimaryKey,
                autoRepositoryAttr.ReadOnlyRepositoryImplementation,
                autoRepositoryAttr.ReadOnlyRepositoryImplementationWithPrimaryKey
            );

            if (autoRepositoryAttr.WithDefaultRepositoryInterfaces)
            {
                RegisterForDbContext(
                    dbContextType,
                    iocManager,
                    defaultAutoRepositoryTypesAttribute.RepositoryInterface,
                    defaultAutoRepositoryTypesAttribute.RepositoryInterfaceWithPrimaryKey,
                    autoRepositoryAttr.RepositoryImplementation,
                    autoRepositoryAttr.RepositoryImplementationWithPrimaryKey,
                    autoRepositoryAttr.ReadOnlyRepositoryInterface,
                    autoRepositoryAttr.ReadOnlyRepositoryInterfaceWithPrimaryKey,
                    autoRepositoryAttr.ReadOnlyRepositoryImplementation,
                    autoRepositoryAttr.ReadOnlyRepositoryImplementationWithPrimaryKey
                );
            }
        }

        private void RegisterForDbContext(
            Type dbContextType, 
            IIocManager iocManager,
            Type repositoryInterface,
            Type repositoryInterfaceWithPrimaryKey,
            Type repositoryImplementation,
            Type repositoryImplementationWithPrimaryKey,
            Type readOnlyRepositoryInterface,
            Type readOnlyRepositoryInterfaceWithPrimaryKey,
            Type readOnlyRepositoryImplementation,
            Type readOnlyRepositoryImplementationWithPrimaryKey)
        {
            foreach (var entityTypeInfo in _dbContextEntityFinder.GetEntityTypeInfos(dbContextType))
            {
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    var genericRepositoryType = repositoryInterface.MakeGenericType(entityTypeInfo.EntityType);
                    if (!iocManager.IsRegistered(genericRepositoryType))
                    {
                        Register(genericRepositoryType, GetImplementedTypeWithIntKey(repositoryImplementation, entityTypeInfo));
                    }
                }

                var genericRepositoryTypeWithPrimaryKey = repositoryInterfaceWithPrimaryKey.MakeGenericType(entityTypeInfo.EntityType,primaryKeyType);
                if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                {
                    var implType = GetImplementedTypeWithPrimaryKey(repositoryImplementationWithPrimaryKey, entityTypeInfo, primaryKeyType);
                    
                    Register(genericRepositoryTypeWithPrimaryKey, implType);
                }
            }

            foreach (var readOnlyTypeInfo in _dbContextEntityFinder.GetReadOnlyEntityTypeInfos(dbContextType))
            {
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(readOnlyTypeInfo.EntityType);
                if (primaryKeyType == typeof(int))
                {
                    var genericRepositoryType = readOnlyRepositoryInterface.MakeGenericType(readOnlyTypeInfo.EntityType);
                    if (!iocManager.IsRegistered(genericRepositoryType))
                    {
                        Register(genericRepositoryType, GetImplementedTypeWithIntKey(readOnlyRepositoryImplementation, readOnlyTypeInfo));
                    }
                }

                var genericReadOnlyRepositoryTypeWithPrimaryKey = readOnlyRepositoryInterfaceWithPrimaryKey.MakeGenericType(readOnlyTypeInfo.EntityType,primaryKeyType);
                if (!iocManager.IsRegistered(genericReadOnlyRepositoryTypeWithPrimaryKey))
                {
                    var implType = GetImplementedTypeWithPrimaryKey(readOnlyRepositoryImplementationWithPrimaryKey, readOnlyTypeInfo, primaryKeyType);

                    Register(genericReadOnlyRepositoryTypeWithPrimaryKey, implType);
                }
            }

            void Register(Type genericRepositoryType, Type implType)
            {
                iocManager.IocContainer.Register(
                    Component
                        .For(genericRepositoryType)
                        .ImplementedBy(implType)
                        .Named(Guid.NewGuid().ToString("N"))
                        .LifestyleTransient()
                );
            }

            Type GetImplementedTypeWithIntKey(Type type, EntityTypeInfo readOnlyTypeInfo) => type.GetGenericArguments().Length == 1
                ? type.MakeGenericType(readOnlyTypeInfo.EntityType)
                : type.MakeGenericType(readOnlyTypeInfo.DeclaringType,
                    readOnlyTypeInfo.EntityType);

            Type GetImplementedTypeWithPrimaryKey(Type type, EntityTypeInfo readOnlyTypeInfo,
                Type primaryKeyType)
            {
                var implType = type.GetGenericArguments().Length == 2
                    ? type.MakeGenericType(readOnlyTypeInfo.EntityType, primaryKeyType)
                    : type.MakeGenericType(readOnlyTypeInfo.DeclaringType,
                        readOnlyTypeInfo.EntityType, primaryKeyType);
                return implType;
            }
        }
    }
}