using System;
using System.Data.Entity;
using DCommon.Data;
using DCommon.Utility;

namespace DCommon.EF
{
    public class EFUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IEFSessionResolver _sessionResolver = new EFSessionResolver();

        /// <summary>
        /// Creates a new instance of <see cref="IUnitOfWork"/>.
        /// </summary>
        /// <returns>Instances of <see cref="EFUnitOfWork"/>.</returns>
        public IUnitOfWork Create()
        {
            Guard.Against<InvalidOperationException>(
                _sessionResolver.DbContextsRegistered == 0,
                "No session factory providers have been registered. You must register ISessionFactory providers using " +
                "the RegisterSessionFactoryProvider method or use NCommon.Configure class to configure NCommon.NHibernate " +
                "using the NHConfiguration class and register ISessionFactory instances using the WithSessionFactory method.");

            return new EFUnitOfWork(_sessionResolver);
        }

        ///<summary>
        /// Registers a <see cref="Func{T}"/> of type <see cref="ObjectContext"/> provider that can be used
        /// to resolve instances of <see cref="ObjectContext"/>.
        /// </summary>
        /// <param name="contextProvider">A <see cref="Func{T}"/> of type <see cref="ObjectContext"/>.</param>
        public void RegisterDataContextCreator(Func<DbContext> contextProvider)
        {
            Guard.Against<ArgumentNullException>(contextProvider == null,
                                                   "Invalid object context provider registration. " +
                                                   "Expected a non-null Func<ObjectContext> instance.");
            _sessionResolver.RegisterDbContextProvider(contextProvider);
        }
    }
}
