using System;

namespace Abp.Domain.Repositories
{
    /// <summary>
    /// Used to define auto-repository types for entities.
    /// This can be used for DbContext types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoRepositoryTypesAttribute : Attribute
    {
        public Type RepositoryInterface { get; }

        public Type RepositoryInterfaceWithPrimaryKey { get; }

        public Type RepositoryImplementation { get; }

        public Type RepositoryImplementationWithPrimaryKey { get; }

        public Type ReadOnlyRepositoryInterface { get; }

        public Type ReadOnlyRepositoryInterfaceWithPrimaryKey { get; }

        public Type ReadOnlyRepositoryImplementation { get; }

        public Type ReadOnlyRepositoryImplementationWithPrimaryKey { get; }

        public bool WithDefaultRepositoryInterfaces { get; set; }

        public AutoRepositoryTypesAttribute(
            Type repositoryInterface,
            Type repositoryInterfaceWithPrimaryKey,
            Type repositoryImplementation,
            Type repositoryImplementationWithPrimaryKey,
            Type readOnlyRepositoryInterface,
            Type readOnlyRepositoryInterfaceWithPrimaryKey,
            Type readOnlyRepositoryImplementation,
            Type readOnlyRepositoryImplementationWithPrimaryKey)
        {
            RepositoryInterface = repositoryInterface;
            RepositoryInterfaceWithPrimaryKey = repositoryInterfaceWithPrimaryKey;
            RepositoryImplementation = repositoryImplementation;
            RepositoryImplementationWithPrimaryKey = repositoryImplementationWithPrimaryKey;
            ReadOnlyRepositoryInterface = readOnlyRepositoryInterface;
            ReadOnlyRepositoryInterfaceWithPrimaryKey = readOnlyRepositoryInterfaceWithPrimaryKey;
            ReadOnlyRepositoryImplementation = readOnlyRepositoryImplementation;
            ReadOnlyRepositoryImplementationWithPrimaryKey = readOnlyRepositoryImplementationWithPrimaryKey;
        }
    }
}