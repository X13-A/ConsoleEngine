using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public class Renderer : Singleton<Renderer>
    {
        private RenderBox frame;
        private float fov = 90;

        public Renderer(int frameWidth, int frameHeight)
        {
            frame = new RenderBox(frameWidth, frameHeight);
        }

        public Renderer() : this(Console.WindowWidth, Console.WindowHeight)
        {}

        public bool IsPointOnScreen(Point2D point)
        {
            bool inWidth = point.X >= 0 && point.X < frame.Width;
            bool inHeight = point.Y >= 0 && point.Y < frame.Height;

            return inWidth && inHeight;
        }

        public void Render(Shape shape, Camera camera)
        {
            // HACK
            // TODO: Make proper window system
            frame.Width = Console.WindowWidth;
            frame.Height = Console.WindowHeight;

            List<Point2D> screenPoints = new List<Point2D>();
            foreach (Point3D point in shape.vertices)
            {
                // Calc coordinates
                float yP = (camera.Position.Y + point.Y) / ((-camera.Position.Z + point.Z) * MathF.Tan(fov / 2f));
                float xP = (-camera.Position.X + point.X) / ((-camera.Position.Z + point.Z) * MathF.Tan(fov / 2f));
                
                Point2D p = new Point2D(xP + 0.5f, yP + 0.5f);
                Point2D pScaled = p.Multiply(new Point2D(frame.Width, frame.Height)).Rounded;

                //Point2D projected = Project(transformed);

                if (!IsPointOnScreen(pScaled)) continue;
                screenPoints.Add(pScaled);
            }

            for (int y = 0; y < frame.Height; y++)
            {
                string line = "";
                for (int x = 0; x < frame.Width; x++)
                {
                    string pixel = " ";
                    foreach (Point2D point in screenPoints)
                    {
                        if (point.Equals(new Point2D(x, y)))
                        {
                            pixel = "*";
                        }
                    }
                    line += pixel;
                }
                ConsoleDisplay.Instance.WriteLine(line);
            }
        }

        public Point2D Project(Point3D point3D)
        {
            float scale = 10 / point3D.Z;
            float x = point3D.X * scale + frame.Width / 2;
            float y = -point3D.Y * scale + frame.Height / 2;

            return new Point2D(x, y);
        }
    }
}
