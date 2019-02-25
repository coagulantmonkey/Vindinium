using Common.Configuration;
using Common.Messaging;
using Common.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using Common.Helpers;

namespace ConfigProvider
{
    [Export(typeof(IMessageConsumer))]
    public class ConfigProvider : IMessageConsumer
    {
        private EventAggregator _aggregator;
        private GameSettingsConfiguration _gameSettings;
        private readonly Log4netManager _logger;

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
                _logger.DebugFormat("ConfigRequestMessage received.", typeof(ConfigProvider));
                ProcessConfigRequest(message);
            }
            if (message is GameSettingsConfigUpdated)
            {
                _logger.DebugFormat("GameSettingsConfigUpdated received.", typeof(ConfigProvider));
                ProcessGameSettingsUpdate(message);
            }
        }

        public void RegisterAggregator(EventAggregator aggregator)
        {
            _aggregator = aggregator;
        }

        public ConfigProvider()
        {
            _logger = new Log4netManager();
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
                    _logger.DebugFormat("There was no callback associated with the Config Request. Message was not processed.", typeof(ConfigProvider));
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
                    if (_gameSettings == null)
                        _gameSettings = (GameSettingsConfiguration)ConfigurationManager.GetSection("ApplicationConfiguration/GameSettingsConfiguration");

                    message.Callback(_gameSettings);
                    break;
                default:
                    _logger.WarnFormat(string.Format("The requested section, {0} was not handled. The callback was not invoked.",
                        message.RequestedSection.ToString()), typeof(ConfigProvider));
                    break;
            }
        }
    }
}
