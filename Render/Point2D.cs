using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public class Point2D
    {
        public float X, Y;
        public Point2D Rounded => new Point2D(MathF.Round(X), MathF.Round(Y));

        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point2D other)
        {
            return X == other.X && Y == other.Y;
        }

        public Point2D Scale(float value)
        {
            return new Point2D(X * value, Y * value);
        }

        public Point2D Multiply(Point2D other)
        {
            return new Point2D(X * other.X, Y * other.Y);
        }

    }
}
