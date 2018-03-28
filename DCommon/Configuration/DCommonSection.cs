using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DCommon.Configuration
{
    public class DCommonSection : ConfigurationSection
    {
        public const string SectinoName = "dcommon";
        public const string DataProviderElementName = "dataProvider";
        public const string ShellSettingElementName = "shellSetting";

        [ConfigurationProperty(DataProviderElementName, IsRequired = false)]
        public DataProviderElement DataProvider
        {
            get { return (DataProviderElement)base[DataProviderElementName]; }
            set { base[DataProviderElementName] = value; }
        }

        [ConfigurationProperty(ShellSettingElementName, IsRequired = false)]
        public ShellSettingElement ShellSetting
        {
            get { return (ShellSettingElement)base[ShellSettingElementName]; }
            set { base[ShellSettingElementName] = value; }
        }

        public static DCommonSection GetDCommonSection()
        {
            return (DCommonSection)ConfigurationManager.GetSection(SectinoName);
        }
    }
}
