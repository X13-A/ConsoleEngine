using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render.Structs
{
    public struct TrianglePixelData
    {
        public Vector2Int coordinates;
        public Vector3 barycentricWeight;

        public TrianglePixelData(Vector2Int coordinates, Vector3 barycentricWeight)
        {
            this.coordinates = coordinates;
            this.barycentricWeight = barycentricWeight;
        }
    }
}
