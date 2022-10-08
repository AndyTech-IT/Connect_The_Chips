using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game.Chips
{
    public class I_Chip : Connection_Chip
    {
        public I_Chip() { }
        public I_Chip(Connection_Chip chip) : base(chip)
        {
        }

        public override Direction[] Connections
        {
            get
            {
                switch (Rotation)
                {
                    case Rotation.Degree_0:
                    case Rotation.Degree_180:
                        return new[] { Direction.Top, Direction.Bottom };
                    case Rotation.Degree_90:
                    case Rotation.Degree_270:
                        return new[] { Direction.Left, Direction.Right };
                    default:
                        throw new Exception($"Wrong chip rotation {Rotation}!");
                }
            }
        }
    }
}
