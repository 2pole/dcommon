using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Mapping.ByCode;

namespace DCommon.NHibernate
{
    public static class MappingTable
    {
        private static readonly MappingCollection _instance = new MappingCollection();

        public static MappingCollection Mappings
        {
            get
            {
                return MappingTable._instance;
            }
        }

        static MappingTable()
        {
        }
    }

    public class MappingCollection : IEnumerable<Type>
    {
        private readonly HashSet<Type> _mappings = new HashSet<Type>();

        public void AddMappings(Assembly assembly)
        {
            if (assembly != null)
            {
                foreach (var type in assembly.GetTypes().Where(Filter))
                {
                    _mappings.Add(type);
                }
            }
        }

        private static bool Filter(Type type)
        {
            return type.IsClass &&
                   !type.IsAbstract &&
                   typeof(IConformistHoldersProvider).IsAssignableFrom(type);
        }

        public void AddMapping(Type type)
        {
            if (type != null && Filter(type))
            {
                _mappings.Add(type);
            }
        }

        public void AddMapping<TMapping>()
            where TMapping : IConformistHoldersProvider
        {
            var type = typeof(TMapping);
            if (Filter(type))
            {
                _mappings.Add(type);
            }
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Type> GetEnumerator()
        {
            return _mappings.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
