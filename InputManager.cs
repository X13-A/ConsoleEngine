using System;
using System.Collections.Generic;
using ConsoleEngine.EventSystem;

namespace ConsoleEngine
{
    public class InputManager : Singleton<InputManager>
    {
        public async void UpdateKeyStates()
        {
            ConsoleKey input = await GetInputAsync();
            EventManager.Instance.Raise(new KeyPressedEvent { key = input });
        }

        public async Task<ConsoleKey> GetInputAsync()
        {
            return await Task.Run(() =>
            {
                ConsoleKey key = Console.ReadKey(intercept:true).Key;
                return key;
            });
        }
    }
}
