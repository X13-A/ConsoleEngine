using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine.EventSystem;

namespace ConsoleEngine
{
    internal class Player
    {
        public TileCoordinate position;

        public Player()
        {
            position = new TileCoordinate(0, 0);
            SubscribeEvents();
        }

        public Player(TileCoordinate position)
        {
            this.position = position;
            SubscribeEvents();
        }

        public void SubscribeEvents()
        {
            EventManager.Instance.AddListener<KeyPressedEvent>(HandleKeyPressed);
        }

        public void Move(int x, int y)
        {
            if (Game.Instance.Map.GetTile(new TileCoordinate(position.x + x, position.y + y)).symbol != ' ') return;
            position.x += x;
            position.y += y;
        }

        public void HandleKeyPressed(KeyPressedEvent e)
        {
            if (e.key == ConsoleKey.UpArrow)
            {
                Move(0, -1);
            }
            else if (e.key == ConsoleKey.DownArrow)
            {
                Move(0, 1);
            }
            else if (e.key == ConsoleKey.RightArrow)
            {
                Move(1, 0);
            }
            else if (e.key == ConsoleKey.LeftArrow)
            {
                Move(-1, 0);
            }
            else if (e.key == ConsoleKey.Escape)
            {
                EventManager.Instance.Raise(new PauseGameEvent());
            }
        }
    }
}
