using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using ConsoleEngine.EventSystem;
using ConsoleEngine.Menus;
using ConsoleEngine.Render;
using ConsoleEngine.Importing;

namespace ConsoleEngine
{
    public class App : Singleton<App>
    {
        private bool done;

        public void Start()
        {
            SetupSingletons();
            MenuStorage.Populate();
            EventManager.Instance.Raise(new OpenMenuEvent { menu = MenuStorage.Get(MenuID.MainMenu)});
            SubscribeEvents();
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
        }

        private void StartLoop()
        {
            done = false;
            while (!done)
            {
                Update();
                Draw();
                PerformanceInfo.Instance.Update();
            }
        }

        private void SetupSingletons()
        {
            new EventManager();
            new InputManager();
            new MenuManager();
            new ConsoleDisplay();
            new Renderer();
            new Utils();
            new PerformanceInfo();
        }

        private void Update()
        {
            InputManager.Instance.UpdateKeyStates();
            Scene3D.Instance?.Update();
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
            Console.Title = $"{PerformanceInfo.Instance.averageFPS} FPS";

            ConsoleDisplay.Instance.Refresh_GreyScale();
        }
    }
}
