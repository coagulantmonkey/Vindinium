using Common.Messaging.Messages;
using System;
using System.Collections.Generic;

namespace Common.Messaging
{
    public interface IMessageConsumer
    {
        List<Type> GetMessageTypesHandled();
        void ProcessMessage(InternalMessage message);
        void RegisterAggregator(EventAggregator aggregator);
    }
}
