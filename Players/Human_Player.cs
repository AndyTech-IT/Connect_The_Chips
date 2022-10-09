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
            _myform = new Human_Form(this);
            _myform.Show();
        }

        public bool Try_Play(Round_Result result)
        {
            try
            {
                Place_Chips(result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void On_ChipsGiven(Chips_Pack pack)
        {
            _myform.Get_Pack(pack);
        }

        protected override void On_GameFinished(Game_Result result)
        {
            _myform.Get_Result(result);
        }

        protected override void On_GameStarted(Map_Data data)
        {
            _myform.Init_Map(data);
        }
    }
}
