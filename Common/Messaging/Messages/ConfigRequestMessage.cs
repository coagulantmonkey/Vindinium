using Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging.Messages
{
    public class ConfigRequestMessage : InternalMessage
    {
        public AvailableSections RequestedSection { get; set; }
        public Action<ConfigurationSection> Callback { get; set; }
    }
}
