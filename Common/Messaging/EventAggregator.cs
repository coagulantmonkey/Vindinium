using Common.Messaging;
using Common.Messaging.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Helpers;

namespace Common.Messaging
{
    public sealed class EventAggregator
    {
        #region Members
        private static readonly EventAggregator instance = new EventAggregator();
        [ImportMany(typeof(IMessageConsumer))]
        private IEnumerable<IMessageConsumer> discoveredListeners;
        private CompositionContainer container;
        private ConcurrentDictionary<Type, List<IMessageConsumer>> listeners =
            new ConcurrentDictionary<Type, List<IMessageConsumer>>();
        private static readonly object listLock = new object();
        #endregion

        #region Singleton Implementation
        static EventAggregator()
        {
        }

        private EventAggregator()
        {
            //Initialise();
        }

        public static EventAggregator Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public void Initialise()
        {
            DiscoverListeners();

            if (discoveredListeners.Any())
            {
                foreach (IMessageConsumer listener in discoveredListeners)
                {
                    AddHandledMessageTypesToDictionary(listener);
                }
            }
            else
            {
                Log4netManager.DebugFormat("No listeners discovered.", typeof(EventAggregator));
            }
        }

        private void DiscoverListeners()
        {
            AggregateCatalog catalog = new AggregateCatalog();
            string location = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            catalog.Catalogs.Add(new DirectoryCatalog(location));
            container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionEx)
            {
                Log4netManager.LogException("EventAggregator not Initialised correctly.", compositionEx, typeof(EventAggregator));
            }
            catch (Exception ex)
            {
                Log4netManager.LogException("EventAggregator not Initialised correctly.", ex, typeof(EventAggregator));
            }
        }

        private void AddHandledMessageTypesToDictionary(IMessageConsumer listener)
        {
            foreach (Type messageType in listener.GetMessageTypesHandled())
            {
                RegisterAsListener(messageType, listener);
            }
        }

        public bool RegisterAsListener(Type messageType, IMessageConsumer listener)
        {
            try
            {
                listeners.AddOrUpdate(messageType,
                    (key) =>
                    {
                        return new List<IMessageConsumer>() { listener };
                    },
                    (key, existingList) =>
                    {
                        lock (listLock)
                        {
                            existingList.Add(listener);
                        }

                        return existingList;
                    });

                Log4netManager.DebugFormat(string.Format("Added {0} as a listener for {1}.", listener.GetType().Name, messageType.Name),
                    typeof(EventAggregator));

                return true;
            }
            catch (Exception ex)
            {
                Log4netManager.LogException(string.Format("Could not add {0} as a listener for {1}.", listener.GetType().Name, messageType.Name),
                    ex, typeof(EventAggregator));

                return false;
            }
        }

        public void ProcessMessage(InternalMessage message)
        {
            Log4netManager.DebugFormat(string.Format("Message of type {0} received.", message.GetType()), typeof(EventAggregator));

            if (listeners.ContainsKey(message.GetType()))
            {
                List<IMessageConsumer> messageConsumers = listeners[message.GetType()];
                Log4netManager.DebugFormat(string.Format("{0} listeners found for {1}.", messageConsumers.Count, message.GetType()), typeof(EventAggregator));

                foreach (IMessageConsumer messageConsumer in messageConsumers)
                {
                    messageConsumer.ProcessMessage(message);
                }
            }
            else
            {
                Log4netManager.WarnFormat(string.Format("No listeners found for {0}.", message.GetType()), typeof(EventAggregator));
            }
        }
    }
}
