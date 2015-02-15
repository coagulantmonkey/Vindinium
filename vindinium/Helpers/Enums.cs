﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vindinium
{
    public enum Tile
    {
        IMPASSABLE_WOOD,
        FREE,
        HERO_1,
        HERO_2,
        HERO_3,
        HERO_4,
        TAVERN,
        GOLD_MINE_NEUTRAL,
        GOLD_MINE_1,
        GOLD_MINE_2,
        GOLD_MINE_3,
        GOLD_MINE_4
    }

    public enum State
    {
        MOVING_TO_ENEMY,
        MOVING_TO_MINE,
        NONE
    }

    public class Direction
    {
        public const string Stay = "Stay";
        public const string North = "North";
        public const string East = "East";
        public const string South = "South";
        public const string West = "West";
    }
}
