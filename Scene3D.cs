using ConsoleEngine.EventSystem;
using ConsoleEngine.Menus;
using ConsoleEngine.Render;

namespace ConsoleEngine
{
    public class Scene3D : Singleton<Scene3D>
    {
        private Camera camera;
        private List<Shape> shapes = new List<Shape>();
        private float rotateSpeed = 0.005f;
        private float moveSpeed = 0.1f;

        public void Init()
        {
            SubscribeEvents();

            // Create first shape (cube)
            Shape cube = new Shape();

            cube.vertices = new Point3D[] {
                new Point3D(-1, -1, -1), // 0
                new Point3D(-1, -1, 1),  // 1
                new Point3D(1, -1, 1),   // 2
                new Point3D(1, -1, -1),  // 3

                new Point3D(-1, 1, -1),  // 4
                new Point3D(-1, 1, 1),   // 5
                new Point3D(1, 1, 1),    // 6
                new Point3D(1, 1, -1),   // 7
            };

            cube.indices = new int[] {
                // Front
                0, 4, 7,
                7, 3, 0,

                // Bottom
                0, 1, 2,
                3, 0, 1,

                // Right
                3, 7, 6,
                6, 2, 3,

                // Left
                0, 1, 5,
                5, 4, 0,

                // Top
                4, 5, 6,
                7, 4, 6,

                // Back
                1, 2, 5,
                5, 6, 2
            };

            foreach (Point3D point in cube.vertices)
            {
                point.X += 5f;
                point.Y += 5f;
                point.Z += 5f;
            }

            shapes.Add(cube);
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

            foreach (Shape shape in shapes)
            {
                Renderer.Instance.Render(shape, camera);
            }
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
