using System.Collections.Generic;

namespace DCommon.Environment.Configuration
{
    public interface ISettingsManager
    {
        ShellSetting LoadSetting();
        void SaveSettings(ShellSetting settings);
    }
}