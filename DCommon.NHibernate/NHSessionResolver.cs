using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace DCommon.NHibernate
{
    public class NHSessionResolver : INHSessionResolver
    {
        private readonly IDictionary<string, Guid> _sessionFactoryTypeCache = new Dictionary<string, Guid>();
        private readonly IDictionary<Guid, Func<ISessionFactory>> _sessionFactories = new Dictionary<Guid, Func<ISessionFactory>>();

        /// <summary>
        /// Gets the count of <see cref="ISessionFactory"/> providers registered with the resolver.
        /// </summary>
        public int SessionFactoriesRegistered
        {
            get { return _sessionFactories.Count; }
        }

        /// <summary>
        /// Gets the unique <see cref="ISession"/> key for a type. 
        /// </summary>
        /// <typeparam name="T">The type for which the ObjectContext key should be retrieved.</typeparam>
        /// <returns>A <see cref="Guid"/> representing the unique object context key.</returns>
        public Guid GetSessionKeyFor<T>()
        {
            var typeName = typeof(T).Name;
            Guid factorykey;
            if (!_sessionFactoryTypeCache.TryGetValue(typeName, out factorykey))
                throw new ArgumentException("No ISessionFactory has been registered for the specified type.");
            return factorykey;
        }

        /// <summary>
        /// Gets the <see cref="ISessionFactory"/> that can be used to create instances of <see cref="ISession"/>
        /// to query and update the specified type..
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="ISessionFactory"/> is returned.</typeparam>
        /// <returns>An <see cref="ISessionFactory"/> that can be used to create instances of <see cref="ISession"/>
        /// to query and update the specified type.</returns>
        public ISessionFactory GetFactoryFor<T>()
        {
            var typeName = typeof(T).Name;
            Guid factorykey;
            if (!_sessionFactoryTypeCache.TryGetValue(typeName, out factorykey))
                throw new ArgumentException("No ISessionFactory has been registered for the specified type.");
            return _sessionFactories[factorykey]();
        }

        /// <summary>
        /// Opens a <see cref="ISession"/> instance for a given type.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="ISession"/> is returned.</typeparam>
        /// <returns>An instance of <see cref="ISession"/>.</returns>
        public ISession OpenSessionFor<T>()
        {
            var key = GetSessionKeyFor<T>();
            var session = _sessionFactories[key]().OpenSession();
            return session;
        }

        /// <summary>
        /// Registers an <see cref="ISessionFactory"/> provider with the resolver.
        /// </summary>
        /// <param name="factoryProvider">A <see cref="Func{T}"/> of type <see cref="ISessionFactory"/>.</param>
        public void RegisterSessionFactoryProvider(Func<ISessionFactory> factoryProvider)
        {
            var key = Guid.NewGuid();
            _sessionFactories.Add(key, factoryProvider);
            //Getting the factory and initializing populating _sessionFactoryTypeCache.
            //todo:
            var factory = factoryProvider();
            var classMappings = factory.GetAllClassMetadata();
            if (classMappings != null && classMappings.Count > 0)
                classMappings.ForEach(map => _sessionFactoryTypeCache
                                                 .Add(map.Value.EntityName, key));
        }
    }
}
