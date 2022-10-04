using Connect_The_Chips.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect_The_Chips.Players
{
    public class Human_Player: Player
    {
        private Human_Form _myform;

        public Human_Player()
        {
            _myform = new Human_Form();
        }

        protected override void On_ChipsGiven(Chips_Pack pack)
        {
            _myform.Get_Pack(pack);
            while (_myform.ShowDialog() != DialogResult.OK)
            {
                if (_myform.DialogResult == DialogResult.Abort)
                    return;
            }
            Place_Chips(_myform.Result);
        }

        protected override void On_GameFinished(Game_Result result)
        {
            throw new NotImplementedException();
        }

        protected override void On_GameStarted(Map_Data data)
        {

        }
    }
}
