using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCommon.Utility;

namespace DCommon.EF
{
    internal static class DbContextExtensions
    {
        public static object[] KeyValuesFor(this DbContext context, object entity)
        {
            Check.IsNotNull(context, "context");
            Check.IsNotNull(entity, "entity");
            var entry = context.Entry(entity);
            return context.KeysFor(entity.GetType())
                .Select(k => entry.Property(k).CurrentValue)
                .ToArray();
        }

        public static IEnumerable<string> KeysFor(this DbContext context, Type entityType)
        {
            Check.IsNotNull(context, "context");
            Check.IsNotNull(entityType, "entityType");

            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            entityType = ObjectContext.GetObjectType(entityType);
            var metadataWorkspace = objectContext.MetadataWorkspace;
            var objectItemCollection = (ObjectItemCollection)metadataWorkspace.GetItemCollection(DataSpace.OSpace);
            var ospaceType = metadataWorkspace.GetItems<EntityType>(DataSpace.OSpace).SingleOrDefault(t => objectItemCollection.GetClrType(t) == entityType);

            if (ospaceType == null)
            {
                throw new ArgumentException(
                    string.Format("The type '{0}' is not mapped as an entity type.", entityType.Name),
                    "entityType");
            }

            return ospaceType.KeyMembers.Select(k => k.Name);
        }
    }
}
