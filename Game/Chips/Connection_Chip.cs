﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game.Chips
{
    public abstract class Connection_Chip
    {
        public Point Position;
        public int X => Position.X;
        public int Y => Position.Y;

        public Rotation Rotation;
        public abstract Direction[] Connections { get; }

        public override string ToString()
        {
            return $"{GetType().Name} at {Position}";
        }

        public override bool Equals(object obj)
        {
            return obj is Connection_Chip chip &&
                   EqualityComparer<Point>.Default.Equals(Position, chip.Position) &&
                   Rotation == chip.Rotation;
        }

        public override int GetHashCode()
        {
            int hashCode = 1876333605;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            return hashCode;
        }
    }
}
