using Common.Configuration;
using Common.Messaging;
using Common.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Helpers;

namespace ConfigProvider
{
    [Export(typeof(IMessageConsumer))]
    public class ConfigProvider : IMessageConsumer
    {
        #region Members
        GameSettingsConfiguration gameSettings;
        EventAggregator aggregator;
        #endregion

        #region IMessageConsumer
        public List<Type> GetMessageTypesHandled()
        {
            return new List<Type>
            {
                typeof(ConfigRequestMessage),
                typeof(GameSettingsConfigUpdated)
            };
        }

        public void ProcessMessage(InternalMessage message)
        {
            if (message is ConfigRequestMessage)
            {
                Log4netManager.DebugFormat("ConfigRequestMessage received.", typeof(ConfigProvider));
                ProcessConfigRequest(message);
            }
            if (message is GameSettingsConfigUpdated)
            {
                Log4netManager.DebugFormat("GameSettingsConfigUpdated received.", typeof(ConfigProvider));
                ProcessGameSettingsUpdate(message);
            }
        }

        public void RegisterAggregator(EventAggregator aggregator)
        {
            this.aggregator = aggregator;
        }
        #endregion

        public ConfigProvider()
        {
        }

        private void ProcessConfigRequest(InternalMessage message)
        {
            var configMessage = message as ConfigRequestMessage;

            if (configMessage != null)
            {
                if (configMessage.Callback != null)
                {
                    ReturnRequestedConfigSection(configMessage);
                }
                else
                {
                    Log4netManager.DebugFormat("There was no callback associated with the Config Request. Message was not processed.", typeof(ConfigProvider));
                }
            }
        }

        private void ProcessGameSettingsUpdate(InternalMessage message)
        {
            // TODO : Handle ProcessGameSettingsUpdate message
        }

        private void ReturnRequestedConfigSection(ConfigRequestMessage message)
        {
            switch (message.RequestedSection)
            {
                case AvailableSections.GameSettings:
                    if (gameSettings == null)
                        gameSettings = (GameSettingsConfiguration)ConfigurationManager.GetSection("ApplicationConfiguration/GameSettingsConfiguration");

                    message.Callback(gameSettings);
                    break;
                default:
                    Log4netManager.WarnFormat(string.Format("The requested section, {0} was not handled. The callback was not invoked.", 
                        message.RequestedSection.ToString()), typeof(ConfigProvider));
                    break;
            }
        }        
    }
}
