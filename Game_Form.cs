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
            // Тренеровка нейросетей
            //new AITrain_Form().ShowDialog();
            // Демонстрация работы нейросетей
            //new Game_Controller().Start_Game(new AI_Player());

            // Запуск игры для игрока
            new Game_Controller().Start_Game(new Human_Player());
        }

        private void Game_Form_Load(object sender, EventArgs e)
        {
            Hide();
        }

        private void Game_Form_Shown(object sender, EventArgs e)
        {
            //Close();
        }
    }
}
