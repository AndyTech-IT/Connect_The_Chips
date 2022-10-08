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
            Game_Controller game = new Game_Controller();
            Human_Player player = new Human_Player();
            game.Start_Game(player);
        }

        private void Game_Form_Load(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
