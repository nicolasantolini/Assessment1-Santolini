using Assessment1_Santolini.Algorithms;
using Assessment1_Santolini.Display;
using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assessment1_Santolini.Navigation
{
    /// <summary>
    /// This class manages the drone's navigation through the grid, calculating the optimal path and score based on provided algorithms.
    /// </summary>
    public class DroneManager
    {
        private readonly IScoreCalculator _scoreCalculator;
        private readonly IGridNavigator _gridNavigator;
        private readonly IPathRenderer _pathRenderer;

        /// <summary>
        /// Initializes a new instance of the DroneManager class with the necessary services injections.
        /// </summary>
        /// <param name="scoreCalculator">Calculates the value of a plane based on the current time.</param>
        /// <param name="gridNavigator">Navigates and reports information of a plane's neighbouring planes.</param>
        /// <param name="pathRenderer">Displays visually the steps of the operation.</param>
        public DroneManager(IScoreCalculator scoreCalculator, IGridNavigator gridNavigator, IPathRenderer pathRenderer)
        {
            _scoreCalculator = scoreCalculator;
            _gridNavigator = gridNavigator;
            _pathRenderer = pathRenderer;
        }

        /// <summary>
        /// Calculates the optimal path through a grid based on the given parameters and returns the path along with its
        /// score.
        /// </summary>
        /// <remarks>The method navigates the grid by evaluating the best possible moves at each step, 
        /// considering constraints such as the time limit and the maximum number of steps.  The path is stored at
        /// each step, and the score reflects the cumulative value of the path.</remarks>
        /// <param name="grid">The grid representing the environment to navigate.</param>
        /// <param name="N">The size N of the NxN grid.</param>
        /// <param name="t">The maximum number of steps allowed for the path.</param>
        /// <param name="T">The time limit, in milliseconds, for the operation to complete.</param>
        /// <param name="start">The starting position in the grid, represented as a tuple of x and y coordinates.</param>
        /// <returns>A tuple containing the optimal path and its associated score.  The path is a list of tuples representing the
        /// x and y coordinates of each step,  and the score is an integer representing the total score of the path.</returns>
        public (List<(int x, int y)>, int score) GetOptimalPath(Grid grid, int N, int t, int T, (int x, int y) start)
        {
            ValidateInputs(N, t, T, grid, start);

            var stopwatch = Stopwatch.StartNew();
            var path = InitializePath(start);
            int score = 0;
            var currentPosition = start;

            _pathRenderer.PrintStart(start, t, T);

            for (int step = 0; step < t; step++)
            {
                if (CheckTimeLimit(stopwatch, T, step)) break;

                int currentTime = (int)stopwatch.ElapsedMilliseconds;
                score = ProcessCurrentPlane(grid, currentPosition, currentTime, score);

                RenderCurrentStep(step, stopwatch.ElapsedMilliseconds, currentPosition, score, grid, currentTime);

                if (CheckTimeLimit(stopwatch, T, step)) break;

                var locallyVisited = BuildLocallyVisitedMap(grid.Size, path);
                var nextPosition = FindNextPosition(grid, currentPosition, currentTime, locallyVisited);

                UpdatePlaneVisitStatus(grid, currentPosition, currentTime);

                if (step < t - 1) // Avoid updating the path on the last step
                {
                    UpdatePathAndPosition(path, nextPosition, ref currentPosition);
                }
            }

            stopwatch.Stop();
            _pathRenderer.PrintCompletion(stopwatch.ElapsedMilliseconds);

            return (path, score);
        }

        /// <summary>
        /// Validates the input parameters for the GetOptimalPath method.
        /// </summary>
        /// <param name="N"></param>
        /// <param name="t"></param>
        /// <param name="T"></param>
        /// <param name="grid"></param>
        /// <param name="start"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateInputs(int N, int t, int T, Grid grid, (int x, int y) start)
        {
            if (N != 20 && N != 100 && N != 1000)
                throw new ArgumentException("Invalid grid size. Must be 20, 100, or 1000.");

            if (t < 1)
                throw new ArgumentException("Invalid number of steps. Must be greater than 0.");

            if (T < 1)
                throw new ArgumentException("Invalid time limit. Must be greater than 0 milliseconds.");

            if (!grid.IsValidCoordinate(start.x, start.y))
                throw new ArgumentException("Start position is outside grid bounds.");
        }

        /// <summary>
        /// Initializes the path with the starting position.
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private List<(int x, int y)> InitializePath((int x, int y) start)
        {
            return new List<(int x, int y)> { start };
        }

        /// <summary>
        /// Determines if the time limit has been reached and prints a message if so. The returns true or false accordingly.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <param name="T"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        private bool CheckTimeLimit(Stopwatch stopwatch, int T, int step)
        {
            if (stopwatch.ElapsedMilliseconds >= T)
            {
                _pathRenderer.PrintTimeLimitReached(T, step);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the current plane by calculating its score and updating the current cumulated score.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="position"></param>
        /// <param name="currentTime"></param>
        /// <param name="currentScore"></param>
        /// <returns></returns>
        private int ProcessCurrentPlane(Grid grid, (int x, int y) position, int currentTime, int currentScore)
        {
            var plane = grid.GetPlane(position.x, position.y);
            int planeValue = _scoreCalculator.CalculatePlaneScore(plane, currentTime);
            return currentScore + planeValue;
        }

        /// <summary>
        /// Invokes the path renderer to display the current step's information.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="elapsedMs"></param>
        /// <param name="currentPosition"></param>
        /// <param name="score"></param>
        /// <param name="grid"></param>
        /// <param name="currentTime"></param>
        private void RenderCurrentStep(int step, long elapsedMs, (int x, int y) currentPosition, int score, Grid grid, int currentTime)
        {
            var currentPlane = grid.GetPlane(currentPosition.x, currentPosition.y);
            int planeValue = _scoreCalculator.CalculatePlaneScore(currentPlane, currentTime);

            _pathRenderer.PrintStepHeader(step, elapsedMs, currentPosition, planeValue, score);
            _pathRenderer.PrintSurroundingGrid(grid, _scoreCalculator, currentTime, currentPosition);
            _pathRenderer.PrintCompleteGrid(grid, _scoreCalculator, currentTime, currentPosition);
        }

        /// <summary>
        /// Creates a 2D boolean array to track the planes visited during the current path.
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool[,] BuildLocallyVisitedMap(int gridSize, List<(int x, int y)> path)
        {
            var locallyVisited = new bool[gridSize, gridSize];
            foreach (var pos in path)
            {
                locallyVisited[pos.y, pos.x] = true;
            }
            return locallyVisited;
        }

        /// <summary>
        /// Determines the next position to move to by relying on the grid navigator to select the best neighboring plane.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="currentPosition"></param>
        /// <param name="currentTime"></param>
        /// <param name="locallyVisited"></param>
        /// <returns></returns>
        private (int x, int y) FindNextPosition(Grid grid, (int x, int y) currentPosition, int currentTime, bool[,] locallyVisited)
        {
            var nextPosition = _gridNavigator.GetBestNeighbor(
                grid,
                (currentPosition.x, currentPosition.y),
                currentTime,
                locallyVisited,
                _scoreCalculator
            );
            _pathRenderer.PrintMove(nextPosition);
            return nextPosition;
        }

        /// <summary>
        /// Updates the path collection with the new position and sets the current position to the new position.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nextPosition"></param>
        /// <param name="currentPosition"></param>
        private void UpdatePathAndPosition(List<(int x, int y)> path, (int x, int y) nextPosition, ref (int x, int y) currentPosition)
        {
            path.Add(nextPosition);
            currentPosition = nextPosition;
        }

        /// <summary>
        /// Updates the visit status of the current plane to mark that it has been visited at the current time.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="position"></param>
        /// <param name="currentTime"></param>
        private void UpdatePlaneVisitStatus(Grid grid, (int x, int y) position, int currentTime)
        {
            var plane = grid.GetPlane(position.x, position.y);
            plane.Visit(currentTime);
        }
    }
}