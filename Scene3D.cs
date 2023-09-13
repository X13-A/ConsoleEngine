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
        private float rotateSpeed = 0.04f;
        private float moveSpeed = 0.05f;
        private Vector3 origin = new Vector3(0f, 0f, 10f);
        public void Init()
        {
            SubscribeEvents();
            shapes.Clear();

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
                ConsoleColor.Blue,
                ConsoleColor.Blue,

                // Bottom
                ConsoleColor.Yellow,
                ConsoleColor.Yellow,

                // Right
                ConsoleColor.Red,
                ConsoleColor.Red,

                // Left
                ConsoleColor.Green,
                ConsoleColor.Green,

                // Top
                ConsoleColor.Yellow,
                ConsoleColor.Yellow,

                // Back
                ConsoleColor.Blue,
                ConsoleColor.Blue
            };
            cube1.Translate(origin);
            //shapes.Add(cube1);

            Shape? shape = ObjImporter.Import(Settings.objectPath);
            if (shape == null)
            {
                ConsoleDisplay2.Instance.DrawLine("Invalid model or model path.", ConsoleColor.Red, ConsoleColor.Black);
                ConsoleDisplay2.Instance.DrawLine("(3D models should only contain triangles)", ConsoleColor.Red, ConsoleColor.Black);
                return;
            }

            Shape actualShape = shape.Value;
            actualShape.Translate(origin);
            //actualShape.Rotate(0, -60, 0, origin);
            actualShape.Scale(new Vector3(2f, 2f, 2f), origin);
            shapes.Add(actualShape);

            Shape clone1 = new Shape(actualShape);
            clone1.Translate(new Vector3(-5, 0, 0));
            shapes.Add(clone1);

            Shape clone2 = new Shape(actualShape);
            clone2.Translate(new Vector3(5, 0, 0));
            shapes.Add(clone2);

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
            float rotateSpeed = 1f * ((float) PerformanceInfo.Instance.DeltaTime);
            if (shapes.Count == 0) return;
            shapes[0].Rotate(0, rotateSpeed, 0, origin);
            //shapes[1].Rotate(0, rotateSpeed, 0, new Vector3(5, 0, 5));
            //foreach (Shape shape in shapes)
            //{
            //    shape.Rotate(0, rotateSpeed, 0, new Vector3(0, 0, 5));
            //}
        }

        public void Rotate(float x, float y, float z)
        {
            camera.Rotation.X += x;
            camera.Rotation.Y += y;
            camera.Rotation.Z += z;
        }

        public void Move(Vector3 distance)
        {
            camera.Move(distance);
        }

        public void HandleKeyPressed(KeyPressedEvent e)
        {
            // Rotate
            if (e.key == ConsoleKey.UpArrow)
            {
                Rotate(rotateSpeed, 0, 0);
            }
            else if (e.key == ConsoleKey.DownArrow)
            {
                Rotate(-rotateSpeed, 0, 0);
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
                camera.Move(new Vector3(0, 0, moveSpeed));
            }
            else if (e.key == ConsoleKey.Q)
            {
                camera.Move(new Vector3(-moveSpeed, 0, 0));
            }
            else if (e.key == ConsoleKey.S)
            {
                camera.Move(new Vector3(0, 0, -moveSpeed));
            }
            else if (e.key == ConsoleKey.D)
            {
                camera.Move(new Vector3(moveSpeed, 0, 0));
            }
            else if (e.key == ConsoleKey.Spacebar)
            {
                camera.Move(new Vector3(0, moveSpeed, 0));
            }
            else if (e.key == ConsoleKey.C)
            {
                camera.Move(new Vector3(0, -moveSpeed, 0));
            }

            // Pause
            else if (e.key == ConsoleKey.Escape)
            {
                EventManager.Instance.Raise(new PauseGameEvent());
            }
        }
    }
}
