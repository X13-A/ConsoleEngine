using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public class Camera
    {
        public Point3D Position { get; set; } = new Point3D(0, 0, 0);
        public float RotationY { get; set; } = 0;

        public Point3D Transform(Point3D point)
        {
            // Rotate around Y axis
            float cosY = MathF.Cos(RotationY);
            float sinY = MathF.Sin(RotationY);
            float x = cosY * point.X + sinY * point.Z;
            float z = -sinY * point.X + cosY * point.Z;

            // Apply camera translation
            z += Position.Z;

            return new Point3D(x, point.Y, z);
        }

        public override string ToString()
        {
            return $"Pos: {Position.X}, {Position.Y}, {Position.Z} RotY: {RotationY}";
        }
    }

}
