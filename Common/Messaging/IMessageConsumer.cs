using Common.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Messaging
{
    public interface IMessageConsumer
    {                
        List<Type> GetMessageTypesHandled();
        void ProcessMessage(InternalMessage message);
        void RegisterAggregator(EventAggregator aggregator);
    }
}
