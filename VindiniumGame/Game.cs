using AIController;
using Common.DataContracts;
using Common.Enums;
using Common.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Common.Helpers;

namespace VindiniumGame
{
    public class Game
    {
        #region Members
        public string ID { get; set; }
        private string _playUrl;
        private string _viewUrl;
        private int _currentTurn;
        private int _maxTurns;
        private bool _finished;
        private bool _loggedMap;
        private ServerConnection _serverConnection;
        private Hero _myHero;
        private List<Hero> _heroes = new List<Hero>();
        private Tile[,] _board;
        private AIManager _ai;

        public event EventHandler<string> ViewUrlChanged = delegate { };
        #endregion

        public Game(StartNewGameMessage message)
        {
            Log4netManager.DebugFormat("New game created.", typeof(Game));

            _ai = new AIManager();

            _serverConnection = new ServerConnection(message);
            _serverConnection.DataReceived += ServerDataReceived;
            _serverConnection.ErrorOccurred += ServerErrorOccurred;
        }

        public void BeginNewGame()
        {
            _serverConnection.BeginNewGame();
        }

        private void ServerErrorOccurred(object sender, string e)
        {
            // TODO - Handle server error
        }

        private void ServerDataReceived(object sender, string e)
        {
            Deserialise(e);
        }

        public void Deserialise(string json)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);

            DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(GameResponse));
            GameResponse gameResponse = (GameResponse)serialiser.ReadObject(stream);

            _playUrl = gameResponse.playUrl;

            if (_viewUrl != gameResponse.viewUrl)
            {
                _viewUrl = gameResponse.viewUrl;
                ViewUrlChanged(this, _viewUrl);
            }


            _myHero = gameResponse.hero;
            _heroes = gameResponse.game.heroes;

            _currentTurn = gameResponse.game.turn;
            _maxTurns = gameResponse.game.maxTurns;
            _finished = gameResponse.game.finished;

            CreateBoard(gameResponse.game.board.size, gameResponse.game.board.tiles);

            if (_currentTurn < _maxTurns)
            {
                _serverConnection.SendCommand(_playUrl, _ai.DetermineMove(_myHero, _board, _heroes));
            }
            else if (_currentTurn >= _maxTurns)
            {
                Log4netManager.DebugFormat("Game over!", typeof(Game));
                // TODO - Handle game over
            }
        }

        private void CreateBoard(int size, string data)
        {
            if (_board == null)
            {
                _board = new Tile[size, size];
            }

            List<string> boardLines = new List<string>();
            int stringStartPoint = 0;
            int lineLength = size * 2;

            for (int i = 0; i < size; i++)
            {
                string boardLine = data.Substring(stringStartPoint, size * 2);
                boardLines.Add(boardLine);
                stringStartPoint += lineLength;
            }

            Task[] tasks = new Task[size];

            for (int i = 0; i < boardLines.Count; i++)
            {
                var temp = i;

                tasks[i] = Task.Factory.StartNew(() =>
                {
                    ParseBoardLine(temp, boardLines[temp]);
                });

            }

            Task.WaitAll(tasks);

            if (!_loggedMap)
            {
                OutputMapToConsole();
            }
        }

        private void ParseBoardLine(int lineID, string boardLine)
        {
            for (int i = 0; i < boardLine.Length; i += 2)
            {
                string substring = boardLine.Substring(i, 2);
                Tile tileToInsert = Tile.FREE;

                switch (substring[0])
                {
                    case '#':
                        tileToInsert = Tile.IMPASSABLE_WOOD;
                        break;
                    case '@':
                        switch (substring[1])
                        {
                            case '1':
                                tileToInsert = Tile.HERO_1;
                                break;
                            case '2':
                                tileToInsert = Tile.HERO_2;
                                break;
                            case '3':
                                tileToInsert = Tile.HERO_3;
                                break;
                            case '4':
                                tileToInsert = Tile.HERO_4;
                                break;
                        }
                        break;
                    case '[':
                        tileToInsert = Tile.TAVERN;
                        break;
                    case '$':
                        switch (substring[1])
                        {
                            case '1':
                                tileToInsert = Tile.GOLD_MINE_1;
                                break;
                            case '2':
                                tileToInsert = Tile.GOLD_MINE_2;
                                break;
                            case '3':
                                tileToInsert = Tile.GOLD_MINE_3;
                                break;
                            case '4':
                                tileToInsert = Tile.GOLD_MINE_4;
                                break;
                            case '-':
                                tileToInsert = Tile.GOLD_MINE_NEUTRAL;
                                break;
                        }
                        break;
                }

                _board[lineID, i / 2] = tileToInsert;
            }
        }

        private void OutputMapToConsole()
        {
            List<string> mapLines = new List<string>();
            string horizontalBound = "";

            for (int i = 0; i <= _board.GetUpperBound(0); i++)
            {
                horizontalBound += "-";
            }

            horizontalBound += "--";

            mapLines.Add(horizontalBound);

            for (int x = 0; x <= _board.GetUpperBound(0); x++)
            {
                string mapLine = "|";

                for (int y = 0; y <= _board.GetUpperBound(0); y++)
                {
                    switch (_board[x, y])
                    {
                        case Tile.FREE:
                            mapLine += (" ");
                            break;
                        case Tile.GOLD_MINE_1:
                        case Tile.GOLD_MINE_2:
                        case Tile.GOLD_MINE_3:
                        case Tile.GOLD_MINE_4:
                        case Tile.GOLD_MINE_NEUTRAL:
                            mapLine += ("$");
                            break;
                        case Tile.HERO_1:
                        case Tile.HERO_2:
                        case Tile.HERO_3:
                        case Tile.HERO_4:
                            mapLine += ("@");
                            break;
                        case Tile.IMPASSABLE_WOOD:
                            mapLine += ("#");
                            break;
                        case Tile.TAVERN:
                            mapLine += ("[");
                            break;
                    }
                }
                mapLine += ("|");

                mapLines.Add(mapLine);
            }
            _loggedMap = true;

            mapLines.Add(horizontalBound);

            foreach(string mapLine in mapLines)
            {
                Log4netManager.DebugFormat(mapLine, typeof(Game));
            }
        }
    }
}
