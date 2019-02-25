using Common.Enums;
using Common.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Pathdinder
{
    public class Pathfinder
    {
        private List<AStarNode> openNodes = new List<AStarNode>();
        private List<AStarNode> closedNodes = new List<AStarNode>();
        private List<AStarNode> path = new List<AStarNode>();
        private Tile[,] worldMap;
        private Vector2D startingPoint;
        private Vector2D destination;
        private int boardHeight;
        private int boardWidth;
        private Log4netManager _logger;

        public Pathfinder()
        {
            _logger = new Log4netManager();
        }

        public void CalculatePath(Vector2D startPoint, Vector2D endPoint, Tile[,] board)
        {
            _logger.DebugFormat(string.Format("Calculating path between {0} and {1}.", startPoint.DisplayString(), endPoint.DisplayString()),
                typeof(Pathfinder));

            openNodes.Clear();
            closedNodes.Clear();
            path.Clear();

            worldMap = board;
            boardHeight = worldMap.GetUpperBound(0);
            boardWidth = worldMap.GetUpperBound(0);
            startingPoint = startPoint;
            destination = endPoint;

            if (BeginPathFinding())
            {
                _logger.DebugFormat("Path finding completed successfully.", typeof(Pathfinder));
            }
            else
            {
                _logger.DebugFormat("Path finding did not complete successfully.", typeof(Pathfinder));
            }
        }

        private bool BeginPathFinding()
        {

            AStarNode currentNode = new AStarNode()
            {
                WorldPosition = startingPoint
            };

            CheckAdjacentNodes(currentNode);

            while (!TargetFound() && openNodes.Any())
            {
                AStarNode nextNode = openNodes
                    .OrderByDescending(openNode => openNode.ManhattanScore(destination))
                    .First();

                openNodes.Remove(nextNode);
                closedNodes.Add(nextNode);

                if (nextNode != null)
                {
                    CheckAdjacentNodes(nextNode);
                }
                else
                {
                    return false;
                }
            }

            ConstructPath();

            return true;
        }

        private void CheckAdjacentNodes(AStarNode currentNode)
        {
            Vector2D tempVector = currentNode.WorldPosition;

            if (tempVector.X > 0)
            {
                AStarNode node = new AStarNode()
                {
                    WorldPosition = new Vector2D(tempVector.X - 1, tempVector.Y),
                    ParentNode = currentNode
                };

                FilterNode(node);
            }
            if (tempVector.X < boardHeight)
            {
                AStarNode node = new AStarNode()
                {
                    WorldPosition = new Vector2D(tempVector.X + 1, tempVector.Y),
                    ParentNode = currentNode
                };

                FilterNode(node);
            }
            if (tempVector.Y > 0)
            {
                AStarNode node = new AStarNode()
                {
                    WorldPosition = new Vector2D(tempVector.X, tempVector.Y - 1),
                    ParentNode = currentNode
                };

                FilterNode(node);
            }
            if (tempVector.Y < boardWidth)
            {
                AStarNode node = new AStarNode()
                {
                    WorldPosition = new Vector2D(tempVector.X, tempVector.Y + 1),
                    ParentNode = currentNode
                };

                FilterNode(node);
            }
        }

        private void FilterNode(AStarNode node)
        {
            AStarNode existingClosedNode = closedNodes
                .Where(closedNode => closedNode.Equals(node))
                .FirstOrDefault();

            if (existingClosedNode == null)
            {
                if (worldMap[node.WorldPosition.X, node.WorldPosition.Y] == Tile.FREE)
                {
                    AStarNode existingOpenNode = openNodes
                        .Where(openNode => openNode.Equals(node))
                        .FirstOrDefault();

                    if (existingOpenNode == null)
                    {
                        openNodes.Add(node);
                    }
                    else
                    {
                        // TODO - Recheck the node score.
                    }
                }
                else
                {
                    closedNodes.Add(node);
                }
            }
        }

        private bool TargetFound()
        {
            AStarNode targetNode = closedNodes
                .Where(closedNode => closedNode.WorldPosition.Equals(destination))
                .FirstOrDefault();

            return (targetNode != null);
        }

        private void ConstructPath()
        {
            AStarNode targetNode = closedNodes
                .Where(closedNode => closedNode.WorldPosition.Equals(destination))
                .FirstOrDefault();

            if (targetNode != null)
            {
                path.Add(targetNode);

                while (targetNode.ParentNode != null && !targetNode.ParentNode.WorldPosition.Equals(startingPoint))
                {
                    path.Insert(0, targetNode.ParentNode);
                    targetNode = targetNode.ParentNode;
                }
            }
            else
            {
                _logger.ErrorFormat("Target node was not found in closed nodes list when trying to construct path.", typeof(Pathfinder));
            }

            _logger.DebugFormat("Steps to destination :", typeof(Pathfinder));
            foreach (AStarNode node in path)
            {
                _logger.DebugFormat(node.WorldPosition.DisplayString(), typeof(Pathfinder));
            }
        }

        public string GetNextMove(Vector2D currentPosition)
        {
            if (path.Any())
            {
                AStarNode nextNode = path[0];
                path.RemoveAt(0);

                Vector2D directionVector = nextNode.WorldPosition.Subtract(currentPosition);

                _logger.DebugFormat(string.Format("Current position is {0}.", currentPosition.DisplayString()), typeof(Pathfinder));
                _logger.DebugFormat(string.Format("Next node position is {0}.", nextNode.WorldPosition.DisplayString()), typeof(Pathfinder));

                if (directionVector.Length() == 1)
                {
                    if (directionVector.X == 1)
                    {
                        return Direction.South;
                    }
                    else if (directionVector.X == -1)
                    {
                        return Direction.North;
                    }
                    if (directionVector.Y == 1)
                    {
                        return Direction.East;
                    }
                    else if (directionVector.Y == -1)
                    {
                        return Direction.West;
                    }
                }
                else
                {
                    _logger.ErrorFormat(string.Format("Direction vector was not equal to 1. Length() returned {0}. Returning stay command.",
                        directionVector.Length()), typeof(Pathfinder));
                    _logger.ErrorFormat(string.Format("Direction vector is {0}.", directionVector.DisplayString()), typeof(Pathfinder));

                    path.Clear();
                }
            }
            else
            {
                _logger.ErrorFormat("Tried to get next move and the path was empty. Returning stay command.", typeof(Pathfinder));
            }

            return Direction.Stay;
        }

        public bool MovesRemaining()
        {
            return path.Any();
        }
    }
}