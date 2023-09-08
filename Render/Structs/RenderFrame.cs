using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public struct RenderFrame
    {
        public int Width;
        public int Height;
        private ConsolePixel[,] Display;


        public RenderFrame(int width, int height)
        {
            Width = width;
            Height = height;
            Display = new ConsolePixel[width, height];
        }

        public RenderFrame(RenderFrame frame)
        {
            Width = frame.Width;
            Height = frame.Height;
            Display = new ConsolePixel[Width, Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Display[x, y] = new ConsolePixel(frame.Display[x, y]);
                }
            }
        }

        public void Clear()
        {
            for (int x = 0; x < Display.GetLength(0); x++)
            {
                for (int y = 0; y < Display.GetLength(1); y++)
                {
                    Display[x, y] = new ConsolePixel(ConsoleColor.Black, ConsoleColor.Black, ' ', float.PositiveInfinity);
                }
            }
        }

        public void SetPixel(int x, int y, ConsolePixel pixel)
        {
            Display[x, y] = pixel;
        }

        public ConsolePixel GetPixel(int x, int y)
        {
            return Display[x, y];
        }

        public ConsolePixel GetPixel(Vector2Int pos)
        {
            return Display[pos.X, pos.Y];
        }

        /// <summary>
        /// <para>Resizes the frame.</para>
        /// <para>RenderFrame.Clear() needs to be called afterwards</para>
        /// </summary>
        public void Resize(int width, int height)
        {
            Display = new ConsolePixel[width, height];
            Width = width;
            Height = height;
        }
    }
}
