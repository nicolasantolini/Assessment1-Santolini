using Assessment1_Santolini.Algorithms;
using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;

namespace TestProject
{
    public class GridNavigatorTests
    {
        private readonly IGridNavigator _navigator;

        public GridNavigatorTests()
        {
            _navigator = new GridNavigator();
        }

        private static Grid CreateRandomGrid(int size, int seed = 123)
        {
            var rnd = new Random(seed);
            var grid = new Grid(size);
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    grid.InitializePlane(x, y, rnd.Next(0, 3)); // values 0..2  
            return grid;
        }

        private static bool[,] CreateVisited(Grid grid, bool initial = false)
        {
            var v = new bool[grid.Size, grid.Size];
            if (initial)
            {
                for (int y = 0; y < grid.Size; y++)
                    for (int x = 0; x < grid.Size; x++)
                        v[y, x] = true;
            }
            return v;
        }

        [Fact]
        public void NoNeighbors_ReturnsSamePosition()
        {
            // Arrange
            var grid = CreateRandomGrid(1, seed: 42);
            var visited = CreateVisited(grid);
            var scoreCalc = new ScoreCalculator();

            // Act
            var result = _navigator.GetBestNeighbor(grid, (0, 0), currentTime: 0, visited, scoreCalc);

            // Assert
            Assert.Equal((0, 0), result);
        }

        [Fact]
        public void PicksUnexploredOverExploredDueToBonus()
        {
            // Arrange
            var grid = CreateRandomGrid(3, seed: 100);
            var visited = CreateVisited(grid, initial: true);
            var scoreCalc = new ScoreCalculator();

            var pos = (x: 1, y: 1);

            // Make one neighbor unexplored with low value  
            var unexplored = (x: 1, y: 0); // up  
            visited[unexplored.y, unexplored.x] = false;
            grid.InitializePlane(unexplored.x, unexplored.y, 0);

            // Ensure at least one explored neighbor has the highest base value but loses due to +10 bonus  
            var explored = (x: 1, y: 2); // down  
            grid.InitializePlane(explored.x, explored.y, 2);

            // Act
            var result = _navigator.GetBestNeighbor(grid, pos, currentTime: 25, visited, scoreCalc);

            // Assert
            Assert.Equal(unexplored, result);
        }

        [Fact]
        public void WhenAllVisited_PicksHighestBaseScore()
        {
            // Arrange
            var grid = CreateRandomGrid(3, seed: 7);
            var visited = CreateVisited(grid, initial: true);
            var scoreCalc = new ScoreCalculator();

            var pos = (x: 1, y: 1);

            var neighbors = grid.GetNeighborCoordinates(pos.x, pos.y).ToList();
            var expected = neighbors
                .Select(n => (n, value: grid.GetPlane(n.x, n.y).Value))
                .OrderByDescending(p => p.value) // first max wins on tie  
                .First().n;

            // Act
            var result = _navigator.GetBestNeighbor(grid, pos, currentTime: 3, visited, scoreCalc);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TieOnScores_PicksFirstByDirectionOrder()
        {
            // Arrange
            var grid = CreateRandomGrid(3, seed: 77);
            var visited = CreateVisited(grid, initial: true);
            var scoreCalc = new ScoreCalculator();

            var pos = (x: 1, y: 1);

            // Set all neighbors to 0  
            foreach (var n in grid.GetNeighborCoordinates(pos.x, pos.y))
                grid.InitializePlane(n.x, n.y, 0);

            // Set two earliest neighbors to equal high score (2)  
            var firstByDir = (x: 0, y: 0); // (-1,-1)  
            var secondByDir = (x: 0, y: 1); // (-1, 0)  
            grid.InitializePlane(firstByDir.x, firstByDir.y, 2);
            grid.InitializePlane(secondByDir.x, secondByDir.y, 2);

            // Act
            var result = _navigator.GetBestNeighbor(grid, pos, currentTime: 1, visited, scoreCalc);

            // Assert
            Assert.Equal(firstByDir, result);
        }
    }
}
