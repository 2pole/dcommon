using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data
{
     public interface ISessionResolver
     { 
         /// <summary>
         /// Gets the unique key for a type. 
         /// </summary>
         /// <typeparam name="T">The type for which the ObjectContext key should be retrieved.</typeparam>
         /// <returns>A <see cref="Guid"/> representing the unique object context key.</returns>
         Guid GetSessionKeyFor<T>();
    }
}
