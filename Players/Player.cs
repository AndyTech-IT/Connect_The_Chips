using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Connect_The_Chips.Game;

namespace Connect_The_Chips.Players
{
    public abstract class Player
    {
        public bool In_Game => _subscribering_game is Game_Controller;

        private Action<Round_Result> _chips_placed;
        private Game_Controller _subscribering_game;
        private Chips_Pack _curent_pack;

        protected Player()
        {
            _subscribering_game = null;
        }

        protected abstract void On_GameStarted(Map_Data data);
        protected abstract void On_ChipsGiven(Chips_Pack pack);
        protected abstract void On_GameFinished(Game_Result result);

        public void Subscribe(Game_Controller game)
        {
            if (_subscribering_game is Game_Controller)
                Unsubscribe();

            game.Game_Started += On_GameStarted;
            game.Chips_Given += Get_NewPack;
            game.Chips_Given += On_ChipsGiven;
            game.Game_Finished += On_GameFinished;

            _chips_placed += game.On_ChipsPlaced;

            _subscribering_game = game;
        }

        public void Unsubscribe()
        {
            if (_subscribering_game is null)
                throw new Exception("Not in game yet!");

            _subscribering_game.Game_Started -= On_GameStarted;
            _subscribering_game.Chips_Given -= Get_NewPack;
            _subscribering_game.Chips_Given -= On_ChipsGiven;
            _subscribering_game.Game_Finished -= On_GameFinished;

            _chips_placed -= _subscribering_game.On_ChipsPlaced;

            _subscribering_game = null;
        }

        protected void Place_Chips(Round_Result result)
        {
            Point[] placement_points = new Point[Game_Controller.CHIPS_PACK_SIZE];
            for (int i =0; i < Game_Controller.CHIPS_PACK_SIZE; i++)
            {
                Point chip_pos = result.Positions[i];
                if (placement_points.Contains(chip_pos))
                    throw new Exception($"Chip {i+1} owerplacing other round chip!");
                placement_points[i] = chip_pos;

                if (_curent_pack.Empty_Positions.Contains(chip_pos) == false)
                    throw new Exception($"Chip {i+1} cant place in {chip_pos}!");
            }
            _chips_placed?.Invoke(result);
        }

        private void Get_NewPack(Chips_Pack pack)
        {
            _curent_pack = pack;
        }
    }
}
