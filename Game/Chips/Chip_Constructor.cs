using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_The_Chips.Game.Chips
{
    public static class Chip_Constructor
    {
        public static Connection_Chip Get_Chip(Chips_Type type, Point point, Rotation rotation)
        {
            switch (type)
            {
                case Chips_Type.L_Chip:
                    return new L_Chip() { Position = point, Rotation = rotation };
                case Chips_Type.T_Chip:
                    return new T_Chip() { Position = point, Rotation = rotation };
                case Chips_Type.I_Chip:
                    return new I_Chip() { Position = point, Rotation = rotation };
                default:
                    throw new ArgumentException(nameof(type));
            }
        }
    }
}
