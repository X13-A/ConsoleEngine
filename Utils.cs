using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console3DRenderer
{
    public class Utils
    {
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
