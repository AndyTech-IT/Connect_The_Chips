using Connect_The_Chips.Game;
using Connect_The_Chips.Game.Chips;
using Connect_The_Chips.Neular_Network;
using Neural_Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect_The_Chips.Players
{
    public class AI_Player : Player
    {
        public static readonly Network_Struct Network_Struct = new Network_Struct(new int[] { 5 * 4, 5 * 5, 5 * 5 }, new int[] { 5 * 5 }, 1);
        private NeuralNetwork _brain;
        private InputMap _map;
        private Human_Form form;

        public AI_Player()
        {
            form = new Human_Form(null);
            _brain = new NeuralNetwork(Network_Struct);
        }

        private double[][] Map_To_Inputs(double[][][] map)
        {
            double[][] result = new double[map.Length][];
            for(int i = 0; i < map.Length; i++)
            {
                result[i] = new double[0];
                foreach (var line in map[i])
                    foreach (double val in line)
                        result[i] = result[i].Append(val).ToArray();
            }
            return result;
        }

        protected override void On_ChipsGiven(Chips_Pack pack)
        {
            Connection_Chip[] placement_chips = new Connection_Chip[0];
            Point[] free_pos = pack.Empty_Positions;
            foreach(var type in pack.Chips)
            {
                Marked_Placement[] placements = new Marked_Placement[0];
                foreach (var p in free_pos)
                {
                    foreach (Rotation r in new[] { Rotation.Degree_0, Rotation.Degree_90, Rotation.Degree_180, Rotation.Degree_270 })
                    {
                        Connection_Chip chip = Chip_Constructor.Get_Chip(type, p, r);
                        double[][] inputs = Map_To_Inputs(_map.With_Chip(chip));
                        double mark = _brain.FeedForward(inputs)[0];
                        placements = placements.Append(new Marked_Placement(chip, mark)).ToArray();
                    }
                }
                placements = placements.OrderBy(p => -p.Mark).ToArray();
                Connection_Chip placement_chip = placements.Select(p => p.Chip).ElementAt(0);
                placement_chips = placement_chips.Append(placement_chip).ToArray();
                free_pos = free_pos.Where(p => p != placement_chip.Position).ToArray();
                _map = new InputMap(_map, placement_chip);
            }
            Place_Chips(new Round_Result(this, placement_chips));
        }

        protected override void On_GameFinished(Game_Result result)
        {
            form.Set_Map(result.GameObjects);
            form.Show();
        }

        protected override void On_GameStarted(Map_Data data)
        {
            form.Init_Map(data);
            _map = new InputMap(data.Nodes, data.Obstructions.Where(o => o.X > 0 && o.X < Game_Controller.MAP_WIDTH - 1 && o.Y > 0 && o.Y < Game_Controller.MAP_HEIGHT - 1).ToArray());
        }
    }
}
