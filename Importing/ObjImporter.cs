using ConsoleEngine.EventSystem;
using ConsoleEngine.Render;
using System.Globalization;
using System.Numerics;
using System.IO;
using System.Collections.Generic;

namespace ConsoleEngine.Importing
{
    public class ObjImporter
    {
        public static Shape? Import(string path)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> allNormals = new List<Vector3>();
            List<Vector3> triangleNormals = new List<Vector3>();
            List<int> indices = new List<int>();

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string? line;
                    do
                    {
                        line = reader.ReadLine();
                        if (line == null) break;

                        string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 0) continue;

                        if (parts[0] == "v")
                        {
                            if (parts.Length >= 4)
                            {
                                float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                                float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                                float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                                vertices.Add(new Vector3(x, y, z));
                            }
                        }
                        else if (parts[0] == "vn")
                        {
                            if (parts.Length >= 4)
                            {
                                float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                                float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                                float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                                allNormals.Add(Vector3.Normalize(new Vector3(x, y, z)));
                            }
                        }
                        else if (parts[0] == "f")
                        {
                            if (parts.Length >= 4)
                            {
                                Vector3 faceNormalSum = Vector3.Zero;

                                for (int i = 1; i <= 3; i++)
                                {
                                    string[] indicesStr = parts[i].Split('/');
                                    if (indicesStr.Length >= 1)
                                    {
                                        int vertexIndex = int.Parse(indicesStr[0]) - 1;
                                        indices.Add(vertexIndex);

                                        if (indicesStr.Length >= 3)
                                        {
                                            int normalIndex = int.Parse(indicesStr[2]) - 1;
                                            faceNormalSum += allNormals[normalIndex];
                                        }
                                    }
                                }

                                // Average the normals for this triangle
                                triangleNormals.Add(Vector3.Normalize(faceNormalSum));
                            }
                        }
                    }
                    while (line != null);
                }

                Shape result = new Shape();
                result.vertices = vertices.ToArray();
                result.indices = indices.ToArray();
                result.normals = triangleNormals.ToArray();
                return result;
            }
            catch (Exception e)
            {
                EventManager.Instance.Raise(new ErrorEvent { message = e.Message });
                Console.WriteLine("Error importing OBJ file: " + e.Message);
                return null;
            }
        }
    }
}
