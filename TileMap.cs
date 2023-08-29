using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console3DRenderer
{
    public class TileMap
    {
        private int width;
        private int height;
        private IDictionary<string, Tile> map;

        public TileMap(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new Dictionary<string, Tile>();

            for (int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    SetTile(new TileCoordinate(x, y), new Tile('.'));
                }
            }
        }

        public TileMap(string mapString)
        {
            map = new Dictionary<string, Tile>();
            ParseMap(mapString);
        }

        public TileMap(string[] mapStrings)
        {
            map = new Dictionary<string, Tile>();
            ParseMap(mapStrings);
        }

        public Tile GetTile(TileCoordinate coord)
        {
            return map[coord.ToString()];
        }

        public void SetTile(TileCoordinate coord, Tile tile)
        {
            map[coord.ToString()] = tile;
        }

        public void ParseMap(string mapString)
        {
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;

            for (int i = 0; i < mapString.Length; i++)
            {
                SetTile(new TileCoordinate(x, y), new Tile(mapString[i]));
                if (mapString[i] == '\n')
                {
                    if (width != 0 && width != x+1)
                    {
                        throw (new Exception("Error parsing TileMap: Invalid size"));
                    }
                    width = x+1;
                    x = 0;

                    height++;
                    y++;
                }
                x++;
            }

            this.width = width;
            this.height = height;
        }

        public void ParseMap(string[] mapString)
        {
            if (mapString.Length == 0)
            {
                throw (new Exception("Error parsing TileMap: Invalid size"));
            }
            if (mapString[0].Length == 0)
            {
                throw (new Exception("Error parsing TileMap: Invalid size"));
            }

            int width = mapString[0].Length;
            int height = mapString.Length;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    SetTile(new TileCoordinate(x, y), new Tile(mapString[y][x]));
                }
            }

            this.width = width;
            this.height = height;
        }

        public List<string> AsStringList()
        {
            List<string> lines = new List<string>();
            for (int y = 0; y < height; y++)
            {
                string line = "";
                for (int x = 0; x < width; x++)
                {
                    line += map[new TileCoordinate(x, y).ToString()].symbol;
                }
                lines.Add(line);
            }
            return lines;
        }

        public override string ToString()
        {
            string res = "";
            for (int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    res += map[new TileCoordinate(x, y).ToString()].symbol;
                }
                if (y != height - 1)
                {
                    res += '\n';
                }
            }
            return res;
        }
    }
}
