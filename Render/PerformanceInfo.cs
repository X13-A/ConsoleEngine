using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEngine.Render
{
    public class PerformanceInfo : Singleton<PerformanceInfo>
    {
        // FPS
        private Stopwatch time = new Stopwatch();
        private TimeSpan lastFrameTime;
        private TimeSpan frameTime;
        public TimeSpan FrameTime => frameTime;
        public double DeltaTime => frameTime.TotalMilliseconds / 1000d;

        private double fps;
        public double FPS => fps;

        // AVG FPS
        private Stopwatch avgWatch = new Stopwatch();
        private ulong frames;
        public double averageFPS;

        protected override void Init()
        {
            time.Start();
            avgWatch.Start();
            SetFPS();
        }

        private void SetFPS()
        {
            frameTime = time.Elapsed - lastFrameTime;
            lastFrameTime = time.Elapsed;
            fps = 1000f / frameTime.TotalMilliseconds;

            frames++;
            if (avgWatch.Elapsed.TotalMilliseconds > 1000)
            {
                averageFPS = frames;
                frames = 0;
                avgWatch.Restart();
            }
        }

        public void Update()
        {
            SetFPS();
        }
    }
}
