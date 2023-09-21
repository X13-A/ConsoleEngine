# ConsoleEngine
3D Renderer running inside .NET console 

### How to use
- In the "bin\Release\net6.0", execute ConsoleEngine.exe.

- Before starting the renderer, you may need to configure your scene.
  - For that, you can click the "edit scene" button that will open up a text-based scene editor.
  - To add objects, you can use the "obj" keyword followed by the path to it (it can be absolute or relative).
  - After that, you can set it's position using "pos" keyword followed by 3 numeric values.
  - Each instruction must be in one line. Here is an example of a valid scene:
    obj "Rafale.obj"
    pos 0 0 10
  - Note that .obj files must only contain triangles, you can use blender to triangulate your meshes.
