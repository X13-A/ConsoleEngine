﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public class ConsolePixel
    {
        public ConsoleColor color = ConsoleColor.Black;
        public ConsoleColor backgroundColor = ConsoleColor.Black;
        public char symbol = ' ';
        public float depth = 0;

        public ConsolePixel(ConsolePixel pixel)
        {
            if (pixel == null) return;
            color = pixel.color;
            backgroundColor = pixel.backgroundColor;
            symbol = pixel.symbol;
            depth = pixel.depth;
        }

        public ConsolePixel(ConsoleColor color, ConsoleColor backgroundColor, char symbol, float depth)
        {
            this.color = color;
            this.backgroundColor = backgroundColor;
            this.symbol = symbol;
            this.depth = depth;
        }

        public bool HasSameColor(ConsolePixel other)
        {
            if (other.color != color) return false;
            if (other.backgroundColor != backgroundColor) return false;
            return true;
        }

        public bool HasSameDisplay(ConsolePixel other)
        {
            return HasSameColor(other) && other.symbol == symbol;
        }
    }
}
