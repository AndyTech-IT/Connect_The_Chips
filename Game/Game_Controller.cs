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
        public static int MAP_WIDTH => MAP_SIZE.Width;
        public static int MAP_HEIGHT => MAP_SIZE.Height;
        public static readonly Size MAP_SIZE = new Size(5, 5);
        public static readonly RandomGenerator RANDOM = new RandomGenerator();
        public static readonly Connection_Chip[] CHIPS_POOL = new Connection_Chip[] 
        { 
            new I_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0},
            new L_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0},
            new T_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0}
        };
        private GameObject[] _all =>
            _nodes.Cast<GameObject>()
            .Union(_plaсed_chips.Cast<GameObject>())
            .Union(_obstructions.Cast<GameObject>())
            .OrderBy(o => -o.Y).ThenBy(o => -o.X).ToArray();

        private Obstruction[] _obstructions;
        private Connection_Node[] _nodes;
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
                return result.Where(p => _all.All(o => o.Position != p)).ToArray();
            }
        }

        private Connection_Chip[] Random_Chips
        {
            get
            {
                Connection_Chip[] result = new Connection_Chip[CHIPS_POOL.Length];
                for (int i = 0; i < CHIPS_POOL.Length; i++)
                {
                    Connection_Chip chip = CHIPS_POOL[RANDOM.Random.Next(CHIPS_POOL.Length)];
                    if (chip is L_Chip)
                    {
                        result[i] = new L_Chip(chip);
                    }
                    else if (chip is T_Chip)
                    {
                        result[i] = new T_Chip(chip);
                    }
                    else if (chip is I_Chip)
                    {
                        result[i] = new I_Chip(chip);
                    }

                }
                return result;
            }
        }

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
            _obstructions = new Obstruction[0];
            _plaсed_chips = new Connection_Chip[0];
            _nodes = new Connection_Node[]
            {
                new Connection_Node() { Position = new Point(0, 1), Rotation = Rotation.Degree_0 },
                new Connection_Node() { Position = new Point(1, 0), Rotation = Rotation.Degree_270 },
                new Connection_Node() { Position = new Point(3, 0), Rotation = Rotation.Degree_270 }
            };
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                Obstruction obstruction = new Obstruction() { Position = new Point(x, 0) };
                if (_nodes.Any(n => n.Position == obstruction.Position) == false)
                    _obstructions = _obstructions.Append(obstruction).ToArray();

                obstruction = new Obstruction() { Position = new Point(x, MAP_HEIGHT - 1) };
                if (_nodes.Any(n => n.Position == obstruction.Position) == false)
                    _obstructions = _obstructions.Append(obstruction).ToArray();
            }
            for (int y = 1; y < MAP_WIDTH - 1; y++)
            {
                Obstruction obstruction = new Obstruction() { Position = new Point(0, y) };
                if (_nodes.Any(n => n.Position == obstruction.Position) == false)
                    _obstructions = _obstructions.Append(obstruction).ToArray();

                obstruction = new Obstruction() { Position = new Point(MAP_WIDTH - 1, y) };
                if (_nodes.Any(n => n.Position == obstruction.Position) == false)
                    _obstructions = _obstructions.Append(obstruction).ToArray();
            }
            Game_Started?.Invoke(new Map_Data(_nodes, _obstructions));
            Chips_Pack chips = new Chips_Pack(Random_Chips, Empty_Positions);
            Chips_Given?.Invoke(chips);
        }

    }
}
