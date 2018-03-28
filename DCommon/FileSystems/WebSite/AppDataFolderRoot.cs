using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace DCommon.FileSystems.WebSite
{
    public class AppDataFolderRoot : IAppDataFolderRoot
    {
        public string RootPath
        {
            get { return "~/App_Data"; }
        }

        public string RootFolder
        {
            get { return HostingEnvironment.MapPath(RootPath); }
        }
    }
}
