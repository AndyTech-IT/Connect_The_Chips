using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect_The_Chips.Game.Chips;

namespace Connect_The_Chips.Game
{
    public struct Game_Result
    {
        public readonly int Score;
        public readonly Connection_Chip[,] Chips_Map;
        public readonly Rotation[,] Rotations_Map;
        public readonly Connection_Node[,] Nodes_List;
    }
}
