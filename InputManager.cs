using System;
using System.Collections.Generic;
using ConsoleEngine.EventSystem;
using ConsoleEngine.Render;

namespace ConsoleEngine
{
    public class InputManager : Singleton<InputManager>
    {
        public async void UpdateKeyStates()
        {
            ConsoleKey? input = await GetInputAsync();
            if (input == null) return;
            EventManager.Instance.Raise(new KeyPressedEvent { key = input.Value });
        }

        public async Task<ConsoleKey?> GetInputAsync()
        {
            return await Task.Run(() =>
            {
                if (App.Instance.AppState == AppState.WaitingInput) return null;
                ConsoleKey? key = Console.ReadKey(intercept:true).Key;
                return key;
            });
        }

        public string? OpenInputScreen(string prompt)
        {
            Console.Clear();
            System.Diagnostics.Process.Start("cmd", "/c cls").WaitForExit();
            Console.SetCursorPosition(0, 0);
            Console.Write(prompt);
            EventManager.Instance.Raise(new WaitForInputEvent());

            int x = (prompt.Length + 1) % Console.WindowWidth;
            int y = (int) prompt.Length / (int) Console.WindowWidth;
            Console.SetCursorPosition(x, y);
            string? res = Console.ReadLine();
            EventManager.Instance.Raise(new InputFinishedEvent());
            Console.Clear();
            ConsoleDisplay2.Instance.Clear();
            return res;
        }
    }
}
