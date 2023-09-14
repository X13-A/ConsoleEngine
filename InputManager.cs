using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                ConsoleKey? key = Console.ReadKey(intercept:true).Key;
                return key;
            });
        }
    }
}
