using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render.Structs
{
    public struct BoundingBox2D
    {
        public int minX;
        public int minY;
        public int maxX;
        public int maxY;

        public static BoundingBox2D FromTriangle2D(Triangle2D triangle)
        {
            BoundingBox2D box = new BoundingBox2D();
            List<float> xCoords = new List<float> { triangle.a.X, triangle.b.X, triangle.c.X };
            box.minX = (int)MathF.Floor(xCoords.Min<float>());
            box.maxX = (int)MathF.Ceiling(xCoords.Max<float>());

            List<float> yCoords = new List<float> { triangle.a.Y, triangle.b.Y, triangle.c.Y };
            box.minY = (int)MathF.Floor(yCoords.Min<float>());
            box.maxY = (int)MathF.Ceiling(yCoords.Max<float>());
            return box;
        }

        public static BoundingBox2D FromTriangle2D(Triangle2DInt triangle)
        {
            BoundingBox2D box = new BoundingBox2D();
            List<float> xCoords = new List<float> { triangle.a.X, triangle.b.X, triangle.c.X };
            box.minX = (int)MathF.Floor(xCoords.Min<float>());
            box.maxX = (int)MathF.Ceiling(xCoords.Max<float>());

            List<float> yCoords = new List<float> { triangle.a.Y, triangle.b.Y, triangle.c.Y };
            box.minY = (int)MathF.Floor(yCoords.Min<float>());
            box.maxY = (int)MathF.Ceiling(yCoords.Max<float>());
            return box;
        }
    }
}
