using Console3DRenderer.EventSystem;
using Console3DRenderer.Menus;
using Console3DRenderer.Render;

namespace Console3DRenderer
{
    public class Scene3D : Singleton<Scene3D>
    {
        private Camera camera;
        private Point3D[] shape;
        private float rotateSpeed = 0.005f;
        private float moveSpeed = 0.1f;
        public void Init()
        {
            SubscribeEvents();
            shape = new Point3D[]
            {
                new Point3D(-1, -1, 5),
                new Point3D(1, -1, 5),

                new Point3D(1, 1, 5),
                new Point3D(-1, 1, 5),

                new Point3D(-1, -1, 10),
                new Point3D(1, -1, 10),

                new Point3D(1, 1, 10),
                new Point3D(-1, 1, 10)
            };

            foreach (Point3D point in shape)
            {
                point.X += 5f;
                point.Y += 5f;
            }
            camera = new Camera();
        }

        public void SubscribeEvents()
        {
            EventManager.Instance.AddListener<PauseGameEvent>(Pause);
            EventManager.Instance.AddListener<ResumeGameEvent>(Resume);
            EventManager.Instance.AddListener<KeyPressedEvent>(HandleKeyPressed);
        }

        private void Pause(PauseGameEvent e)
        {
            EventManager.Instance.Raise(new OpenMenuEvent { menu = MenuManager.Instance.PauseMenu });
        }

        private void Resume(ResumeGameEvent e)
        {
            EventManager.Instance.Raise(new CloseMenuEvent());
        }

        public void Draw()
        {
            if (camera == null) return;
            ConsoleDisplay.Instance.WriteLine(camera.ToString(), ConsoleColor.White, ConsoleColor.DarkCyan);
            Renderer.Instance.Render(shape, camera);
        }

        public void Update()
        {
        }

        public void Rotate(float x, float y, float z)
        {
            camera.RotationY += y;
        }

        public void Move(float x, float y, float z)
        {
            camera.Position.X += x;
            camera.Position.Y += y;
            camera.Position.Z += z;
        }

        public void HandleKeyPressed(KeyPressedEvent e)
        {
            // Rotate
            if (e.key == ConsoleKey.UpArrow)
            {
                Rotate(0, 0, 0);
            }
            else if (e.key == ConsoleKey.DownArrow)
            {
                Rotate(0, 0, 0);
            }
            else if (e.key == ConsoleKey.RightArrow)
            {
                Rotate(0, rotateSpeed, 0);
            }
            else if (e.key == ConsoleKey.LeftArrow)
            {
                Rotate(0, -rotateSpeed, 0);
            }

            // Move
            if (e.key == ConsoleKey.Z)
            {
                Move(0, 0, moveSpeed);
            }
            else if (e.key == ConsoleKey.Q)
            {
                Move(-moveSpeed, 0, 0);
            }
            else if (e.key == ConsoleKey.S)
            {
                Move(0, 0, -moveSpeed);
            }
            else if (e.key == ConsoleKey.D)
            {
                Move(moveSpeed, 0, 0);
            }
            else if (e.key == ConsoleKey.Spacebar)
            {
                Move(0, moveSpeed, 0);
            }
            else if (e.key == ConsoleKey.C)
            {
                Move(0, -moveSpeed, 0);
            }

            // Pause
            else if (e.key == ConsoleKey.Escape)
            {
                EventManager.Instance.Raise(new PauseGameEvent());
            }
        }
    }
}
