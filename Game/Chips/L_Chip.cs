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
        public L_Chip() { }
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
                        return new[] { Direction.Left, Direction.Bottom };
                    case Rotation.Degree_90:
                        return new[] { Direction.Top, Direction.Left};
                    case Rotation.Degree_180:
                        return new[] { Direction.Right, Direction.Top };
                    case Rotation.Degree_270:
                        return new[] { Direction.Bottom, Direction.Right };
                    default:
                        throw new Exception($"Wrong chip rotation {Rotation}!");
                }
            }
        }
    }
}
