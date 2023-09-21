using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine.Menus;
#pragma warning disable CS8618

namespace ConsoleEngine.EventSystem
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

    public class StartRendererEvent : Event
    {
    }

    public class PauseGameEvent : Event
    {
    }

    public class ResumeGameEvent : Event
    {
    }

    public class ErrorEvent : Event
    {
        public string message;
    }

    public class ImportErrorEvent : ErrorEvent
    {
    }

    public class RenderErrorEvent : ErrorEvent
    {
    }

    public class WindowResizeEvent : Event
    {
        public int width;
        public int height;
    }
}







      