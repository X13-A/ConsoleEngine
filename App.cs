using ConsoleEngine.EventSystem;
using ConsoleEngine.Menus;
using ConsoleEngine.Render;

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
            EventManager.Instance.AddListener<StartRendererEvent>(StartRenderer);
        }

        private void Exit(ExitAppEvent e)
        {
            done = true;
        }

        private void StartRenderer(StartRendererEvent e)
        {
            if (Scene.Instance != null)
            {
                Scene.Instance.Reset();
            }
            else
            {
                new Scene();
            }
            EventManager.Instance.Raise(new CloseMenuEvent());
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
            Scene.Instance?.Update();
        }

        private void Draw()
        {
            if (MenuManager.Instance.ActiveMenu != null)
            {
                MenuManager.Instance.ActiveMenu.Draw();
            }
            else
            {
                Scene.Instance?.Draw();
            }
            Console.Title = $"{PerformanceInfo.Instance.averageFPS} FPS ({ConsoleDisplay.Instance.consoleWidth}x{ConsoleDisplay.Instance.consoleHeight})";
            ConsoleDisplay.Instance.Refresh_GreyScale();
        }
    }
}
