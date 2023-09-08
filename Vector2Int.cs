using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public class Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2Int(Vector2Int other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }

        public bool Equals(Vector2Int other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}
