using ConsoleEngine.EventSystem;
using ConsoleEngine.Render.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public class Renderer : Singleton<Renderer>
    {
        private RenderFrame frame;
        private string symbols = " .:-=+*%@#";

        public Renderer(int frameWidth, int frameHeight)
        {
            frame = new RenderFrame(frameWidth, frameHeight);
            frame.Resize(Console.WindowWidth, Console.WindowHeight);
            frame.Clear();
        }

        public Renderer() : this(Console.WindowWidth, Console.WindowHeight)
        { }

        /// <param name="intensity">Must range from 0 to 1</param>
        /// <returns></returns>
        public char SymbolFromIntensity(float intensity)
        {
            int index = (int) (symbols.Length * intensity);
            if (index < 0) return 'x';
            if (index > symbols.Length) return 'x';
            return symbols[index];
        }

        public bool IsPointInFrame(int x, int y)
        {
            bool inWidth = x >= 0 && x < frame.Width;
            bool inHeight = y >= 0 && y < frame.Height;

            return inWidth && inHeight;
        }

        public List<TrianglePixelData> GetPixelsInsideTriangle(Triangle2D triangle)
        {
            List<TrianglePixelData> pixels = new List<TrianglePixelData>();

            BoundingBox2D bb = BoundingBox2D.FromTriangle2D(triangle);
            for (int x = bb.minX; x < bb.maxX; x++)
            {
                for (int y = bb.minY; y < bb.maxY; y++)
                {
                    Vector3 barycentricWeights;
                    if (triangle.ContainsPoint(new Vector2(x, y), out barycentricWeights))
                    {
                        pixels.Add(new TrianglePixelData(new Vector2Int(x, y), barycentricWeights));
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
                Vector3 a = shape.vertices[shape.indices[i * 3]];
                Vector3 b = shape.vertices[shape.indices[i * 3 + 1]];
                Vector3 c = shape.vertices[shape.indices[i * 3 + 2]];

                Triangle2D projectedTriangle = ProjectTriangle(a, b, c, camera);
                List<TrianglePixelData> pixels = GetPixelsInsideTriangle(projectedTriangle);

                for (int j = 0; j < pixels.Count; j++)
                {
                    if (!IsPointInFrame(pixels[j].coordinates.X, pixels[j].coordinates.Y)) continue;

                    float depth = pixels[j].barycentricWeight.X * a.Z + pixels[j].barycentricWeight.Y * b.Z + pixels[j].barycentricWeight.Z * c.Z;
                    if (depth < frame.GetPixel(pixels[j].coordinates.X, pixels[j].coordinates.Y).depth)
                    {
                        ConsoleColor color = (shape.colors.Length > i ? shape.colors[i] : ConsoleColor.Gray);
                        frame.SetPixel(pixels[j].coordinates.X, pixels[j].coordinates.Y, new ConsolePixel(ConsoleColor.Black, color, ' ', depth));
                    }
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