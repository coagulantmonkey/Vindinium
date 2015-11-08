using Common.DataContracts;
using Common.Enums;
using Common.Helpers;
using Pathdinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIController
{
    public class AIManager
    {
        #region Members
        private State _lastState = State.NONE;
        private Pathfinder _pathfinder;
        #endregion

        public string DetermineMove(Hero myHero, Tile[,] board, List<Hero> heroes)
        {
            if (_pathfinder == null)
                _pathfinder = new Pathfinder();

            Log4netManager.DebugFormat("Moving towards nearest mine.", typeof(AIManager));

            if (_pathfinder.MovesRemaining())
            {
                Log4netManager.DebugFormat("Moves left in current path.", typeof(AIManager));
                return _pathfinder.GetNextMove(myHero.BoardPosition());
            }
            else
            {
                Log4netManager.DebugFormat("No moves left in current path.", typeof(AIManager));
                _pathfinder.CalculatePath(myHero.BoardPosition(), FindNearestNeutralMine(board, myHero), board);
                return _pathfinder.GetNextMove(myHero.BoardPosition());
            }
        }

        private Vector2D FindNearestEnemy(List<Hero> heroes, Hero myHero)
        {
            var target = heroes
                .Select(hero => new { TargetVector = new Vector2D(hero.pos.x, hero.pos.y), Hero = hero })
                .Where(vector => vector.TargetVector.DistanceApart(myHero.BoardPosition()) > 0)
                .OrderBy(vector => vector.TargetVector.Length())
                .Select(vector => vector.Hero)
                .First();


            Log4netManager.DebugFormat(string.Format("Closest enemy located at {0}.", target.BoardPosition().DisplayString()), typeof(AIManager));
            return new Vector2D(target.BoardPosition().X, target.BoardPosition().Y);
        }

        private Vector2D FindNearestNeutralMine(Tile[,] board, Hero myHero)
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

            if (neutralMines.Any())
            {
                Log4netManager.DebugFormat(string.Format("Found {0} neutral mines.", neutralMines.Count), typeof(AIManager));
                Vector2D nearestNeutralMine = neutralMines
                    .OrderBy(mineLocation => mineLocation.DistanceApart(myHero.BoardPosition()))
                    .First();

                Log4netManager.DebugFormat(string.Format("Closest neutral mine located at {0}.", nearestNeutralMine.DisplayString()), typeof(AIManager));
                return nearestNeutralMine;
            }
            else
            {
                Log4netManager.DebugFormat("Found 0 neutral mines, returning hero's position.", typeof(AIManager));
                return myHero.BoardPosition();
            }
        }

        private Vector2D FindNearestUnownedMine(Tile[,] board, Hero myHero)
        {
            // TODO
            return new Vector2D(0,0);
        }
    }
}
