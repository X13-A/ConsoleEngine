﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace ConsoleEngine.Render
{
    public class ConsoleDisplayOLD : Singleton<ConsoleDisplayOLD>
    {
        private List<string> lines = new List<string>();
        private List<string> newLines = new List<string>();

        public void Init()
        {
            Console.CursorVisible = false;
        }

        public void Refresh()
        {
            // HACK: Use proper line count

            for (int i = newLines.Count; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            lines = Utils.CloneList(newLines);
            newLines.Clear();
            Console.SetCursorPosition(0, 0);
        }

        public void WriteLine(ConsolePixel[] pixels)
        {
        }

        public void Write(ConsolePixel pixel)
        {

        }

        public void WriteLine(string line) => WriteLine(line, ConsoleColor.White, ConsoleColor.Black);

        public void WriteLine(string line, ConsoleColor color, ConsoleColor backgroundColor)
        {
            int i = newLines.Count;
            if (i >= Console.WindowHeight - 1)
            {
                return;
            }

            int whitespaces = 0;
            if (i < lines.Count)
            {
                whitespaces = Math.Max(0, lines[i].Length - line.Length);
            }

            Console.ForegroundColor = color;
            Console.BackgroundColor = backgroundColor;
            Console.Write(line);
            Console.ResetColor();
            for (int j = 0; j < whitespaces; j++)
            {
                Console.Write(" ");
            }
            Console.WriteLine($"");
            newLines.Add(line);
        }
    }
}
