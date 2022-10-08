using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game.Chips
{
    public class T_Chip : Connection_Chip
    {
        public T_Chip() :base() { }
        public T_Chip(Connection_Chip chip) : base(chip)
        {
        }

        public override Direction[] Connections
        {
            get
            {
                switch (Rotation)
                {
                    case Rotation.Degree_0:
                        return new[] { Direction.Left, Direction.Bottom, Direction.Right };
                    case Rotation.Degree_90:
                        return new[] { Direction.Top, Direction.Right, Direction.Bottom };
                    case Rotation.Degree_180:
                        return new[] { Direction.Right, Direction.Top, Direction.Left };
                    case Rotation.Degree_270:
                        return new[] { Direction.Bottom, Direction.Left, Direction.Top };
                    default:
                        throw new Exception($"Wrong chip rotation {Rotation}!");
                }
            }
        }

        public override Chips_Type Chip_Type => Chips_Type.T_Chip;
    }
}
