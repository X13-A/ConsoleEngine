using ConsoleEngine.EventSystem;
using ConsoleEngine.Importing;
using ConsoleEngine.Menus;
using ConsoleEngine.Render;
using ConsoleEngine.Render.Structs;
using System.Numerics;

namespace ConsoleEngine
{
    public class Scene3D : Singleton<Scene3D>
    {
        private Camera camera;
        private List<Shape> shapes = new List<Shape>();
        private float rotateSpeed = 0.01f;
        private float moveSpeed = 0.2f;

        public void Init()
        {
            SubscribeEvents();

            // Create first shape (cube)
            Shape cube1 = new Shape();

            cube1.vertices = new Vector3[] {
                new Vector3(-1, -1, -1), // 0
                new Vector3(-1, -1, 1),  // 1
                new Vector3(1, -1, 1),   // 2
                new Vector3(1, -1, -1),  // 3

                new Vector3(-1, 1, -1),  // 4
                new Vector3(-1, 1, 1),   // 5
                new Vector3(1, 1, 1),    // 6
                new Vector3(1, 1, -1),   // 7
            };

            cube1.indices = new int[] {
                // Front
                0, 4, 7,
                7, 3, 0,

                // Bottom
                0, 1, 2,
                2, 3, 0,

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

            cube1.colors = new ConsoleColor[]
            {
                // Front
                ConsoleColor.Cyan,
                ConsoleColor.Cyan,

                // Bottom
                ConsoleColor.DarkRed,
                ConsoleColor.DarkRed,

                // Right
                ConsoleColor.Yellow,
                ConsoleColor.Yellow,

                // Left
                ConsoleColor.Blue,
                ConsoleColor.Blue,

                // Top
                ConsoleColor.DarkCyan,
                ConsoleColor.DarkCyan,

                // Back
                ConsoleColor.Green,
                ConsoleColor.Green
            };

            //cube1.Translate(new Vector3(0, 0, 5));
            //shapes.Add(cube1);

            Shape? shape = ObjImporter.Import("C:\\Users\\Actility\\Downloads\\cat.obj");
            if (shape != null)
            {
                Shape actualShape = shape.Value;
                actualShape.Translate(new Vector3(0, 0, 5));
                actualShape.Scale(new Vector3(10, -10, 10), new Vector3(0,0,5));
                shapes.Add(actualShape);
            }

            //Shape cube2 = new Shape(cube1);
            //cube2.Translate(new Vector3(5, 0, 5));
            //shapes.Add(cube2);

            //Shape cube3 = new Shape(cube1);
            //cube3.Translate(new Vector3(-5, 0, 5));
            //shapes.Add(cube3);

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
            ConsoleDisplay2.Instance.DrawLine(camera.ToString(), ConsoleColor.White, ConsoleColor.DarkCyan);

            foreach (Shape shape in shapes)
            {
                Renderer.Instance.Render(shape, camera);
            }
            Renderer.Instance.Draw();
        }

        public void Update()
        {
            float rotateSpeed = 1f * (float) (App.Instance.FrameTime / 1000);
            foreach (Shape shape in shapes)
            {
                shape.Rotate(0, rotateSpeed, 0, new Vector3(0, 0, 5));
            }
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
