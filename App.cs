﻿using System;
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
                PerformanceInfo.Instance.Update();
            }
        }

        private void SetupSingletons()
        {
            new EventManager();
            new InputManager();
            new MenuManager();
            new ConsoleDisplay2();
            new Renderer();
            new Utils();
            new PerformanceInfo();
            PerformanceInfo.Instance.Init();
            MenuManager.Instance.Init();
            ConsoleDisplay2.Instance.Init();
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

            ConsoleDisplay2.Instance.Refresh();
        }
    }
}
