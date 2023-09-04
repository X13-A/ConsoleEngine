using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public class Utils : Singleton<Utils>
    {
        private Random rand = new Random();

        public float RandomFloat(float min, float max)
        {
            return min + (float)rand.NextDouble() * (max - min);
        }

        public static List<T> CloneList<T>(List<T> list)
        {
            List<T> res = new List<T>();
            foreach (T item in list)
            {
                res.Add(item);
            }
            return res;
        }
    }
}
