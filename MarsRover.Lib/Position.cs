﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarsRover.Lib
{
    public interface IPosition
    {
        int X { get; set; }
        int Y { get; set; }
        string ToString();
    }

    public class Position : IPosition
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", X, Y);
        }
    }

    
}
