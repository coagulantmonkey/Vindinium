using Common.DataContracts;
using Common.Messaging.Messages;

namespace Common.Helpers
{
    public static class Extensions
    {
        public static int DistanceApart(this Vector2D source, Vector2D target)
        {
            Vector2D distance = new Vector2D((source.X - target.X), (source.Y - target.Y));
            return distance.Length();
        }

        public static Vector2D BoardPosition(this Hero hero)
        {
            return new Vector2D(hero.pos.x, hero.pos.y);
        }

        public static Vector2D Subtract(this Vector2D source, Vector2D target)
        {
            return new Vector2D((source.X - target.X), (source.Y - target.Y));
        }

        public static string DisplayString(this Vector2D source)
        {
            return string.Format("({0},{1})", source.X, source.Y);
        }

        public static string DisplayString(this StartNewGameMessage message)
        {
            return "Number of turns : " + message.NumberOfTurns +
                "Server Url : " + message.ServerURL +
                "Play mode : " + (message.TrainingMode ? "Training" : "Arena");
        }
    }
}
