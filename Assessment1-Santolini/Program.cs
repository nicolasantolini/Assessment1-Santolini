using Assessment1_Santolini.Algorithms;
using Assessment1_Santolini.Data;
using Assessment1_Santolini.Display;
using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Navigation;
using Assessment1_Santolini.Simulation;



//Initialize the components
IGridNavigator navigator = new GridNavigator();
IScoreCalculator scoreCalculator = new ScoreCalculator();
IPathRenderer pathRenderer = new PathRenderer();

//Initialize the drone manager to find the optimal path through the grid
DroneManager droneManager = new DroneManager(scoreCalculator, navigator, pathRenderer);

//Initialize parameters
int gridSize = 20;
int maxSteps = 20;
int timeLimitMs = 3000;
int startX = 0;
int startY = 0;

//Prompt user to input for parameters
Console.WriteLine("Enter grid size (only valid choices 20, 100, 1000): ");
string? gridSizeInput = Console.ReadLine();
if (int.TryParse(gridSizeInput, out int parsedGridSize) && (parsedGridSize == 20 || parsedGridSize == 100 || parsedGridSize == 1000))
{
    gridSize = parsedGridSize;
}
Console.WriteLine("Enter maximum steps (default 20): ");
string? maxStepsInput = Console.ReadLine();
if (int.TryParse(maxStepsInput, out int parsedMaxSteps) && parsedMaxSteps > 0 && parsedMaxSteps <= 100)
{
    maxSteps = parsedMaxSteps;
}
Console.WriteLine("Enter time limit in milliseconds (default 3000): ");
string? timeLimitInput = Console.ReadLine();
if (int.TryParse(timeLimitInput, out int parsedTimeLimit) && parsedTimeLimit > 0)
{
    timeLimitMs = parsedTimeLimit;
}

Console.WriteLine("Enter starting X coordinate (default 0): ");
string? startXInput = Console.ReadLine();
if (int.TryParse(startXInput, out int parsedStartX) && parsedStartX >= 0 && parsedStartX < gridSize)
{
    startX = parsedStartX;
}
Console.WriteLine("Enter starting Y coordinate (default 0): ");
string? startYInput = Console.ReadLine();
if (int.TryParse(startYInput, out int parsedStartY) && parsedStartY >= 0 && parsedStartY < gridSize)
{
    startY = parsedStartY;
}

//Initialize the grid
Grid grid = GridFillerFromTxt.LoadGridFromFile(gridSize);

try
{
    //Get the optimal path using the drone manager
    var (path, score) = droneManager.GetOptimalPath(grid, gridSize, maxSteps, timeLimitMs, (startX, startY));

    //Print the results
    Console.WriteLine($"Optimal path: {string.Join(" -> ", path.Select(p => $"({p.x},{p.y})"))}");
    Console.WriteLine($"Total score: {score}");
    Console.WriteLine($"Total unique planes visited: {path.Distinct().Count()} out of {grid.Size * grid.Size} ({(double)path.Distinct().Count() / (grid.Size * grid.Size) * 100:0.00}%)");


    //Draw the path on the grid
    for (int y = 0; y < grid.Size; y++)
    {
        for (int x = 0; x < grid.Size; x++)
        {
            if (path.Contains((x, y)))
            {
                Console.Write($"[{grid.GetPlane(x, y)?.Value.ToString().PadLeft(2) ?? "  "}] ");
            }
            else
            {
                Console.Write($" {grid.GetPlane(x, y)?.Value.ToString().PadLeft(2) ?? "  "}  ");
            }
        }
        Console.WriteLine();
    }

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
