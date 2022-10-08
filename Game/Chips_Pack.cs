using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect_The_Chips.Game.Chips;

namespace Connect_The_Chips.Game
{
    public struct Chips_Pack
    {
        public readonly Chips_Type[] Chips;
        public readonly Point[] Empty_Positions;

        public Chips_Pack(Chips_Type[] chips, Point[] empty_positions)
        {
            Empty_Positions = empty_positions;
            Chips = chips;
        }
    }
}
