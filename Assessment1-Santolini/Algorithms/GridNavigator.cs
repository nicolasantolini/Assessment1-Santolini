using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;

namespace Assessment1_Santolini.Algorithms
{
    /// <summary>
    /// This class provides functionality to navigate a grid by selecting the best neighboring plane to move to based on a scoring system.
    /// </summary>
    public class GridNavigator : IGridNavigator
    {
        /// <summary>
        /// Determines the best neighboring plane to move to based on a scoring system.
        /// </summary>
        /// <remarks>The method evaluates all neighboring planes of the current position and calculates a
        /// score for each plane based on the provided <paramref name="scoreCalculator"/> and whether the plane has been
        /// visited during the current path. The plane with the highest score is selected as the best neighbor.</remarks>
        /// <param name="grid">The grid containing the planes and their properties.</param>
        /// <param name="position">The current position as a tuple of x and y coordinates.</param>
        /// <param name="currentTime">The current time step, used for score calculations.</param>
        /// <param name="locallyVisited">A 2D boolean array indicating whether each plane has been visited during the current path.</param>
        /// <param name="scoreCalculator">An implementation of <see cref="IScoreCalculator"/> used to calculate the score for each plane.</param>
        /// <returns>A tuple representing the coordinates of the best neighboring plane to move to. If no neighbors are available,
        /// returns the current position.</returns>
        public (int x, int y) GetBestNeighbor(Grid grid, (int x, int y) position, int currentTime, bool[,] locallyVisited, IScoreCalculator scoreCalculator)
        {
            var neighbors = grid.GetNeighborCoordinates(position.x, position.y).ToList();
            if (!neighbors.Any()) return position;

            var scoredNeighbors = neighbors.Select(neighbor =>
            {
                var plane = grid.GetPlane(neighbor.x, neighbor.y);
                int baseScore = scoreCalculator.CalculatePlaneScore(plane, currentTime + 1);

                // 1. Strong bonus for planes never visited on this path
                bool isUnexplored = !locallyVisited[neighbor.y, neighbor.x];
                double explorationBonus = isUnexplored ? 10.0 : 0.0;

                // 2. Combine scores
                double totalScore = (baseScore) + explorationBonus;

                return (neighbor, totalScore);
            });

            return scoredNeighbors.OrderByDescending(n => n.totalScore).First().neighbor;
        }
    }
}
