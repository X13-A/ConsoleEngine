using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Timers;

namespace ConsoleEngine.Render
{
    public class ConsoleDisplay2 : Singleton<ConsoleDisplay2>
    {
        private RenderFrame oldFrame;
        private RenderFrame frame;
        private Vector2Int cursor = new Vector2Int(0, 0);
        private bool isCursorInFrame => cursor.X < frame.Width && cursor.Y < frame.Height;

        public void Init()
        {
            Console.CursorVisible = false;
            frame = new RenderFrame(Console.WindowWidth, Console.WindowHeight);
            frame.Clear();
            oldFrame = new RenderFrame(frame);
        }

        public void DrawPixel(ConsolePixel pixel)
        {
            if (!isCursorInFrame) return;

            frame.SetPixel(cursor.X, cursor.Y, pixel);
            cursor.X++;
        }

        public void DrawLine(List<ConsolePixel> pixels)
        {
            for (int i = 0; i < pixels.Count; i++)
            {
                DrawPixel(pixels[i]);
            }
            cursor.Y++;
            cursor.X = 0;
        }

        public void DrawLine(string line) => DrawLine(line, ConsoleColor.White, ConsoleColor.Black);

        public void DrawLine(string line, ConsoleColor color, ConsoleColor backgroundColor)
        {
            for (int i = 0; i < line.Length; i++)
            {
                DrawPixel(new ConsolePixel(color, backgroundColor, line[i], 0));
            }
            cursor.Y++;
            cursor.X = 0;
        }

        public void DrawFrame(RenderFrame frame)
        {
            for (int y = 0; y < frame.Height; y++)
            {
                for (int x = 0; x < frame.Width; x++)
                {
                    DrawPixel(frame.GetPixel(x, y));
                }
                cursor.Y++;
                cursor.X = 0;
            }
        }

        public void Refresh()
        {
            cursor.X = 0; cursor.Y = 0;
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < frame.Height; y++)
            {
                for (int x = 0; x < frame.Width; x++)
                {
                    ConsolePixel newPixel = frame.GetPixel(x, y);
                    ConsolePixel oldPixel = oldFrame.GetPixel(x, y);

                    if (!newPixel.Equals(oldPixel))
                    {
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = newPixel.color;
                        Console.BackgroundColor = newPixel.backgroundColor;
                        Console.Write(newPixel.symbol);
                        Console.ResetColor();
                    }
                }
            }

            oldFrame = new RenderFrame(frame);

            // TODO: Trigger resize only when Console size changes
            frame.Resize(Console.WindowWidth, Console.WindowHeight);
            frame.Clear();
        }
    }
}
