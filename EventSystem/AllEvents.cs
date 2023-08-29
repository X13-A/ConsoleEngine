using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console3DRenderer.Menus;
#pragma warning disable CS8618

namespace Console3DRenderer.EventSystem
{
    public class KeyPressedEvent : Event
    {
        public ConsoleKey key;
    }

    public class OpenMenuEvent : Event
    {
        public Menu menu;
    }

    public class CloseMenuEvent : Event
    {
    }

    public class ExitAppEvent : Event
    {
    }

    public class StartGameEvent : Event
    {
    }

    public class PauseGameEvent : Event
    {
    }

    public class ResumeGameEvent : Event
    {
    }
}
