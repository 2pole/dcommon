using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DCommon.FileSystems.Desktop
{
    public class AppDataFolderRoot : IAppDataFolderRoot
    {
        public static string DefaultRootPath { get; set; }

        #region Implementation of IAppDataFolderRoot

        /// <summary>
        /// DCommon
        /// </summary>
        public string RootPath
        {
            get { return DefaultRootPath ?? "DCommon"; }
        }

        /// <summary>
        /// Physical path of root (typically: MapPath(RootPath))
        /// </summary>
        public string RootFolder
        {
            get
            {
                return Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData),
                    RootPath);
            }
        }

        #endregion
    }
}
