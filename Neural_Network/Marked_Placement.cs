using Connect_The_Chips.Game.Chips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Neular_Network
{
    public struct Marked_Placement
    {
        public readonly Connection_Chip Chip;
        public readonly double Mark;

        public Marked_Placement(Connection_Chip chip, double mark)
        {
            Chip = chip;
            Mark = mark;
        }
    }
}
