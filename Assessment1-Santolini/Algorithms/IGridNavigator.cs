using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;
using System.Collections.Generic;

namespace Assessment1_Santolini.Algorithms
{
    /// <summary>
    /// This interface defines a rule for navigating a grid by selecting the best neighboring plane to move to based on a scoring system.
    /// </summary>
    public interface IGridNavigator
    {
        (int x, int y) GetBestNeighbor(Grid grid, (int x, int y) position, int currentTime, bool[,] locallyVisited, IScoreCalculator scoreCalculator);
    }
}