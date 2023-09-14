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
        private string symbols = " .-~:;=+*%#@";

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
            int index = (int) Math.Round(intensity * symbols.Length);
            if (index < 0) return symbols[0];
            if (index >= symbols.Length) return symbols[symbols.Length-1];
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
            for (int x = (int) MathF.Max(bb.minX, 0); x < (int) MathF.Min(bb.maxX, ConsoleDisplay.Instance.consoleWidth); x++)
            {
                for (int y = (int)MathF.Max(bb.minY, 0); y < (int)MathF.Min(bb.maxY, ConsoleDisplay.Instance.consoleHeight); y++)
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

        public Triangle2D? ProjectTriangle(Triangle3D triangle, Camera camera)
        {
            Vector3 aTransformed = camera.TransformToCameraSpace(triangle.a);
            Vector3 bTransformed = camera.TransformToCameraSpace(triangle.b);
            Vector3 cTransformed = camera.TransformToCameraSpace(triangle.c);

            // If any vertex of the triangle is behind the camera in camera space, return null
            if (aTransformed.Z < 0 || bTransformed.Z < 0 || cTransformed.Z < 0) return null;

            Vector2 aProjected = camera.ProjectPerspective(aTransformed);
            aProjected *= new Vector2(frame.Width, frame.Height);
            aProjected.X = (int)MathF.Round(aProjected.X);
            aProjected.Y = (int)MathF.Round(aProjected.Y);

            Vector2 bProjected = camera.ProjectPerspective(bTransformed);
            bProjected *= new Vector2(frame.Width, frame.Height);
            bProjected.X = (int)MathF.Round(bProjected.X);
            bProjected.Y = (int)MathF.Round(bProjected.Y);

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
                Triangle3D triangle = new Triangle3D
                (
                    shape.vertices[shape.indices[i * 3]],
                    shape.vertices[shape.indices[i * 3 + 1]],
                    shape.vertices[shape.indices[i * 3 + 2]]
                );
                triangle.SetNormal();
                // TODO: Fix backface culling
                //if (!camera.IsVisible(triangle)) continue;

                Triangle2D? projectedTriangle = ProjectTriangle(triangle, camera);
                if (projectedTriangle == null) continue;
                List<TrianglePixelData> pixels = GetPixelsInsideTriangle(projectedTriangle.Value);

                for (int j = 0; j < pixels.Count; j++)
                {
                    if (!IsPointInFrame(pixels[j].coordinates.X, pixels[j].coordinates.Y)) continue;

                    float depth = pixels[j].barycentricWeight.X * triangle.a.Z + pixels[j].barycentricWeight.Y * triangle.b.Z + pixels[j].barycentricWeight.Z * triangle.c.Z;
                    if (depth < frame.GetPixel(pixels[j].coordinates.X, pixels[j].coordinates.Y).depth)
                    {
                        ConsoleColor color = (shape.colors.Length > i ? shape.colors[i] : ConsoleColor.Gray);
                        float intensityX = (1 + MathF.Sin(triangle.normal.Value.X)) / 2;
                        frame.SetPixel(pixels[j].coordinates.X, pixels[j].coordinates.Y, new ConsolePixel(color, ConsoleColor.Black, SymbolFromIntensity(intensityX), depth));
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
            ConsoleDisplay.Instance.DrawFrame(frame);
            frame.Resize(Console.WindowWidth, Console.WindowHeight);
            frame.Clear();
        }
    }
}