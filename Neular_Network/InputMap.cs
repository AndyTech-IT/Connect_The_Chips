using Connect_The_Chips.Game;
using Connect_The_Chips.Game.Chips;
using Neural_Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Connect_The_Chips.Neular_Network
{
    public class InputMap
    {
        public static readonly Size[] Layer_Sizes = new Size[] { new Size(5, 4), new Size(5, 5), new Size(5, 5) };
        public readonly double[][][] Map;

        private const double _min_value = 0.001;
        private const double _max_value = 0.999;

        public double[][][] With_Chip(Connection_Chip chip)
        {
            double[][][] result = Map;
            result[2][chip.Y - 1][chip.X - 1] = ((int)chip.Rotation + 1) / ((int)Rotation.Degree_270 + 2);
            return result;
        }

        public InputMap(InputMap old_map, Connection_Chip new_chip)
        {
            Map = old_map.With_Chip(new_chip);
        }

        public InputMap(Connection_Node[] nodes, Obstruction[] obstructions, Connection_Chip[] chips = null)
        {
            Map = new double[Layer_Sizes.Length][][];
            for (int i = 0; i < Layer_Sizes.Length; i++)
            {
                Size size = Layer_Sizes[i];
                Map[i] = new double[size.Height][];
                for (int y = 0; y < size.Height; y++)
                {
                    Map[i][y] = new double[size.Width];
                    for (int x = 0; x < size.Width; x++)
                        Map[i][y][x] = _min_value;
                }
            }

            foreach(GameObject node in nodes)
            {
                Point pos = node.Position;
                if (node.Y == 0)
                {
                    pos.Y = 0;
                    pos.X--;
                }
                else if (node.X == Game_Controller.MAP_WIDTH - 1)
                {
                    pos.Y = 1;
                    pos.X = node.Y - 1;
                }
                else if (node.Y == Game_Controller.MAP_HEIGHT - 1)
                {
                    pos.Y = 2;
                    pos.X--;
                }
                else if (node.X == 0)
                {
                    pos.Y = 3;
                    pos.X = node.Y - 1;
                }

                Map[0][pos.Y][pos.X] = _max_value;
            }

            foreach (GameObject obstruction in obstructions)
                Map[1][obstruction.Y - 1][obstruction.X - 1] = _max_value;

            if (chips is null)
                return;

            foreach (GameObject chip in chips)
                Map[2][chip.Y - 1][chip.X - 1] = ((int)chip.Rotation + 1) / ((int)Rotation.Degree_270 + 2);
        }
    }
}
