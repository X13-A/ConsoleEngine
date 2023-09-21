using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public struct Shape
    {
        public Vector3[] vertices = { };
        public Vector3[] normals = { };
        public int[] indices = { };
        public Vector3 origin = Vector3.Zero;

        public Shape() {}

        public Shape(Shape shape)
        {
            vertices = new Vector3[shape.vertices.Length];
            indices = new int[shape.indices.Length];
            normals = new Vector3[shape.indices.Length/3];
            shape.vertices.CopyTo(vertices, 0);
            shape.indices.CopyTo(indices, 0);
            shape.normals.CopyTo(normals, 0);
        }

        public void Translate(Vector3 offset)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += offset;
            }
        }

        public void Rotate(float angleX, float angleY, float angleZ, Vector3 point)
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

        public void Scale(Vector3 scaleFactors, Vector3? origin = null)
        {
            Vector3 scaleOrigin = origin ?? Vector3.Zero;

            Translate(-scaleOrigin);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices[i].X * scaleFactors.X,
                                          vertices[i].Y * scaleFactors.Y,
                                          vertices[i].Z * scaleFactors.Z);
            }

            Translate(scaleOrigin);
        }
    }
}
