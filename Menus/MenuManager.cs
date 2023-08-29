using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console3DRenderer.EventSystem;

namespace Console3DRenderer.Menus
{
    public class MenuManager : Singleton<MenuManager>
    {
        private Menu? activeMenu;
        public Menu? ActiveMenu => activeMenu;
        public bool MenuOpened => activeMenu == null;

        private int cursorIndex;

        public int CursorIndex => cursorIndex;

        private Menu mainMenu;
        private Menu pauseMenu;
        public Menu MainMenu => mainMenu;
        public Menu PauseMenu => pauseMenu;

        public void Init()
        {
            SubscribeEvents();
            CreateRendererMenus();
        }

        public void CreateRendererMenus()
        {
            mainMenu = new ConsoleMenu("Main menu");

            ConsoleMenuItem exitButton = new ConsoleMenuItem("Exit", () => EventManager.Instance.Raise(new ExitAppEvent()));
            exitButton.color = ConsoleColor.White;
            exitButton.highlightColor = ConsoleColor.Red;

            mainMenu.AddItem(new ConsoleMenuItem("Start renderer", () => EventManager.Instance.Raise(new StartGameEvent())));
            mainMenu.AddItem(new ConsoleMenuItem("Settings", () => { }));
            mainMenu.AddItem(exitButton);

            pauseMenu = new ConsoleMenu("Paused");
            pauseMenu.AddItem(new ConsoleMenuItem("Resume", () => EventManager.Instance.Raise(new ResumeGameEvent())));
            pauseMenu.AddItem(new ConsoleMenuItem("Settings", () => { }));
            pauseMenu.AddItem(ConsoleMenuItem.CreateBackButton((ConsoleMenu)mainMenu));

            EventManager.Instance.Raise(new OpenMenuEvent { menu = mainMenu });
        }

        public void CreateMenusExample()
        {
            ConsoleMenu mainMenu = new ConsoleMenu("Main menu");
            ConsoleMenu animals = new ConsoleMenu("Animals");
            ConsoleMenu food = new ConsoleMenu("Food");
            ConsoleMenu countries = new ConsoleMenu("Countries");

            ConsoleMenuItem exitButton = new ConsoleMenuItem("Exit", () => EventManager.Instance.Raise(new ExitAppEvent()));
            exitButton.color = ConsoleColor.Red;
            exitButton.highlightColor = ConsoleColor.Red;
            mainMenu.AddItem(animals);
            mainMenu.AddItem(food);
            mainMenu.AddItem(countries);
            mainMenu.AddItem(exitButton);

            ConsoleMenuItem backToMainMenu = ConsoleMenuItem.CreateBackButton(mainMenu);

            animals.AddItem(backToMainMenu);
            animals.AddItem(new ConsoleMenuItem("Cat"));
            animals.AddItem(new ConsoleMenuItem("Dog"));
            animals.AddItem(new ConsoleMenuItem("Cow"));

            food.AddItem(backToMainMenu);
            food.AddItem(new ConsoleMenuItem("Salad"));
            food.AddItem(new ConsoleMenuItem("Tomato"));
            food.AddItem(new ConsoleMenuItem("Pizza"));

            countries.AddItem(backToMainMenu);
            countries.AddItem(new ConsoleMenuItem("France"));
            countries.AddItem(new ConsoleMenuItem("Greece"));
            countries.AddItem(new ConsoleMenuItem("Spain"));

            EventManager.Instance.Raise(new OpenMenuEvent { menu = mainMenu });
        }

        public void SubscribeEvents()
        {
            EventManager.Instance.AddListener<OpenMenuEvent>(OpenMenu);
            EventManager.Instance.AddListener<CloseMenuEvent>(CloseMenu);
            EventManager.Instance.AddListener<KeyPressedEvent>(NavigateMenu);
        }

        public void OpenMenu(OpenMenuEvent e)
        {
            activeMenu = e.menu;
            cursorIndex = 0;
        }

        public void CloseMenu(CloseMenuEvent e)
        {
            activeMenu = null;
        }

        public void NavigateMenu(KeyPressedEvent e)
        {
            if (activeMenu == null) return;

            if (e.key == ConsoleKey.UpArrow)
            {
                cursorIndex = Math.Max(0, cursorIndex - 1);
            }
            else if (e.key == ConsoleKey.DownArrow)
            {
                cursorIndex = Math.Min(activeMenu.Items.Count - 1, cursorIndex + 1);
            }
            else if (e.key == ConsoleKey.Enter)
            {
                if (cursorIndex <= activeMenu.Items.Count - 1)
                {
                    activeMenu.Items[cursorIndex].action?.Invoke();
                }
            }
        }
    }
}
