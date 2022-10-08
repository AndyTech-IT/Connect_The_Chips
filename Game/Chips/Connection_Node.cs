using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect_The_Chips.Game.Chips;

namespace Connect_The_Chips.Game
{
    public class Connection_Node : Connection_Chip
    {
        public override Direction[] Connections
        {
            get
            {
                switch (Rotation)
                {
                    case Rotation.Degree_0:
                        return new Direction[] { Direction.Right };
                    case Rotation.Degree_90:
                        return new Direction[] { Direction.Top };
                    case Rotation.Degree_180:
                        return new Direction[] { Direction.Left };
                    case Rotation.Degree_270:
                        return new Direction[] { Direction.Bottom };
                    default:
                        throw new Exception($"Wrong node rotation {Rotation}!");
                }
            }
        }

        public override Chips_Type Chip_Type => Chips_Type.Node;

        public override string ToString()
        {
            return $"{GetType().Name} at {Position}";
        }
    }
}
