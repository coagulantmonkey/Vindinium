namespace Common.Helpers
{
    public class Vector2D
    {
        public int X { get; private set; }
        public int Y { get; private set; }

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
