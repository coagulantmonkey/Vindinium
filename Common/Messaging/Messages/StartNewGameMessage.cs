using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging.Messages
{
    public class StartNewGameMessage : InternalMessage
    {
        public string PrivateKey { get; set; }
        public bool TrainingMode { get; set; }
        public string ServerURL { get; set; }
        public int NumberOfTurns { get; set; }
    }
}
