using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Caching;

namespace DCommon.FileSystems
{
    /// <summary>
    /// Enable monitoring changes over virtual path
    /// </summary>
    public interface IVirtualPathMonitor : IVolatileProvider
    {
        IVolatileToken WhenPathChanges(string virtualPath);
    }
}
