using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ConsoleEngine.Render
{
    public class Camera
    {
        public Vector3 Position = new Vector3(0, 0, 0);
        public float RotationY { get; set; } = 0;

        public float Fov = 90;

        public Vector3 TransformToCameraSpace(Vector3 point)
        {
            // Translation
            float xT = point.X - Position.X;
            float yT = point.Y + Position.Y;
            float zT = point.Z - Position.Z;

            // Rotation about Y-axis
            float xR = xT * MathF.Cos(RotationY) + zT * MathF.Sin(RotationY);
            float yR = yT;
            float zR = -xT * MathF.Sin(RotationY) + zT * MathF.Cos(RotationY);

            return new Vector3(xR, yR, zR);
        }

        public Vector2 ProjectPerspective(Vector3 point)
        {
            float f = 1.0f / MathF.Tan(Fov / 2.0f);

            float xP = point.X * f / point.Z;
            float yP = point.Y * f / point.Z;

            return new Vector2(xP, yP);
        }

        public bool IsVisible(Triangle3D triangle)
        {
            if (triangle.normal == null)
            {
                triangle.SetNormal();
            }
            Vector3 viewVector = Vector3.Normalize(Position - triangle.a);
            return Vector3.Dot(triangle.normal.Value, viewVector) >= 0;
        }

        public override string ToString()
        {
            return $"Pos: {Position.X}, {Position.Y}, {Position.Z} RotY: {RotationY}";
        }
    }

}
