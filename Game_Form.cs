using Connect_The_Chips.Game;
using Connect_The_Chips.Game.Chips;
using Connect_The_Chips.Players;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect_The_Chips
{
    public partial class Game_Form : Form
    {
        public Game_Form()
        {
            InitializeComponent();
            Human_Form form = new Human_Form();
            form.Init_Map(new Map_Data(new Connection_Node[] { new Connection_Node() }, new Obstruction[] { new Obstruction() }));
            form.Get_Pack(
                new Chips_Pack(new Connection_Chip[] {
                    new L_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0}, new L_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0}, new L_Chip{ Position = new Point(-1), Rotation = Rotation.Degree_0}
                },
                new Point[] {
                    new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(0, 3),
                    new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3),
                    new Point(2, 0), new Point(2, 1), new Point(2, 2), new Point(2, 3),
                    new Point(3, 0), new Point(3, 1), new Point(3, 2), new Point(3, 3),
                }));
            form.ShowDialog();
        }
    }
}
