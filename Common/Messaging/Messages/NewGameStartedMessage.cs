using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging.Messages
{
    public class NewGameStartedMessage : InternalMessage
    {
        public string ViewUrl { get; set; }
    }
}
