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
        #endregion

        public string DetermineMove(Hero myHero, Tile[,] board, List<Hero> heroes)
        {
            if (myHero.mineCount == 0)
            {
                Log4netManager.DebugFormat("Hero has 0 mines. Moving towards closest neutral mine.", typeof(AIManager));
                if (_lastState != State.MOVING_TO_MINE)
                {
                    Log4netManager.DebugFormat("Last state was not Moving To Mine. Determining closest neutral mine.", typeof(AIManager));
                    _lastState = State.MOVING_TO_MINE;
                    Vector2D destination = FindNearestNeutralMine(board, myHero);
                    Pathfinder.CalculatePath(myHero.BoardPosition(), destination, board);
                }
            }
            else
            {
                Log4netManager.DebugFormat("Hero has a mine. Moving towards closest enemy.", typeof(AIManager));
                if (_lastState != State.MOVING_TO_ENEMY)
                {
                    Log4netManager.DebugFormat("Last state was not Moving To Enemy. Determining closest enemy.", typeof(AIManager));
                    _lastState = State.MOVING_TO_ENEMY;
                    Vector2D destination = FindNearestEnemy(heroes, myHero);
                    Pathfinder.CalculatePath(myHero.BoardPosition(), destination, board);
                }
            }

            return Pathfinder.GetNextMove(myHero.BoardPosition());
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
    }
}
