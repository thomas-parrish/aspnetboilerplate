using System;

using Abp.Domain.Repositories;

using JetBrains.Annotations;

namespace Abp.Dapper
{
    public class DapperAutoRepositoryTypeAttribute : AutoRepositoryTypesAttribute
    {
        public DapperAutoRepositoryTypeAttribute(
            [NotNull] Type repositoryInterface,
            [NotNull] Type repositoryInterfaceWithPrimaryKey,
            [NotNull] Type repositoryImplementation,
            [NotNull] Type repositoryImplementationWithPrimaryKey,
            [NotNull] Type readOnlyRepositoryInterface,
            [NotNull] Type readOnlyRepositoryInterfaceWithPrimaryKey,
            [NotNull] Type readOnlyRepositoryImplementation,
            [NotNull] Type readOnlyRepositoryImplementationWithPrimaryKey)
            : base(repositoryInterface, 
                repositoryInterfaceWithPrimaryKey, 
                repositoryImplementation, 
                repositoryImplementationWithPrimaryKey, 
                readOnlyRepositoryInterface, 
                readOnlyRepositoryInterfaceWithPrimaryKey, 
                readOnlyRepositoryImplementation,
                readOnlyRepositoryImplementationWithPrimaryKey)
        {
        }
    }
}
