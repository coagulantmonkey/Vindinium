using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vindinium.Helpers;
using vindinium.Interfaces;

namespace vindinium
{
    public class AIManager : IAIManager
    {
        #region Members
        private string playURL;
        private Hero myHero;
        private List<Hero> heroes = new List<Hero>();
        private int currentTurn;
        private int maxTurns;
        private Tile[,] board;
        private bool printedMap;
        private ServerConnection serverConnection;
        private bool trainingMode;
        private State lastState = State.NONE;
        public bool Finished { get; private set; }
        public string ViewURL { get; private set; }
        public event EventHandler<string> ViewUrlChanged = delegate { };
        #endregion

        public AIManager()
        {
            trainingMode = ConfigManager.GetConfigKey("PlayMode") == "Training";
            serverConnection = new ServerConnection();
            serverConnection.DataReceived += serverConnection_DataReceived;
        }

        public void Deserialise(string json)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);

            DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(GameResponse));
            GameResponse gameResponse = (GameResponse)serialiser.ReadObject(stream);

            playURL = gameResponse.playUrl;

            if (ViewURL != gameResponse.viewUrl)
            {
                ViewURL = gameResponse.viewUrl;
                ViewUrlChanged(this, ViewURL);
            }


            myHero = gameResponse.hero;
            heroes = gameResponse.game.heroes;

            currentTurn = gameResponse.game.turn;
            maxTurns = gameResponse.game.maxTurns;
            Finished = gameResponse.game.finished;

            CreateBoard(gameResponse.game.board.size, gameResponse.game.board.tiles);

            if (currentTurn < maxTurns && !serverConnection.Errored)
            {
                DetermineMove();
            }
            else if (currentTurn >= maxTurns)
            {
                Console.WriteLine("Game over!");
                Console.ReadLine();
            }
            else if (serverConnection.Errored)
            {
                Console.WriteLine(serverConnection.ErrorText);
                Console.ReadLine();
            }
        }

        public void Run()
        {
            int numberOfTurns = 0;
            if (trainingMode)
            {
                Console.WriteLine("Enter the number of turns the game should run for : ");
                
                bool success = int.TryParse(Console.ReadLine(), out numberOfTurns);

                if (!success)
                {
                    Console.WriteLine("Could not parse number of turns, game will run for 10 turns.");
                    numberOfTurns = 10;
                }
            }

            serverConnection.CreateGame(trainingMode, numberOfTurns);
        }

        private void serverConnection_DataReceived(object sender, string data)
        {
            Deserialise(data);
        }

        private void CreateBoard(int size, string data)
        {
            if (board == null)
            {
                board = new Tile[size, size];
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

            if (!printedMap)
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
                board[lineID, i / 2] = tileToInsert;
            }
        }

        private void DetermineMove()
        {
            if (myHero.mineCount == 0)
            {
                if (lastState != State.MOVING_TO_MINE)
                {
                    lastState = State.MOVING_TO_MINE;
                    Vector2D destination = FindNearestNeutralMine();
                    Pathfinder.CalculatePath(myHero.BoardPosition(), destination, board);
                }                
            }
            else
            {
                if (lastState != State.MOVING_TO_ENEMY)
                {
                    lastState = State.MOVING_TO_ENEMY;
                    Vector2D destination = FindNearestEnemy();
                    Pathfinder.CalculatePath(myHero.BoardPosition(), destination, board);
                }                
            }

            serverConnection.SendCommand(playURL, Pathfinder.GetNextMove(myHero.BoardPosition()));
        }

        private Vector2D FindNearestEnemy()
        {
            var target = heroes
                .Select(hero => new { TargetVector = new Vector2D(hero.pos.x, hero.pos.y), Hero = hero })
                .Where(vector => vector.TargetVector.DistanceApart(myHero.BoardPosition()) > 0)
                .OrderBy(vector => vector.TargetVector.Length())
                .Select(vector => vector.Hero)
                .First();


            Console.WriteLine("Closest enemy located at {0}.", target.BoardPosition().DisplayString());
            return new Vector2D(target.BoardPosition().X, target.BoardPosition().Y);
        }

        private Vector2D FindNearestNeutralMine()
        {
            List<Vector2D> neutralMines = new List<Vector2D>();

            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int column = 0; column < board.GetLength(1); column++)
                {
                    if (board[row, column] == Tile.GOLD_MINE_NEUTRAL)
                    {
                        neutralMines.Add(new Vector2D(row, column));
                    }
                }
            }

            Vector2D nearestNeutralMine = neutralMines
                .OrderBy(mineLocation => mineLocation.DistanceApart(myHero.BoardPosition()))
                .First();

            Console.WriteLine("Closest neutral mine located at {0}.", nearestNeutralMine.DisplayString());
            return nearestNeutralMine;
        }

        private void OutputMapToConsole()
        {
            string horizontalBound = "";

            for (int i = 0; i <= board.GetUpperBound(0); i++)
            {
                horizontalBound += "-";
            }

            Console.WriteLine(horizontalBound);

            for (int x = 0; x <= board.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= board.GetUpperBound(0); y++)
                {
                    switch (board[x, y])
                    {
                        case Tile.FREE:
                            Console.Write(" ");
                            break;
                        case Tile.GOLD_MINE_1:
                        case Tile.GOLD_MINE_2:
                        case Tile.GOLD_MINE_3:
                        case Tile.GOLD_MINE_4:
                        case Tile.GOLD_MINE_NEUTRAL:
                            Console.Write("$");
                            break;
                        case Tile.HERO_1:
                        case Tile.HERO_2:
                        case Tile.HERO_3:
                        case Tile.HERO_4:
                            Console.Write("@");
                            break;
                        case Tile.IMPASSABLE_WOOD:
                            Console.Write("#");
                            break;
                        case Tile.TAVERN:
                            Console.Write("[");
                            break;
                    }
                }
                Console.WriteLine("|");
            }
            printedMap = true;

            Console.WriteLine(horizontalBound);
        }
    }
}
