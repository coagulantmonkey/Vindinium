using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging.Messages
{
    public class GameSettingsConfigUpdated : InternalMessage
    {
        public string ServerURL { get; set; }
        public int NumberOfTurns { get; set; }
        public string PrivateKey { get; set; }
    }
}
