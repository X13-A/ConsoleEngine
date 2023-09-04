using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public class Shape
    {
        public Vector3[] vertices = { };
        public int[] indices = { };
        public ConsoleColor[] colors = { };
        
        public Shape() {}

        public Shape(Shape shape)
        {
            vertices = new Vector3[shape.vertices.Length];
            indices = new int[shape.indices.Length];
            colors = new ConsoleColor[shape.colors.Length];
            shape.vertices.CopyTo(vertices, 0);
            shape.indices.CopyTo(indices, 0);
            shape.colors.CopyTo(colors, 0);
        }

        public void Translate(Vector3 offset)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += offset;
            }
        }

        public void Rotate(Vector3 point, float angleX, float angleY, float angleZ)
        {
            Translate(-point);

            for (int i = 0; i < vertices.Length; i++)
            {
                // Rotate around the X-axis
                float yRotatedX = vertices[i].Y * MathF.Cos(angleX) - vertices[i].Z * MathF.Sin(angleX);
                float zRotatedX = vertices[i].Y * MathF.Sin(angleX) + vertices[i].Z * MathF.Cos(angleX);

                // Rotate around the Y-axis
                float xRotatedY = vertices[i].X * MathF.Cos(angleY) + zRotatedX * MathF.Sin(angleY);
                float zRotatedY = -vertices[i].X * MathF.Sin(angleY) + zRotatedX * MathF.Cos(angleY);

                // Rotate around the Z-axis
                float xRotatedZ = xRotatedY * MathF.Cos(angleZ) - yRotatedX * MathF.Sin(angleZ);
                float yRotatedZ = xRotatedY * MathF.Sin(angleZ) + yRotatedX * MathF.Cos(angleZ);

                vertices[i] = new Vector3(xRotatedZ, yRotatedZ, zRotatedY);
            }

            Translate(point);
        }
    }
}
