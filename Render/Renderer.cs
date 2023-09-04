using ConsoleEngine.EventSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public class Triangle2D
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

        public bool ContainsPoint(Vector2 p)
        {
            // Calculate barycentric coordinates
            float denom = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
            float wA = ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / denom;
            float wB = ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / denom;
            float wC = 1 - wA - wB;

            // If they are between 0 and 1 then we good
            return wA >= 0 && wA <= 1 && wB >= 0 && wB <= 1 && wC >= 0 && wC <= 1;
        }
    }

    public class Triangle2DInt
    {
        public Vector2Int a;
        public Vector2Int b;
        public Vector2Int c;

        public List<Vector2Int> points => new List<Vector2Int> { a, b, c };
    }

    public class BoundingBox2D
    {
        public int minX;
        public int minY;
        public int maxX;
        public int maxY;

        public static BoundingBox2D FromTriangle2D(Triangle2D triangle)
        {
            BoundingBox2D box = new BoundingBox2D();
            List<float> xCoords = new List<float> { triangle.a.X, triangle.b.X, triangle.c.X };
            box.minX = (int) MathF.Round(xCoords.Min<float>());
            box.maxX = (int)MathF.Round(xCoords.Max<float>());

            List<float> yCoords = new List<float> { triangle.a.Y, triangle.b.Y, triangle.c.Y };
            box.minY = (int)MathF.Round(yCoords.Min<float>());
            box.maxY = (int)MathF.Round(yCoords.Max<float>());
            return box;
        }

        public static BoundingBox2D FromTriangle2D(Triangle2DInt triangle)
        {
            BoundingBox2D box = new BoundingBox2D();
            List<float> xCoords = new List<float> { triangle.a.X, triangle.b.X, triangle.c.X };
            box.minX = (int)MathF.Round(xCoords.Min<float>());
            box.maxX = (int)MathF.Round(xCoords.Max<float>());

            List<float> yCoords = new List<float> { triangle.a.Y, triangle.b.Y, triangle.c.Y };
            box.minY = (int)MathF.Round(yCoords.Min<float>());
            box.maxY = (int)MathF.Round(yCoords.Max<float>());
            return box;
        }
    }

    public class Renderer : Singleton<Renderer>
    {
        private RenderFrame frame;

        public Renderer(int frameWidth, int frameHeight)
        {
            frame = new RenderFrame(frameWidth, frameHeight);
            frame.Resize(Console.WindowWidth, Console.WindowHeight);
            frame.Clear();
        }

        public Renderer() : this(Console.WindowWidth, Console.WindowHeight)
        {}

        public bool IsPointInFrame(int x, int y)
        {
            bool inWidth = x >= 0 && x < frame.Width;
            bool inHeight = y >= 0 && y < frame.Height;

            return inWidth && inHeight;
        }

        public List<Vector2Int> GetPixelsInsideTriangle(Triangle2D triangle, Camera camera)
        {
            List<Vector2Int> pixels = new List<Vector2Int>();

            BoundingBox2D bb = BoundingBox2D.FromTriangle2D(triangle);
            for (int x = bb.minX; x < bb.maxX; x++)
            {
                for (int y = bb.minY; y < bb.maxY; y++)
                {
                    if (triangle.ContainsPoint(new Vector2(x, y)))
                    {
                        pixels.Add(new Vector2Int(x, y));
                    }
                }
            }

            return pixels;
        }

        public Triangle2D ProjectTriangle(Vector3 a, Vector3 b, Vector3 c, Camera camera)
        {
            Vector3 aTransformed = camera.TransformToCameraSpace(a);
            Vector2 aProjected = camera.ProjectPerspective(aTransformed);
            aProjected *= new Vector2(frame.Width, frame.Height);
            aProjected.X = (int)MathF.Round(aProjected.X);
            aProjected.Y = (int)MathF.Round(aProjected.Y);

            Vector3 bTransformed = camera.TransformToCameraSpace(b);
            Vector2 bProjected = camera.ProjectPerspective(bTransformed);
            bProjected *= new Vector2(frame.Width, frame.Height);
            bProjected.X = (int)MathF.Round(bProjected.X);
            bProjected.Y = (int)MathF.Round(bProjected.Y);

            Vector3 cTransformed = camera.TransformToCameraSpace(c);
            Vector2 cProjected = camera.ProjectPerspective(cTransformed);
            cProjected *= new Vector2(frame.Width, frame.Height);
            cProjected.X = (int)MathF.Round(cProjected.X);
            cProjected.Y = (int)MathF.Round(cProjected.Y);

            return new Triangle2D(aProjected, bProjected, cProjected);
        }

        public void Render(Shape shape, Camera camera)
        {
            if (shape.indices.Length % 3 != 0)
            {
                EventManager.Instance.Raise(new RenderErrorEvent { message = "Error: Invalid index buffer, length needs to be multiple of 3" });
                return;
            }

            for (int i = 0; i < shape.indices.Length / 3; i++)
            {
                Vector3 a = shape.vertices[shape.indices[i*3]];
                Vector3 b = shape.vertices[shape.indices[i*3+1]];
                Vector3 c = shape.vertices[shape.indices[i*3+2]];

                Triangle2D projectedTriangle = ProjectTriangle(a, b, c, camera);
                List<Vector2Int> pixels = GetPixelsInsideTriangle(projectedTriangle, camera);

                for (int j = 0; j < pixels.Count; j++)
                {
                    if (!IsPointInFrame(pixels[j].X, pixels[j].Y)) continue;
                    //float depth = Vector3.Distance(camera.Position.
                    frame.SetPixel(pixels[j].X, pixels[j].Y, new ConsolePixel(shape.colors[i], shape.colors[i], ' ', 0));
                }
            }
        }

        public Vector2 Project(Vector3 point3D)
        {
            float scale = 10 / point3D.Z;
            float x = point3D.X * scale + frame.Width / 2;
            float y = -point3D.Y * scale + frame.Height / 2;

            return new Vector2(x, y);
        }

        public void Draw()
        {
            ConsoleDisplay2.Instance.DrawFrame(frame);
            frame.Resize(Console.WindowWidth, Console.WindowHeight);
            frame.Clear();
        }
    }
}
