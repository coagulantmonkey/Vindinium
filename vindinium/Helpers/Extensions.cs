using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vindinium.Helpers
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
    }
}
