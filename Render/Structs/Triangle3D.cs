using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public struct Triangle3D
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Vector3? normal;

        public List<Vector3> points => new List<Vector3> { a, b, c };

        public Triangle3D(Vector3 a, Vector3 b, Vector3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            normal = null;
        }

        public void SetNormal()
        {
            Vector3 edge1 = b - a;
            Vector3 edge2 = c - a;
            normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));
        }
    }
}
