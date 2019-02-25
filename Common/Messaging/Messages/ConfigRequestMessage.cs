using Common.Configuration;
using System;
using System.Configuration;

namespace Common.Messaging.Messages
{
    public class ConfigRequestMessage : InternalMessage
    {
        public AvailableSections RequestedSection { get; set; }
        public Action<ConfigurationSection> Callback { get; set; }
    }
}
