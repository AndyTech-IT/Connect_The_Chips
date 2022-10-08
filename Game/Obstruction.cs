using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game
{
    public class Obstruction: GameObject
    {
        public override string ToString()
        {
            return $"{GetType().Name} at {Position}";
        }
    }
}
