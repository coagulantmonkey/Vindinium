using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class AStarNode
    {
        public Vector2D WorldPosition { get; set; }
        public AStarNode ParentNode { get; set; }

        public int ManhattanScore(Vector2D destination, int movementScore = 1)
        {
            int xScore = Math.Abs(WorldPosition.X - destination.X);
            int yScore = Math.Abs(WorldPosition.Y - destination.Y);

            return movementScore * (xScore + yScore);
        }

        public override bool Equals(object obj)
        {
            AStarNode incomingNode = obj as AStarNode;

            if (incomingNode != null)
            {
                return incomingNode.WorldPosition.Equals(this.WorldPosition);
            }
            else
            {
                return false;
            }
        }
    }
}
