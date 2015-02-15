using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vindinium.Helpers
{
    public static class Pathfinder
    {
        private static List<AStarNode> openNodes = new List<AStarNode>();
        private static List<AStarNode> closedNodes = new List<AStarNode>();
        private static List<AStarNode> path = new List<AStarNode>();
        private static Tile[,] worldMap;
        private static Vector2D startingPoint;
        private static Vector2D destination;
        private static int boardHeight;
        private static int boardWidth;

        public static void CalculatePath(Vector2D startPoint, Vector2D endPoint, Tile[,] board)
        {
            worldMap = board;
            boardHeight = worldMap.GetUpperBound(0);
            boardWidth = worldMap.GetUpperBound(0);
            startingPoint = startPoint;
            destination = endPoint;

            BeginPathFinding();
        }

        private static bool BeginPathFinding()
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

        private static void CheckAdjacentNodes(AStarNode currentNode)
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

        private static void FilterNode(AStarNode node)
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

        private static bool TargetFound()
        {
            AStarNode targetNode = closedNodes
                .Where(closedNode => closedNode.WorldPosition.Equals(destination))
                .FirstOrDefault();

            return (targetNode != null);
        }

        private static void ConstructPath()
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
                Console.WriteLine("Target node was not found in closed nodes list when trying to construct path.");
            }

            Console.WriteLine("Steps to destination :");
            foreach (AStarNode node in path)
            {
                Console.WriteLine(node.WorldPosition.DisplayString());
            }
        }

        public static string GetNextMove(Vector2D currentPosition)
        {
            if (path.Any())
            {
                AStarNode nextNode = path[0];
                path.RemoveAt(0);

                Vector2D directionVector = nextNode.WorldPosition.Subtract(currentPosition);

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
                    Console.WriteLine("Direction vector was not equal to 1. Length() returned {0}. Returning stay command.", directionVector.Length());
                }
            }
            else
            {
                Console.WriteLine("Tried to get next move and the path was empty. Returning stay command.");
            }

            return Direction.Stay;
        }
    }
}
