using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleEngine.EventSystem;
using ConsoleEngine.Menus;

namespace ConsoleEngine
{
    internal class Game : Singleton<Game>
    {
        private TileMap map;
        private Player player;

        public TileMap Map => map;

        public void Init()
        {
            SetupMap();
            EventManager.Instance.AddListener<PauseGameEvent>(Pause);
            EventManager.Instance.AddListener<ResumeGameEvent>(Resume);

            player = new Player(new TileCoordinate(3, 3));
        }

        private void Pause(PauseGameEvent e)
        {
            EventManager.Instance.Raise(new OpenMenuEvent { menu = MenuManager.Instance.PauseMenu });
        }

        private void Resume(ResumeGameEvent e)
        {
            EventManager.Instance.Raise(new CloseMenuEvent());
        }

        private void SetupMap()
        {
            string[] mapStrings =
            {
                "#######",
                "#     #",
                "#     #",
                "#     #",
                "#     #",
                "#     #",
                "#######",
            };
            map = new TileMap(mapStrings);
        }

        public void Draw()
        {
            if (map != null)
            {
                List<string> mapLines = map.AsStringList();

                // Draw player
                mapLines[player.position.y] = mapLines[player.position.y].Remove(player.position.x, 1).Insert(player.position.x, "x");
                
                // Draw map
                foreach (string line in mapLines)
                {
                    ConsoleDisplay.Instance.WriteLine(line);
                }
            }
        }

        public void Update()
        {

        }
    }
}
