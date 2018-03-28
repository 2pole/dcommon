using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DCommon.EF
{
    public interface IEFSessionResolver
    {
        /// <summary>
        /// Gets the unique <see cref="IEFSessionLocator"/> key for a type. 
        /// </summary>
        /// <typeparam name="T">The type for which the ObjectContext key should be retrieved.</typeparam>
        /// <returns>A <see cref="Guid"/> representing the unique object context key.</returns>
        Guid GetSessionKeyFor<T>();

        /// <summary>
        /// Gets the <see cref="DbContext"/> that can be used to query and update a given type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="DbContext"/> is returned.</typeparam>
        /// <returns>An <see cref="DbContext"/> that can be used to query and update the given type.</returns>
        DbContext GetDbContextFor<T>();

        /// <summary>
        /// Registers an <see cref="DbContext"/> provider with the resolver.
        /// </summary>
        /// <param name="contextProvider">A <see cref="Func{T}"/> of type <see cref="DbContext"/>.</param>
        void RegisterDbContextProvider(Func<DbContext> contextProvider);

        /// <summary>
        /// Gets the count of <see cref="DbContext"/> providers registered with the resolver.
        /// </summary>
        int DbContextsRegistered { get; }
    }
}
