using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game.Chips
{
    public abstract class Connection_Chip: GameObject
    {
        public abstract Direction[] Connections { get; }
        public abstract Chips_Type Chip_Type { get; }

        public override string ToString()
        {
            return $"{GetType().Name} at {Position}";
        }

        public Connection_Chip() 
        {
            Position = new Point(-1);
            Rotation = Rotation.Degree_0;
        }

        public Connection_Chip(Connection_Chip chip)
        {
            Position = chip.Position;
            Rotation = chip.Rotation;
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
