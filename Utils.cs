using ConsoleEngine.EventSystem;
using System.Diagnostics;

namespace ConsoleEngine
{
    public class Utils : Singleton<Utils>
    {
        private Random rand = new Random();

        public static float? RandomFloat(float min, float max)
        {
            if (Instance == null)
            {
                EventManager.Instance.Raise(new ErrorEvent { message = "Error: You must instanciate the class Utils before using this method." });
                return null;
            }
            return min + (float) Instance.rand.NextDouble() * (max - min);
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

        public static void OpenFile(string path)
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = path
            };
            process.Start();
            process.WaitForExit();
        }
    }
}
