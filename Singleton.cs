using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine
{
    public class Singleton<T> where T:class
    {
        private static T instance;
        public static T Instance => instance;

        public Singleton()
        {
            if (instance != null) return;
            instance = this as T;
        }

        public Singleton(bool replace)
        {
            if (replace || instance == null)
            {
                instance = this as T;
                return;
            }
        }
    }
}
