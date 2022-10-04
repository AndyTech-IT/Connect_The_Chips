using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game
{
    public struct Map_Data
    {
        public readonly Connection_Node[] Nodes;
        public readonly Obstruction[] Obstructions;

        public Map_Data(Connection_Node[] nodes, Obstruction[] obstructions)
        {
            Nodes = nodes;
            Obstructions = obstructions;
        }
    }
}
