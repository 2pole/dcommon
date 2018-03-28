using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.FileSystems
{
    /// <summary>
    /// Abstraction over the root location of "~/App_Data", mainly to enable
    /// unit testing of AppDataFolder.
    /// </summary>
    public interface IAppDataFolderRoot
    {
        /// <summary>
        /// Virtual path of root ("~/App_Data")
        /// </summary>
        string RootPath { get; }
        /// <summary>
        /// Physical path of root (typically: MapPath(RootPath))
        /// </summary>
        string RootFolder { get; }
    }
}
