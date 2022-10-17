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
        public readonly Connection_Tree[] Connections;
        public readonly int[] Connections_Score;
        public readonly GameObject[] GameObjects;

        public Game_Result(Connection_Tree[] connections, GameObject[] all)
        {
            GameObjects = all;
            Connections = connections;
            Score = 0;
            Connections_Score = connections.Select(c => c.Nodes_Points.Length * c.Nodes_Points.Length /*+ 1 - c.Transfer_Chips.Length*/).ToArray();
            foreach (int score in Connections_Score)
            {
                Score += score;
            }
        }
    }
}
