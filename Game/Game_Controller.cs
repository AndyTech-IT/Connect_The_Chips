using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Connect_The_Chips.Game.Chips;
using Connect_The_Chips.Players;

namespace Connect_The_Chips.Game
{
    public class Game_Controller
    {
        public const int NODES_COUNT = 8;
        public const int OBSTRUCTIONS_COUNT = 7;
        public const int ROUNDS_COUNT = 5;
        public const int CHIPS_PACK_SIZE = 3;
        public static int MAP_WIDTH => MAP_SIZE.Width;
        public static int MAP_HEIGHT => MAP_SIZE.Height;

        public static readonly Size MAP_SIZE = new Size(7, 7);
        public static readonly RandomGenerator GENERATOR = new RandomGenerator();
        public static readonly Chips_Type[] CHIPS_POOL = new Chips_Type[] 
        { 
            Chips_Type.I_Chip,
            Chips_Type.L_Chip,
            Chips_Type.T_Chip
        };

        public Action<Map_Data> Game_Started;
        public Action<Chips_Pack> Chips_Given;
        public Action<Game_Result> Game_Finished;

        private GameObject[] _all =>
            _nodes.Cast<GameObject>()
            .Union(_plaсed_chips.Cast<GameObject>())
            .Union(_obstructions.Cast<GameObject>())
            .OrderBy(o => -o.Y).ThenBy(o => -o.X).ToArray();

        private Obstruction[] _obstructions;
        private Connection_Node[] _nodes;
        private Connection_Chip[] _plaсed_chips;

        private int _round;

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

        private Chips_Type[] Random_Chips => GENERATOR.Next_Items(CHIPS_POOL, CHIPS_PACK_SIZE);

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

            if (_round == ROUNDS_COUNT)
                return;
            else
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
            _round = 0;

            for (int x = 1; x < MAP_WIDTH-1; x++)
            {
                _obstructions = _obstructions.Union(new Obstruction[] {
                    new Obstruction() { Position = new Point(x, 0), Rotation = Rotation.Degree_270 },
                    new Obstruction() { Position = new Point(x, MAP_HEIGHT - 1), Rotation = Rotation.Degree_90 }
                }).ToArray();
            }
            for (int y = 1; y < MAP_HEIGHT - 1; y++)
            {
                _obstructions = _obstructions.Union(new Obstruction[] {
                    new Obstruction() { Position = new Point(0, y), Rotation = Rotation.Degree_0 },
                    new Obstruction() { Position = new Point(MAP_WIDTH - 1, y), Rotation = Rotation.Degree_180 }
                }).ToArray();
            }

            Obstruction[] replacement_o = GENERATOR.Next_Items(_obstructions, NODES_COUNT);
            _obstructions = _obstructions.Where(o => replacement_o.Contains(o) == false).ToArray();
            _nodes = replacement_o.Select(o => new Connection_Node() { Position = o.Position, Rotation = o.Rotation }).ToArray();

            _obstructions = _obstructions.Union(new Obstruction[] {
                new Obstruction() { Position = new Point(0, 0), Rotation = Rotation.Degree_0 },
                new Obstruction() { Position = new Point(0, MAP_HEIGHT - 1), Rotation = Rotation.Degree_0 },
                new Obstruction() { Position = new Point(MAP_WIDTH - 1, 0), Rotation = Rotation.Degree_0 },
                new Obstruction() { Position = new Point(MAP_WIDTH - 1, MAP_HEIGHT - 1), Rotation = Rotation.Degree_0 }
            }).ToArray();

            Point[] not_tryed_points = Empty_Positions;
            for (int i = 0; i < OBSTRUCTIONS_COUNT; i++)
            {
                while (true)
                {
                    Point point = GENERATOR.Next_Item(not_tryed_points);
                    not_tryed_points = not_tryed_points.Where(p => p != point).ToArray();

                    if (_nodes.Any(n
                    => n.X == point.X && n.Y == point.Y + 1
                    || n.X == point.X && n.Y == point.Y - 1
                    || n.X == point.X + 1 && n.Y == point.Y
                    || n.X == point.X - 1 && n.Y == point.Y
                    ))
                        continue;
                    _obstructions = _obstructions.Append(new Obstruction() { Position = point, Rotation = Rotation.Degree_0 }).ToArray();
                    break;
                }
            }

            Game_Started?.Invoke(new Map_Data(_nodes, _obstructions));
            NewRound();
        }

        private void NewRound()
        {
            _round++;
            Chips_Pack chips = new Chips_Pack(Random_Chips, Empty_Positions);
            Chips_Given?.Invoke(chips);
        }

    }
}
