using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configuration
{
    public class GameSettingsConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("PrivateKey", IsRequired = true)]
        public string PrivateKey
        {
            get { return (string)this["PrivateKey"]; }
        }

        [ConfigurationProperty("ServerURL", IsRequired = false, DefaultValue = "http://vindinium.org")]
        public string ServerURL
        {
            get { return (string)this["ServerURL"]; }
        }

        [ConfigurationProperty("NumberOfTurns", IsRequired = false, DefaultValue = "10")]
        public int NumberOfTurns
        {
            get { return (int)this["NumberOfTurns"]; }
        }

        [ConfigurationProperty("TrainingMode", IsRequired = false, DefaultValue = "true")]
        public bool TrainingMode
        {
            get { return (bool)this["TrainingMode"]; }
        }
    }
}
