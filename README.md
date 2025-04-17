# ConsoleEngine
3D Renderer running inside the windows console.
More details here:
https://www.notion.so/alexfoulon/Console-Engine-447dd53f8f0f42059319ac9a5c8d2b4f

### How to use
- In the "bin\Release\net6.0", execute ConsoleEngine.exe.

- Before starting the renderer, you can customize your scene if you want:
  - For that, you can click the "edit scene" button that will open up a text-based scene editor.
  - To add objects, you can use the "obj" keyword followed by the path to it (it can be absolute or relative).
  - After that, you can set it's position using "pos" keyword followed by 3 numeric values.
  - Each instruction must be in one line. Here is an example of a valid scene:
    ```
    obj "Rafale.obj"
    pos 0 0 10
    obj "Spring.obj"
    pos 5 0 10
    ```
  - Note that .obj files must only contain triangles, you can use blender to triangulate your meshes.
  - As of now, only the first object will rotate around the (0,0,10) coordinates.
