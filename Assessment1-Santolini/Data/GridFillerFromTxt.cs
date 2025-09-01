using Assessment1_Santolini.Entities;

namespace Assessment1_Santolini.Data
{

    /// <summary>
    /// This class provides functionality to load a grid content from a text file.
    /// </summary>
    public static class GridFillerFromTxt
    {
        private const int SMALL_GRID_SIZE = 20;
        private const int MEDIUM_GRID_SIZE = 100;
        private const int LARGE_GRID_SIZE = 1000;


        /// <summary>
        /// Loads a grid of specified size from file
        /// </summary>
        /// <param name="gridSize">Size of the grid to load</param>
        /// <returns>A grid object containing the data extracted from the file</returns>
        public static Grid LoadGridFromFile(int gridSize)
        {
            if (gridSize != SMALL_GRID_SIZE && gridSize != MEDIUM_GRID_SIZE && gridSize != LARGE_GRID_SIZE)
            {
                throw new ArgumentException("Invalid grid size. Must be 20, 100, or 1000.");
            }

            Grid grid = new Grid(gridSize);


            try
            {
				// Construct file path based on project structure
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\"));
                string filePath = Path.Combine(projectDirectory, "GridsFiles", $"{gridSize}.txt");

                // Read file contents and parse into grid
                string[] lines = File.ReadAllLines(filePath);

                for (int y = 0; y < gridSize; y++)
                {
                    string[] values = lines[y].Split(' ');
                    for (int x = 0; x < gridSize; x++)
                    {
                        int planeValue = int.Parse(values[x]);
                        grid.InitializePlane(x, y, planeValue); 
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"Grid file for size {gridSize} not found.");
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new DirectoryNotFoundException($"Directory not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading grid from file: " + ex.Message);
            }


            return grid;
        }

    }
}
