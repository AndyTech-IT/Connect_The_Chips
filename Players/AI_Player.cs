using Connect_The_Chips.Game;
using Connect_The_Chips.Game.Chips;
using Connect_The_Chips.Neular_Network;
using Neural_Network;
using RandomGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Connect_The_Chips.Players
{
    public class AI_Player : Player
    {
        public static readonly Network_Struct Network_Struct = new Network_Struct( 5 * 4 + 5 * 5 + 5 * 5 , new int[] { 40 }, 1 + 4);


        public bool Training => _training;
        public double Score => _brain.Fitnes;


        private static int Curent_ID = 0;

        private static readonly Random_Generator _generator = new Random_Generator();


        private const string TRAINING_DIRECTORY = "Training AIs";
        private const string TRAINED_DIRECTORY = "Ready AIs";


        private readonly int _id;

        private bool _training;
        private NeuralNetwork _brain;
        private InputMap _map;

       Human_Form form;

        public AI_Player(string path = null, bool training = false, bool clear = false)
        {
            _brain = new NeuralNetwork(Network_Struct);
            _training = training;

            if (training == false)
                form = new Human_Form(null);

            _id = Curent_ID++;

            if (clear)
                return;
            if (path is string && File.Exists(path))
            {
                _brain.Load(path);
                return;
            }
            if (Directory.Exists(TRAINED_DIRECTORY) && Directory.GetFiles(TRAINED_DIRECTORY).Length > 0)
            {
                Load_FromDirectory(TRAINED_DIRECTORY);
                return;
            }
            if (Directory.Exists(TRAINING_DIRECTORY) && Directory.GetFiles(TRAINING_DIRECTORY).Length > 0)
            {
                Load_FromDirectory(TRAINING_DIRECTORY);
                return;
            }
        }

        public void Union(AI_Player best, double rate)
        {
            _brain.Union(rate, best._brain);
        }

        public void Mutate(double rate, double value)
        {
            _brain.Mutate(rate, value);
        }

        public void ClearScore() => _brain.Chainge_Fitnes(-_brain.Fitnes);

        public static void Clearing_Before_Saving()
        {
            if (Directory.Exists(TRAINED_DIRECTORY))
                Directory.Delete(TRAINED_DIRECTORY, true);
            if (Directory.Exists(TRAINING_DIRECTORY))
                Directory.Delete(TRAINING_DIRECTORY, true);
        }

        public void Save(string name, bool release = false)
        {
            if (release)
            {
                Save_ToDirectory(TRAINED_DIRECTORY, name);
            }
            else
            {
                Save_ToDirectory(TRAINING_DIRECTORY, name);
            }
        }

        private void Save_ToDirectory(string directory, string name)
        {
            if (Directory.Exists(directory) == false)
                Directory.CreateDirectory(directory);
            _brain.Save($"{directory}\\{name}.{NeuralNetwork.FILE_EXSTANTION}");
        }

        private void Load_FromDirectory(string directory)
        {
            string path = _generator.Next_Item(Directory.GetFiles(directory));
            _brain.Load(path);
        }

        private double[] Map_To_Inputs(double[][][] map)
        {
            double[] result = new double[0];
            for(int i = 0; i < map.Length; i++)
            {
                foreach (var line in map[i])
                    foreach (double val in line)
                        result = result.Append(val).ToArray();
            }
            return result;
        }

        protected override void On_ChipsGiven(Chips_Pack pack)
        {
            IEnumerable <Connection_Chip> placement_chips = new Connection_Chip[0];
            IEnumerable <Point> free_pos = pack.Empty_Positions;
            foreach(var type in pack.Chips)
            {
                IEnumerable<Marked_Placement> placements = new Marked_Placement[0];
                foreach (var p in free_pos)
                {
                    Connection_Chip chip = Chip_Constructor.Get_Chip(type, p, Rotation.Degree_0);
                    double[] inputs = Map_To_Inputs(_map.With_Chip(chip));
                    double[] result = _brain.FeedForward(inputs);
                    double mark = result[0];
                    double max = result[1];
                    int index = 1;
                    for (int i = 2; i < 5; i++)
                    {
                        if (result[i] > max)
                        {
                            max = result[i];
                            index = i;
                        }

                    }
                    chip.Rotation = (Rotation)(index - 1);
                    placements = placements.Append(new Marked_Placement(chip, mark));
                }
                placements = placements.OrderBy(p => -p.Mark).ToArray();
                Connection_Chip placement_chip = placements.Select(p => p.Chip).ElementAt(0);
                placement_chips = placement_chips.Append(placement_chip);
                free_pos = free_pos.Where(p => p != placement_chip.Position);
                _map = new InputMap(_map, placement_chip);
            }
            Place_Chips(new Round_Result(this, placement_chips.ToArray()));
        }

        protected override void On_GameFinished(Game_Result result)
        {
            if (Training)
                _brain.Chainge_Fitnes(result.Score);
            if (Training == false)
            {
                form.Set_Map(result.GameObjects);
                form.Show();
            }
        }

        protected override void On_GameStarted(Map_Data data)
        {
            _map = new InputMap(data.Nodes, data.Obstructions.Where(o => o.X > 0 && o.X < Game_Controller.MAP_WIDTH - 1 && o.Y > 0 && o.Y < Game_Controller.MAP_HEIGHT - 1).ToArray());
            if (Training == false)
                form.Init_Map(data);
        }

        public override bool Equals(object obj)
        {
            return obj is AI_Player player &&
                   _id == player._id;
        }

        public override int GetHashCode()
        {
            return 1969571243 + _id.GetHashCode();
        }

        public static bool operator ==(AI_Player left, AI_Player right)
        {
            return EqualityComparer<AI_Player>.Default.Equals(left, right);
        }

        public static bool operator !=(AI_Player left, AI_Player right)
        {
            return !(left == right);
        }
    }
}
