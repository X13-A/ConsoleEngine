using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public struct Triangle2D
    {
        public Vector2 a;
        public Vector2 b;
        public Vector2 c;

        public List<Vector2> points => new List<Vector2> { a, b, c };

        public Triangle2D(Vector2 a, Vector2 b, Vector2 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public bool ContainsPoint(Vector2 p, out Vector3 barycentricWeights)
        {
            // Calculate barycentric coordinates
            float denom = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
            float wA = ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / denom;
            float wB = ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / denom;
            float wC = 1 - wA - wB;

            barycentricWeights = new Vector3(wA, wB, wC);
            // If they are between 0 and 1 then we good
            return wA >= 0 && wA <= 1 && wB >= 0 && wB <= 1 && wC >= 0 && wC <= 1;
        }
    }
}
