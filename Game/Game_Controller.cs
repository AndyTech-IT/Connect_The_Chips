using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using Connect_The_Chips.Game.Chips;
using Connect_The_Chips.Players;
using RandomGenerator;

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
        public static readonly Random_Generator GENERATOR = new Random_Generator();
        public static readonly Chips_Type[] CHIPS_POOL = new Chips_Type[] 
        { 
            Chips_Type.I_Chip,
            Chips_Type.L_Chip,
            Chips_Type.T_Chip
        };

        public Action<Map_Data> Game_Started;
        public Action<Chips_Pack> Chips_Given;
        public Action<Game_Result> Game_Finished;
        public Action<int> Indexed_Game_Finished;

        private GameObject[] _all =>
            _nodes.Cast<GameObject>()
            .Union(_plaсed_chips.Cast<GameObject>())
            .Union(_obstructions.Cast<GameObject>())
            .OrderBy(o => -o.Y).ThenBy(o => -o.X).ToArray();

        private Obstruction[] _obstructions;
        private Connection_Node[] _nodes;
        private Connection_Chip[] _plaсed_chips;

        private int _index;
        private int _round;

        private Game_Controller _sync_game;

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

        private Chips_Type[] Random_Chips => GENERATOR.Next_Items(CHIPS_POOL, CHIPS_PACK_SIZE, true);

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

            if (_round++ == ROUNDS_COUNT)
                End_Game();
            else if (_sync_game is null)
                NewRound();
        }

        public Game_Controller(Game_Controller sync_game = null)
        {
            _sync_game = sync_game;
            if (sync_game is Game_Controller)
            {
                sync_game.Game_Started += On_Sync_Game_Started;
                sync_game.Chips_Given += On_Sync_Game_Chips_Given;
            }
        }

        public void Start_Game(Player player, int index = -1)
        {
            player.Subscribe(this);
            _index = index;

            if (_sync_game is null)
                Begin_Game();
        }

        private void End_Game()
        {
            Connection_Tree[] connections = new Connection_Tree[0];
            foreach(var node in _nodes)
            {
                if (connections.Any(c => c.Nodes_Points.Any(p => p == node.Position)))
                    continue;

                GameObject naibor = Get_Naibor(node, node.Connections[0]);
                if (naibor is Connection_Chip chip && chip.Connections.Any(c => (int)c == -(int)node.Connections[0]))
                    connections = connections.Append(Build_Tree(node.Connections[0], chip, node)).ToArray();
            }
            Game_Finished?.Invoke(new Game_Result(connections, _all));
            if (_index != -1)
                Indexed_Game_Finished?.Invoke(_index);
        }

        private Connection_Tree Build_Tree(Direction direction, Connection_Chip chip, Connection_Node start_node = null)
        {
            Direction skip_dir = (Direction)(-(int)direction);

            Point[] chips = new Point[] { chip.Position };
            Point[] nodes = new Point[0];
            if (start_node != null)
                nodes = new Point[] { start_node.Position };

            foreach (var dir in chip.Connections)
            {
                if (dir == skip_dir)
                    continue;
                if (Get_Naibor(chip, dir) is Connection_Node node)
                {
                    if ((int)node.Connections[0] == -(int)dir)
                        nodes = nodes.Append(node.Position).ToArray();
                    continue;
                }
                if (Get_Naibor(chip, dir) is Connection_Chip naibor)
                {
                    if (naibor.Connections.Any(d => (int)d == -(int)dir) == false)
                        continue;
                    Connection_Tree branch =  Build_Tree(dir, naibor);
                    chips = chips.Union(branch.Transfer_Chips).ToArray();
                    nodes = nodes.Union(branch.Nodes_Points).ToArray();
                }
            }
            return new Connection_Tree() { Nodes_Points = nodes, Transfer_Chips = chips };
        }

        private GameObject Get_Naibor(GameObject gameObject, Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return _all.FirstOrDefault(o => o.X == gameObject.X && o.Y == gameObject.Y - 1);
                case Direction.Left:
                    return _all.FirstOrDefault(o => o.X == gameObject.X - 1 && o.Y == gameObject.Y);
                case Direction.Bottom:
                    return _all.FirstOrDefault(o => o.X == gameObject.X && o.Y == gameObject.Y + 1);
                case Direction.Right:
                    return _all.FirstOrDefault(o => o.X == gameObject.X + 1 && o.Y == gameObject.Y);
                default:
                    throw new Exception($"Direction {direction} dont supported!");
            }
        }

        private void On_Sync_Game_Started(Map_Data sync_data)
        {
            _obstructions = sync_data.Obstructions;
            _plaсed_chips = new Connection_Chip[0];
            _nodes = sync_data.Nodes;
            _round = 0;
            Game_Started?.Invoke(sync_data);
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

        public void On_Sync_Game_Chips_Given(Chips_Pack sync_pack)
        {
            Chips_Pack chips = new Chips_Pack(sync_pack.Chips, Empty_Positions);
            Chips_Given?.Invoke(chips);
        }
        private void NewRound()
        {
            Chips_Pack chips = new Chips_Pack(Random_Chips, Empty_Positions);
            Chips_Given?.Invoke(chips);
        }

    }
}
