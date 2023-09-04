using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render.Structs
{
    public struct Triangle2DInt
    {
        public Vector2Int a;
        public Vector2Int b;
        public Vector2Int c;

        public List<Vector2Int> points => new List<Vector2Int> { a, b, c };
    }
}
