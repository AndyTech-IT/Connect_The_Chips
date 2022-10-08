using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game.Chips
{
    public class L_Chip: Connection_Chip
    {
        public L_Chip():base() { }
        public L_Chip(Connection_Chip chip) : base(chip)
        {
        }

        public override Direction[] Connections
        {
            get
            {
                switch (Rotation)
                {
                    case Rotation.Degree_0:
                        return new[] { Direction.Right, Direction.Bottom };
                    case Rotation.Degree_90:
                        return new[] { Direction.Top, Direction.Right };
                    case Rotation.Degree_180:
                        return new[] { Direction.Left, Direction.Top };
                    case Rotation.Degree_270:
                        return new[] { Direction.Bottom, Direction.Left };
                    default:
                        throw new Exception($"Wrong chip rotation {Rotation}!");
                }
            }
        }

        public override Chips_Type Chip_Type => Chips_Type.L_Chip;
    }
}
