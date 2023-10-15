using ConsoleEngine.EventSystem;

namespace ConsoleEngine.Render
{
    public class ConsoleDisplay : Singleton<ConsoleDisplay>
    {
        private RenderFrame oldFrame;
        private RenderFrame frame;
        private Vector2Int cursor = new Vector2Int(0, 0);
        public int consoleWidth = Console.WindowWidth;
        public int consoleHeight = Console.WindowHeight;
        private int lastResize = 0;
        private bool isCursorInFrame => cursor.X < frame.Width && cursor.Y < frame.Height;

        protected override void Init()
        {
            Console.CursorVisible = false;
            frame = new RenderFrame(Console.WindowWidth, Console.WindowHeight);
            frame.Clear();
            oldFrame = new RenderFrame(frame);
            AddListeners();
        }

        public void AddListeners()
        {
            EventManager.Instance.AddListener<WindowResizeEvent>(Resize);
        }

        public void DrawPixel(ConsolePixel pixel)
        {
            if (!isCursorInFrame) return;

            frame.SetPixel(cursor.X, cursor.Y, pixel);
            cursor.X++;
        }

        public void DrawLine(List<ConsolePixel> pixels)
        {
            for (int i = 0; i < pixels.Count; i++)
            {
                DrawPixel(pixels[i]);
            }
            cursor.Y++;
            cursor.X = 0;
        }

        public void DrawLine(string line) => DrawLine(line, ConsoleColor.White, ConsoleColor.Black);

        public void DrawLine(string line, ConsoleColor color, ConsoleColor backgroundColor)
        {
            for (int i = 0; i < line.Length; i++)
            {
                DrawPixel(new ConsolePixel(color, backgroundColor, line[i], 0));
            }
            cursor.Y++;
            cursor.X = 0;
        }

        public void DrawFrame(RenderFrame frame)
        {
            for (int y = 0; y < frame.Height; y++)
            {
                for (int x = 0; x < frame.Width; x++)
                {
                    DrawPixel(frame.GetPixel(x, y));
                }
                cursor.Y++;
                cursor.X = 0;
            }
        }

        public Vector2Int GetNextDifferencePosition(Vector2Int cursor)
        {

            ConsolePixel startPixel = frame.GetPixel(cursor.X, cursor.Y);
            IncrementCursor(ref cursor);

            while (IsCursorInBounds(cursor) && startPixel.HasSameDisplay(frame.GetPixel(cursor)))
            {
                IncrementCursor(ref cursor);
            }

            return cursor;
        }

        public void IncrementCursor(ref Vector2Int cursor)
        {
            cursor.X++;
            if (cursor.X == frame.Width)
            {
                cursor.X = 0;
                cursor.Y++;
            }
        }

        public bool IsCursorInBounds(Vector2Int cursor)
        {
            return cursor.Y < frame.Height;
        }

        public void Refresh_new()
        {
            cursor.X = 0; cursor.Y = 0;
            Vector2Int drawCursor = new Vector2Int(0, 0);
            do
            {
                Vector2Int startPos = new Vector2Int(drawCursor);

                ConsolePixel newPixel = frame.GetPixel(drawCursor);
                ConsolePixel oldPixel = oldFrame.GetPixel(drawCursor);
                string block = "";

                if (newPixel.HasSameDisplay(oldPixel))
                {
                    IncrementCursor(ref drawCursor);
                    continue;
                }
                else
                {
                    // Get difference
                    Vector2Int nextDiffPos = GetNextDifferencePosition(drawCursor);
                    if (IsCursorInBounds(drawCursor))
                    {
                        do
                        {
                            block += frame.GetPixel(drawCursor).symbol;
                            IncrementCursor(ref drawCursor);
                        }
                        while (!drawCursor.Equals(nextDiffPos) && IsCursorInBounds(drawCursor));
                    }
                }

                Console.SetCursorPosition(startPos.X, startPos.Y);
                Console.ForegroundColor = newPixel.color;
                Console.BackgroundColor = newPixel.backgroundColor;
                Console.Write(block);
            }
            // NEEDS CHECKING
            while (IsCursorInBounds(drawCursor));

            oldFrame = new RenderFrame(frame);

            // TODO: Trigger resize only when Console size changes
            frame.Resize(Console.WindowWidth, Console.WindowHeight);
            frame.Clear();
        }

        public void CheckResize()
        {
            consoleWidth = Console.WindowWidth;
            consoleHeight = Console.WindowHeight;

            if (consoleWidth != frame.Width || consoleHeight != frame.Height)
            {
                EventManager.Instance.Raise(new WindowResizeEvent { width = consoleWidth, height = consoleHeight });
            }
        }

        public void Resize(WindowResizeEvent e)
        {
            frame.Resize(e.width, e.height);
            lastResize = 0;
            Console.Clear();
            System.Diagnostics.Process.Start("cmd", "/c cls").WaitForExit();
        }

        public void Refresh()
        {
            CheckResize();

            cursor.X = 0; cursor.Y = 0;
            Console.SetCursorPosition(0, 0);

            if (lastResize != 0)
            {
                for (int y = 0; y < frame.Height; y++)
                {
                    for (int x = 0; x < frame.Width; x++)
                    {
                        ConsolePixel newPixel = frame.GetPixel(x, y);
                        ConsolePixel oldPixel = oldFrame.GetPixel(x, y);
                        if (!newPixel.HasSameDisplay(oldPixel))
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = newPixel.color;
                            Console.BackgroundColor = newPixel.backgroundColor;
                            Console.Write(newPixel.symbol);
                            Console.ResetColor();
                        }
                    }
                }
            }

            lastResize++;

            oldFrame = new RenderFrame(frame);
            frame.Clear();
        }

        public void Refresh_GreyScale()
        {
            CheckResize();
            cursor.X = 0; cursor.Y = 0;
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            for (int y = 0; y < frame.Height; y++)
            {
                string line = "";
                for (int x = 0; x < frame.Width; x++)
                {
                    ConsolePixel pixel = frame.GetPixel(x, y);
                    if (pixel == null) line += ' ';
                    else line += pixel.symbol;
                }
                if (y == frame.Height - 1) Console.Write(line);
                else Console.WriteLine(line);
            }

            lastResize++;
            oldFrame = new RenderFrame(frame);
            frame.Clear();
        }

        public void Clear()
        {
            frame.Clear();
        }
    }
}
