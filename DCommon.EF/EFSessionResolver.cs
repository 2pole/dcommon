using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using DCommon;

namespace DCommon.EF
{
    /// <summary>
    /// Implementation of <see cref="IEFSessionResolver"/> that resolves <see cref="IEFSession"/> instances.
    /// </summary>
    public class EFSessionResolver : IEFSessionResolver
    {
        private readonly IDictionary<string, Guid> _dbContextTypeCache = new Dictionary<string, Guid>();
        private readonly IDictionary<Guid, Func<DbContext>> _dbContexts = new Dictionary<Guid, Func<DbContext>>();

        protected IDictionary<Guid, Func<DbContext>> DbContexts
        {
            get { return _dbContexts; }
        }

        protected  IDictionary<string, Guid> DbContextTypeCache
        {
            get { return _dbContextTypeCache; }
        }

        /// <summary>
        /// Gets the number of <see cref="DbContext"/> instances registered with the session resolver.
        /// </summary>
        public int DbContextsRegistered
        {
            get { return _dbContexts.Count; }
        }

        /// <summary>
        /// Gets the unique DbContext key for a type. 
        /// </summary>
        /// <typeparam name="T">The type for which the DbContext key should be retrieved.</typeparam>
        /// <returns>A <see cref="Guid"/> representing the unique object context key.</returns>
        public Guid GetSessionKeyFor<T>()
        {
            var typeName = typeof(T).Name;
            Guid key;
            if (!_dbContextTypeCache.TryGetValue(typeName, out key))
                throw new ArgumentException("No DbContext has been registered for the specified type.");
            return key;
        }

        /// <summary>
        /// Opens a <see cref="IEFSessionLocator"/> instance for a given type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="IEFSessionLocator"/> is returned.</typeparam>
        /// <returns>An instance of <see cref="IEFSessionLocator"/>.</returns>
        public IEFSessionLocator OpenSessionFor<T>()
        {
            var context = GetDbContextFor<T>();
            return new EFSessionLocator(context);
        }

        /// <summary>
        /// Gets the <see cref="DbContext"/> that can be used to query and update a given type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="DbContext"/> is returned.</typeparam>
        /// <returns>An <see cref="DbContext"/> that can be used to query and update the given type.</returns>
        public DbContext GetDbContextFor<T>()
        {
            var typeName = typeof(T).Name;
            Guid key;
            if (!_dbContextTypeCache.TryGetValue(typeName, out key))
                throw new ArgumentException("No DbContext has been registered for the specified type.");
            return _dbContexts[key]();
        }

        /// <summary>
        /// Registers an <see cref="DbContext"/> provider with the resolver.
        /// </summary>
        /// <param name="contextProvider">A <see cref="Func{T}"/> of type <see cref="DbContext"/>.</param>
        public virtual void RegisterDbContextProvider(Func<DbContext> contextProvider)
        {
            var key = Guid.NewGuid();
            _dbContexts.Add(key, contextProvider);
            //Getting the object context and populating the _DbContextTypeCache.
            using (var context = contextProvider())
            {
                var entities = GetEntityTypes(context);
                entities.ForEach(entity => _dbContextTypeCache.Add(entity.Name, key));
            }
        }

        protected virtual IEnumerable<EntityType> GetEntityTypes(DbContext context)
        {
            var entities = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace);
            return entities;
        }
    }
}
