using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Mvc.Bootstrap.Html
{
    public static class BootstrapperCss
    {
        public const string CssClass = "class";
        public const string ControlLabel = "control-label";
        public const string ControlGroup = "control-group";
        public const string Error = "error";

        //combined css
        public const string ControlGroupError = ControlGroup + " " + Error;
    }
}
