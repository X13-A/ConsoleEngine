using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using Console3DRenderer.EventSystem;
using Console3DRenderer.Menus;
using Console3DRenderer.Render;

namespace Console3DRenderer
{
    public class App : Singleton<App>
    {
        private bool done;
        private Stopwatch stopwatch = new Stopwatch();
        private TimeSpan lastFrameTime;
        private TimeSpan frameTime;
        public double FrameTime => frameTime.TotalMilliseconds;
        public double FPS => 1000f/frameTime.TotalMilliseconds;

        public void Start()
        {
            SetupSingletons();
            SubscribeEvents();
            stopwatch.Start();
            StartLoop();
        }

        private void SubscribeEvents()
        {
            EventManager.Instance.AddListener<ExitAppEvent>(Exit);
            EventManager.Instance.AddListener<StartGameEvent>(StartGame);
        }

        private void Exit(ExitAppEvent e)
        {
            done = true;
        }

        private void StartGame(StartGameEvent e)
        {
            EventManager.Instance.Raise(new CloseMenuEvent());

            new Scene3D();
            Scene3D.Instance.Init();

            //new Game();
            //Game.Instance.Init();
        }

        private void StartLoop()
        {
            done = false;
            while (!done)
            {
                Update();
                Draw();
            }
        }

        private void SetupSingletons()
        {
            new EventManager();
            new InputManager();
            new MenuManager();
            new ConsoleDisplay();
            new Renderer();
            MenuManager.Instance.Init();
            ConsoleDisplay.Instance.Init();
        }

        private void SetFPS()
        {
            frameTime = stopwatch.Elapsed - lastFrameTime;
            lastFrameTime = stopwatch.Elapsed;
        }

        private void Update()
        {
            InputManager.Instance.UpdateKeyStates();
            Scene3D.Instance?.Update();
            App.Instance.SetFPS();
        }

        private void Draw()
        {
            if (MenuManager.Instance.ActiveMenu != null)
            {
                MenuManager.Instance.ActiveMenu.Draw();
            }
            else
            {
                Scene3D.Instance?.Draw();
            }
            ConsoleDisplay.Instance.Refresh();
        }
    }
}
