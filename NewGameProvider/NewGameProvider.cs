using Common.Helpers;
using Common.Messaging;
using Common.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using VindiniumGame;

namespace NewGameProvider
{
    [Export(typeof(IMessageConsumer))]
    public class NewGameProvider : IMessageConsumer
    {
        private EventAggregator aggregator;
        private List<Game> _games;
        private readonly Log4netManager _logger;

        public List<Type> GetMessageTypesHandled()
        {
            return new List<Type>
            {
                typeof(StartNewGameMessage)
            };
        }

        public void ProcessMessage(InternalMessage message)
        {
            if (message is StartNewGameMessage)
            {
                _logger.DebugFormat("StartNewGameMessage received.", typeof(NewGameProvider));
                Task.Factory.StartNew(() =>
                    {
                        BeginNewGame((StartNewGameMessage)message);
                    });
            }
        }

        public void RegisterAggregator(EventAggregator aggregator)
        {
            this.aggregator = aggregator;
        }

        public NewGameProvider()
        {
            _games = new List<Game>();
            _logger = new Log4netManager();
        }

        private void BeginNewGame(StartNewGameMessage message)
        {
            _logger.DebugFormat(string.Format("Starting new game : {0}", message.DisplayString()), typeof(NewGameProvider));
            Game game = new Game(message);
            game.ViewUrlChanged += game_ViewUrlChanged;
            _games.Add(game);
            game.BeginNewGame();
        }

        void game_ViewUrlChanged(object sender, string e)
        {
            EventAggregator.Instance.ProcessMessage(new NewGameStartedMessage
                {
                    ViewUrl = e
                });
        }
    }
}
