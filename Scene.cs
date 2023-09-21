using ConsoleEngine.EventSystem;
using ConsoleEngine.Importing;
using ConsoleEngine.Menus;
using ConsoleEngine.Render;
using ConsoleEngine.Render.Structs;
using System.Numerics;

namespace ConsoleEngine
{
    public class Scene : Singleton<Scene>
    {
        private Camera camera;
        private List<Shape> shapes = new List<Shape>();
        private float rotateSpeed = 0.02f;
        private float moveSpeed = 0.05f;

        public Shape CreateCube()
        {
            Shape cube = new Shape();
            cube.vertices = new Vector3[] {
                new Vector3(-1, -1, -1), // 0
                new Vector3(-1, -1, 1),  // 1
                new Vector3(1, -1, 1),   // 2
                new Vector3(1, -1, -1),  // 3

                new Vector3(-1, 1, -1),  // 4
                new Vector3(-1, 1, 1),   // 5
                new Vector3(1, 1, 1),    // 6
                new Vector3(1, 1, -1),   // 7
            };
            cube.indices = new int[] {
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
            return cube;
        }

        protected override void Init()
        {
            shapes.Clear();
            SubscribeEvents();
            LoadScene();
            camera = new Camera();
            camera.Position = new Vector3(0, 6, 3.5f);
            camera.Rotation = new Vector3(-0.8f, 0, 0);
        }

        public void Reset()
        {
            shapes.Clear();
            LoadScene();
            camera = new Camera();
            camera.Position = new Vector3(0, 6, 3.5f);
            camera.Rotation = new Vector3(-0.8f, 0, 0);
        }

        public void LoadScene()
        {
            if (!File.Exists(Settings.scenePath))
            {
                EventManager.Instance.Raise(new ImportErrorEvent { message = $"File {Settings.scenePath} not found."});
                return;
            }

            Shape? shape = null;
            Shape currentShape = new Shape(); 
            foreach (var line in File.ReadLines(Settings.scenePath))
            {
                var parts = line.Replace("\"", "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0) continue;

                if (parts[0].ToLower() == "obj" && parts.Length >= 2)
                {
                    string objPath = parts[1];

                    shape = ObjImporter.Import(objPath);
                    if (shape.HasValue)
                    {
                        currentShape = shape.Value;
                        shapes.Add(currentShape);
                    }
                }
                else if (parts[0].ToLower() == "pos" && shape.HasValue && parts.Length >= 4)
                {
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[2]);
                    float z = float.Parse(parts[3]);
                    currentShape.Translate(new Vector3(x, y, z));
                    currentShape.origin = new Vector3(x, y, z);
                }
            }
        }

        public void SubscribeEvents()
        {
            EventManager.Instance.AddListener<PauseGameEvent>(Pause);
            EventManager.Instance.AddListener<ResumeGameEvent>(Resume);
            EventManager.Instance.AddListener<KeyPressedEvent>(HandleKeyPressed);
        }

        private void Pause(PauseGameEvent e)
        {
            EventManager.Instance.Raise(new OpenMenuEvent { menu = MenuStorage.Get(MenuID.PauseMenu)});
        }

        private void Resume(ResumeGameEvent e)
        {
            EventManager.Instance.Raise(new CloseMenuEvent());
        }

        public void Draw()
        {
            if (camera == null) return;

            foreach (Shape shape in shapes)
            {
                Renderer.Instance.Render(shape, camera);
            }
            Renderer.Instance.Draw();
        }

        public void Update()
        {
            float rotateSpeed = 0.25f * ((float) PerformanceInfo.Instance.DeltaTime);
            foreach (Shape shape in shapes)
            {
                shape.Rotate(0, rotateSpeed, 0, shape.origin);
            }
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
