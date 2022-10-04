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
    public class Game_Controller
    {
        public const int CHIPS_PACK_SIZE = 3;
        public static readonly Size MAP_SIZE = new Size(2, 2);
        public static readonly RandomGenerator RANDOM = new RandomGenerator();
        public static readonly Connection_Chip[] CHIPS_POOL = new Connection_Chip[] 
        { 
            new I_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0},
            new L_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0},
            new T_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0}
        };

        private Obstruction[] _obstructions;
        private Connection_Node[] _connection_Nodes;

        private Connection_Chip[] _plaсed_chips;

        public Action<Map_Data> Game_Started;
        public Action<Chips_Pack> Chips_Given;
        public Action<Game_Result> Game_Finished;

        private Point[] Empty_Positions
        {
            get
            {
                Point[] result = new Point[0];
                for (int y = 0; y < MAP_SIZE.Height; y++)
                {
                    for (int x = 0; x < MAP_SIZE.Width; x++)
                    {
                        result = result.Append(new Point(x, y)).ToArray();
                    }
                }
                return result.Where(p => _connection_Nodes.All(n => n.Point != p) && _obstructions.All(o => o.Position != p)).ToArray();
            }
        }

        private Connection_Chip[] Random_Chips => new object[3].Select(
                    c => CHIPS_POOL[RANDOM.Random.Next(CHIPS_POOL.Length)]
                    ).ToArray();

        public void On_ChipsPlaced(Round_Result result)
        {
            if (result.Placed_Chips.All(c => Empty_Positions.Contains(c.Position)) == false)
                throw new Exception("Can`t place all chips!");

            _plaсed_chips = _plaсed_chips.Union(result.Placed_Chips).ToArray();
        }

        public void Start_Game(Player player)
        {
            player.Subscribe(this);
            Begin_Game();
        }

        private void Begin_Game()
        {
            Game_Started?.Invoke(new Map_Data(_connection_Nodes, _obstructions));
            Chips_Given?.Invoke(
                new Chips_Pack(Random_Chips,Empty_Positions));
        }

    }
}
