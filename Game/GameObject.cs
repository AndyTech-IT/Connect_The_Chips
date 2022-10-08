using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game
{
    public class GameObject
    {
        public Point Position;
        public int X => Position.X;
        public int Y => Position.Y;

        public Rotation Rotation;
    }
}
