using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;
using System;

namespace Assessment1_Santolini.Display
{
    /// <summary>
    /// This class implements the IPathRenderer interface to provide rendering on the console.
    /// </summary>
    public class PathRenderer : IPathRenderer
    {
        private const int ColumnWidth = 5;

        /// <summary>
        /// This method prints the starting information for the pathfinding process.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="maxSteps"></param>
        /// <param name="timeLimitMs"></param>
        public void PrintStart((int x, int y) start, int maxSteps, int timeLimitMs)
        {
            Console.WriteLine("Starting path at position ({0}, {1})", start.x, start.y);
            Console.WriteLine("Maximum steps: {0}, Time limit: {1}ms", maxSteps, timeLimitMs);
        }

        /// <summary>
        /// This method prints the header information for each step in the pathfinding process.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="elapsedMs"></param>
        /// <param name="currentPosition"></param>
        /// <param name="planeValue"></param>
        /// <param name="score"></param>
        public void PrintStepHeader(int step, long elapsedMs, (int x, int y) currentPosition, int planeValue, int score)
        {
            Console.WriteLine("\n--- Step: {0} (Time elapsed: {1}ms) ---", step, elapsedMs);
            Console.WriteLine("Current position: ({0}, {1})", currentPosition.x, currentPosition.y);
            Console.WriteLine("Plane value: {0}", planeValue);
            Console.WriteLine("Current score: {0}", score);
        }

        /// <summary>
        /// This method prints the values of the planes surrounding the current position in a 5x5 grid.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="scoreCalculator"></param>
        /// <param name="currentTime"></param>
        /// <param name="currentPosition"></param>
        public void PrintSurroundingGrid(Grid grid, IScoreCalculator scoreCalculator, int currentTime, (int x, int y) currentPosition)
        {
            Console.WriteLine("Surrounding grid values:");
            for (int y = Math.Max(0, currentPosition.y - 2); y <= Math.Min(grid.Size - 1, currentPosition.y + 2); y++)
            {
                var rowPlanes = new List<string>();
                for (int x = Math.Max(0, currentPosition.x - 2); x <= Math.Min(grid.Size - 1, currentPosition.x + 2); x++)
                {
                    int val = scoreCalculator.CalculatePlaneScore(grid.GetPlane(x, y), currentTime);
                    string formattedValue = val.ToString().PadLeft(ColumnWidth - 2);
                    rowPlanes.Add(x == currentPosition.x && y == currentPosition.y
                        ? $"[{formattedValue}]"
                        : $" {formattedValue} ");
                }
                Console.WriteLine(string.Join("", rowPlanes));
            }
        }

        /// <summary>
        /// This method prints the complete grid with all plane values, highlighting the current position.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="scoreCalculator"></param>
        /// <param name="currentTime"></param>
        /// <param name="currentPosition"></param>
        public void PrintCompleteGrid(Grid grid, IScoreCalculator scoreCalculator, int currentTime, (int x, int y) currentPosition)
        {
            int totalWidth = grid.Size * ColumnWidth;
            string separatorLine = new string('-', totalWidth);

            Console.WriteLine("\nComplete grid state:");
            Console.WriteLine(separatorLine);

            for (int y = 0; y < grid.Size; y++)
            {
                var rowPlanes = new List<string>();
                for (int x = 0; x < grid.Size; x++)
                {
                    int currentVal = scoreCalculator.CalculatePlaneScore(grid.GetPlane(x, y), currentTime);
                    string formattedValue = currentVal.ToString().PadLeft(ColumnWidth - 2);
                    rowPlanes.Add(x == currentPosition.x && y == currentPosition.y
                        ? $"[{formattedValue}]"
                        : $" {formattedValue} ");
                }
                Console.WriteLine(string.Join("", rowPlanes));
            }

            Console.WriteLine(separatorLine);
        }

        /// <summary>
        /// This method prints a message indicating that the time limit has been reached.
        /// </summary>
        /// <param name="timeLimitMs"></param>
        /// <param name="stepsCompleted"></param>
        public void PrintTimeLimitReached(int timeLimitMs, int stepsCompleted)
        {
            Console.WriteLine("Time limit of {0}ms reached after {1} steps", timeLimitMs, stepsCompleted);
        }

        /// <summary>
        /// This method prints the next move to be made.
        /// </summary>
        /// <param name="nextPosition"></param>
        public void PrintMove((int x, int y) nextPosition)
        {
            Console.WriteLine("Moving to: ({0}, {1})", nextPosition.x, nextPosition.y);
        }

        /// <summary>
        /// This method prints a message indicating the completion of the pathfinding process along with the elapsed time.
        /// </summary>
        /// <param name="elapsedMs"></param>
        public void PrintCompletion(long elapsedMs)
        {
            Console.WriteLine("\nPath finding completed in {0}ms", elapsedMs);
        }
    }
}