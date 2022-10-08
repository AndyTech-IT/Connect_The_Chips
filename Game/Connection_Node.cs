using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connect_The_Chips.Game.Chips;

namespace Connect_The_Chips.Game
{
    public class Connection_Node: GameObject
    {
        public Direction Connection
        {
            get
            {
                switch (Rotation)
                {
                    case Rotation.Degree_0:
                        return Direction.Left;
                    case Rotation.Degree_90:
                        return Direction.Top;
                    case Rotation.Degree_180:
                        return Direction.Right;
                    case Rotation.Degree_270:
                        return Direction.Bottom;
                    default:
                        throw new Exception($"Wrong node rotation {Rotation}!");
                }
            }
        }
    }
}
