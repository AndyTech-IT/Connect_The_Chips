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
        public const int NODES_COUNT = 8;
        public const int CHIPS_PACK_SIZE = 3;
        public static int MAP_WIDTH => MAP_SIZE.Width;
        public static int MAP_HEIGHT => MAP_SIZE.Height;
        public static readonly Size MAP_SIZE = new Size(7, 7);
        public static readonly RandomGenerator RANDOM = new RandomGenerator();
        public static readonly Chips_Type[] CHIPS_POOL = new Chips_Type[] 
        { 
            Chips_Type.I_Chip,
            Chips_Type.L_Chip,
            Chips_Type.T_Chip
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

        private Chips_Type[] Random_Chips
        {
            get
            {
                Chips_Type[] result = new Chips_Type[CHIPS_POOL.Length];
                for (int i = 0; i < CHIPS_POOL.Length; i++)
                {
                    Chips_Type chip = CHIPS_POOL[RANDOM.Random.Next(CHIPS_POOL.Length)];
                    result[i] = chip;
                }
                return result;
            }
        }

        public void On_ChipsPlaced(Round_Result result)
        {
            if (result.Positions.All(p => Empty_Positions.Contains(p)) == false)
                throw new Exception("Can`t place all chips!");

            Connection_Chip[] result_chips = new Connection_Chip[CHIPS_PACK_SIZE];
            for (int i = 0; i < CHIPS_PACK_SIZE; i++)
            {
                switch (result.Placed_Chips[i])
                {
                    case Chips_Type.T_Chip:
                        result_chips[i] = new T_Chip();
                        break;
                        case Chips_Type.I_Chip:
                        result_chips[i] = new I_Chip();
                        break;
                    case Chips_Type.L_Chip:
                        result_chips[i] = new L_Chip();
                        break;
                    default:
                        throw new Exception($"Wrong chip type {result.Placed_Chips[i]}!");
                }
                result_chips[i].Position = result.Positions[i];
                result_chips[i].Rotation = result.Rotations[i];
            }
            _plaсed_chips = _plaсed_chips.Union(result_chips).ToArray();
            NewRound();
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
            _nodes = new Connection_Node[NODES_COUNT];

            for (int x = 1; x < MAP_WIDTH-1; x++)
            {
                _obstructions = _obstructions.Append(new Obstruction() { Position = new Point(x, 0), Rotation = Rotation.Degree_270 }).ToArray();

                _obstructions = _obstructions.Append(new Obstruction() { Position = new Point(x, MAP_HEIGHT - 1), Rotation = Rotation.Degree_90 }).ToArray();
            }
            for (int y = 1; y < MAP_HEIGHT - 1; y++)
            {
                _obstructions = _obstructions.Append(new Obstruction() { Position = new Point(0, y), Rotation = Rotation.Degree_0 }).ToArray();

                _obstructions = _obstructions.Append(new Obstruction() { Position = new Point(MAP_WIDTH - 1, y), Rotation = Rotation.Degree_180 }).ToArray();
            }

            int index = 0;
            int[] indexes = _obstructions.Select(o => index++).ToArray();
            for (int i = 0; i < NODES_COUNT; i++)
            {
                index = indexes[RANDOM.Random.Next(indexes.Length)];
                indexes = indexes.Where(x => x != index).ToArray();
                indexes = indexes.Select(x => x > index ? x - 1 : x).ToArray();
                Obstruction obstruction = _obstructions[index];
                _obstructions = _obstructions.Where(o => o != obstruction).ToArray();

                _nodes[i] = new Connection_Node() { Position = obstruction.Position, Rotation = obstruction.Rotation };
            }

            _obstructions = _obstructions.Append(new Obstruction() { Position = new Point(0, 0), Rotation = Rotation.Degree_0 }).ToArray();
            _obstructions = _obstructions.Append(new Obstruction() { Position = new Point(0, MAP_HEIGHT - 1), Rotation = Rotation.Degree_0 }).ToArray();
            _obstructions = _obstructions.Append(new Obstruction() { Position = new Point(MAP_WIDTH - 1, 0), Rotation = Rotation.Degree_0 }).ToArray();
            _obstructions = _obstructions.Append(new Obstruction() { Position = new Point(MAP_WIDTH - 1, MAP_HEIGHT - 1), Rotation = Rotation.Degree_0 }).ToArray();

            Game_Started?.Invoke(new Map_Data(_nodes, _obstructions));
            NewRound();
        }

        private void NewRound()
        {
            Chips_Pack chips = new Chips_Pack(Random_Chips, Empty_Positions);
            Chips_Given?.Invoke(chips);
        }

    }
}
