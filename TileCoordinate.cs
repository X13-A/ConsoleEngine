using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Console3DRenderer
{
    public class TileCoordinate
    {
        public int x;
        public int y;

        public TileCoordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator == (TileCoordinate left, TileCoordinate right)
        {

            return left.x == right.x && left.y == right.y;
        }

        public static bool operator != (TileCoordinate left, TileCoordinate right)
        {
            return !(left.x == right.x && left.y == right.y);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == null) return false;
            if (obj is not TileCoordinate) return false;

            TileCoordinate other = obj as TileCoordinate;
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{x}, {y}";
        }
    }
}
