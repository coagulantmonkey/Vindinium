using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vindinium.Helpers
{
    public class Vector2D
    {
        #region Members
        public int X { get; private set; }
        public int Y { get; private set; }
        #endregion

        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Length()
        {
            return (X * X) + (Y * Y);
        }

        public override bool Equals(object obj)
        {
            Vector2D incomingVector = obj as Vector2D;

            if (incomingVector != null)
            {
                return incomingVector.X == this.X && incomingVector.Y == this.Y;
            }
            else
            {
                return false;
            }
        }
    }
}
