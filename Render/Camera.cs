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
        public Vector3 Rotation = new Vector3(0, 0, 0);

        public float Fov = 90;

        public Vector3 TransformToCameraSpace(Vector3 point)
        {
            // Translation
            float xT = point.X + Position.X;
            float yT = point.Y - Position.Y;
            float zT = point.Z - Position.Z;

            // Rotation about X-axis
            float yRx = yT * MathF.Cos(Rotation.X) - zT * MathF.Sin(Rotation.X);
            float zRx = yT * MathF.Sin(Rotation.X) + zT * MathF.Cos(Rotation.X);

            // Rotation about Y-axis
            float xRy = xT * MathF.Cos(Rotation.Y) + zRx * MathF.Sin(Rotation.Y);
            float zRy = -xT * MathF.Sin(Rotation.Y) + zRx * MathF.Cos(Rotation.Y);

            // Rotation about Z-axis
            float xRz = xRy * MathF.Cos(Rotation.Z) - yRx * MathF.Sin(Rotation.Z);
            float yRz = xRy * MathF.Sin(Rotation.Z) + yRx * MathF.Cos(Rotation.Z);

            return new Vector3(xRz, yRz, zRy);
        }

        public Vector2 ProjectPerspective(Vector3 point)
        {
            float f = 1.0f / MathF.Tan(Fov / 2.0f);

            float xP = -point.X * f / point.Z;
            float yP = -point.Y * f / point.Z;

            // We add 0.5 so that the origin is the center of the screen
            return new Vector2(xP + 0.5f, yP + 0.5f);
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
            return $"Pos: {Position.X}, {Position.Y}, {Position.Z} Rot: {Rotation.X}, {Rotation.Y}, {Rotation.Z}";
        }

        public void Move(Vector3 distance)
        {
            Vector3 forward = new Vector3(MathF.Sin(Rotation.Y), 0, MathF.Cos(Rotation.Y));
            Vector3 right = Vector3.Normalize(Vector3.Cross(forward, new Vector3(0, 1, 0)));
            Vector3 up = Vector3.Normalize(Vector3.Cross(right, forward));

            // Update position
            Position += forward * distance.Z;
            Position += -right * distance.X;
            Position.Y += distance.Y;
        }
    }

}
