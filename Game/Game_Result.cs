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
        public readonly Connection_Tree[] Connections;

        public Game_Result(Connection_Tree[] connections, GameObject[] all)
        {
            Connections = connections;
            Score = 0;
            foreach (int score in connections.Select(c => c.Nodes_Points.Length * c.Nodes_Points.Length * c.Nodes_Points.Length - c.Transfer_Chips.Length))
            {
                Score += score;
            }
            Chips_Map = null;
            Rotations_Map = null;
            Nodes_List = null;
        }
    }
}
