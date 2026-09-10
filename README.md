# CartesianRobotSim

A small WPF (`net10.0-windows`) application that simulates a Cartesian robot using a 3D model.  
The project follows the MVVM design pattern and provides interactive controls to visualize and animate robot movement.

## Features

- 3D visualization of a Cartesian robot
- Interactive controls for robot movement and orientation
- Real-time animated robot motion
- Manual movement in X/Y/Z directions
- Move-to-point control using specific X/Y/Z coordinates
- Circular motion around a selected axis (X/Y/Z), with configurable center and radius
- Path creation with up to 5 vertices
- Path deletion
- Path persistence between sessions

## Prerequisites

- .NET 10 SDK
- Visual Studio 2022 or later
- Build target: `net10.0-windows` with `UseWPF` enabled

## Build and run

1. Clone the repository to your local machine.
2. Open the solution (`CartesianRobotSim.slnx`) in Visual Studio.
3. Build the solution to restore NuGet packages and compile the application.
4. Run the application by pressing **F5** or selecting **Start Debugging**.

## Path storage and logs

The application stores user-created paths in a local file for persistence between sessions.

- Saved to: `%LOCALAPPDATA%\CartesianRobotSim\Paths.txt`
- If the folder or file does not exist, it is created when saving a path for the first time.

Expected line format (one path per line):

`(x1,y1,z1);(x2,y2,z2);(x3,y3,z3);(x4,y4,z4);(x5,y5,z5)`

Example:

`(0,0,0);(10,10,10);(20,20,20);(30,30,30);(40,40,40)`

- Semicolons separate vertices.
- Parentheses wrap each vertex.
- Commas separate X, Y, and Z values.
- Each path supports up to 5 vertices; additional vertices are ignored.
- The serializer writes in the same format for reliable restore.

Startup and exception logs are stored in the same folder (for example, `startup_log.txt`).

## UI controls

- **Move**: Move the robot to a specified X/Y/Z position.
- **Axis selection**: Choose X, Y, or Z for circular movement.
- **Circle settings**: Set radius and center for circular trajectories.
- **Path creation**: Build motion paths with up to 5 vertices.
- **Path deletion**: Remove saved paths from the simulation and storage.
- **Vertex add/remove**: Edit vertices for the currently built path.
- **X/Y/Z movement**: Incrementally move the robot along each axis.

## Troubleshooting

- Ensure prerequisites are installed and development tools are configured correctly.
- For unexpected behavior or errors, check logs in `%LOCALAPPDATA%\CartesianRobotSim\`.
- If paths do not appear, verify `Paths.txt` formatting and file permissions.

## Contributing

Contributions are welcome through issues and pull requests.

## License

No license file is currently included in the repository.
