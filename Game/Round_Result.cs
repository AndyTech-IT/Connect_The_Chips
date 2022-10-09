using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect_The_Chips.Game.Chips;
using Connect_The_Chips.Players;

namespace Connect_The_Chips.Game
{
    public struct Round_Result
    {
        public Chips_Type[] Placed_Chips;
        public Point[] Positions;
        public Rotation[] Rotations;
        public Player Player;

        public Round_Result(Player player,  Connection_Chip[] placement_chips)
        {
            Player = player;
            Placed_Chips = new Chips_Type[Game_Controller.CHIPS_PACK_SIZE];
            Positions = new Point[Game_Controller.CHIPS_PACK_SIZE];
            Rotations = new Rotation[Game_Controller.CHIPS_PACK_SIZE];
            for (int i = 0; i < Game_Controller.CHIPS_PACK_SIZE; i++)
            {
                Placed_Chips[i] = placement_chips[i].Chip_Type;
                Positions[i] = placement_chips[i].Position;
                Rotations[i] = placement_chips[i].Rotation;
            }
        }
    }
}
