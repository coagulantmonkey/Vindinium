using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vindinium
{
    public static class ConfigManager
    {
        public static string GetConfigKey(string keyName)
        {
            return ConfigurationManager.AppSettings[keyName];
        }
    }
}
