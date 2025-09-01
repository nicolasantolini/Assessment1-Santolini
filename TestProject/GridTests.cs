using Assessment1_Santolini.Entities;

namespace TestProject
{
    public class GridTests
    {

        [Fact]
        public void Constructor_WithPositiveSize_SetsSize()
        {
            // Act
            var grid = new Grid(3);

            // Assert
            Assert.Equal(3, grid.Size);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(int.MinValue)]
        public void Constructor_WithNonPositiveSize_ThrowsArgumentException(int size)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Grid(size));
            Assert.Equal("size", ex.ParamName);
        }

        [Fact]
        public void InitializePlane_WithValidCoordinates_SetsPlane()
        {
            // Arrange
            var grid = new Grid(2);

            grid.InitializePlane(1, 0, baseValue: 42);

            // Act
            var plane = grid.GetPlane(1, 0);

            // Assert
            Assert.NotNull(plane);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(2, 0)]
        [InlineData(0, 2)]
        public void InitializePlane_WithInvalidCoordinates_Throws(int x, int y)
        {
            // Arrange
            var grid = new Grid(2);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.InitializePlane(x, y, 1));
        }

        [Fact]
        public void GetPlane_BeforeInitialization_ReturnsNull()
        {
            // Arrange
            var grid = new Grid(2);

            // Act
            var plane = grid.GetPlane(0, 0);

            // Assert
            Assert.Null(plane);
        }

        [Fact]
        public void GetPlane_ReturnsSameInstance_OnMultipleGets()
        {
            // Arrange
            var grid = new Grid(3);
            grid.InitializePlane(2, 2, 7);

            // Act
            var first = grid.GetPlane(2, 2);
            var second = grid.GetPlane(2, 2);

            // Assert
            Assert.Same(first, second);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(3, 0)]
        [InlineData(0, 3)]
        public void GetPlane_WithInvalidCoordinates_Throws(int x, int y)
        {
            // Arrange
            var grid = new Grid(3);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetPlane(x, y));
        }

        [Theory]
        [InlineData(3, 0, 0, true)]
        [InlineData(3, 2, 2, true)]
        [InlineData(3, 1, 2, true)]
        [InlineData(3, -1, 0, false)]
        [InlineData(3, 0, -1, false)]
        [InlineData(3, 3, 0, false)]
        [InlineData(3, 0, 3, false)]
        public void IsValidCoordinate_ReturnsExpected(int size, int x, int y, bool expected)
        {
            // Arrange 
            var grid = new Grid(size);

            // Act & Assert
            Assert.Equal(expected, grid.IsValidCoordinate(x, y));
        }

        [Fact]
        public void GetNeighborCoordinates_CenterPlane_ReturnsEightNeighbors()
        {
            // Arrange
            var grid = new Grid(3);

            // Act
            var neighbors = grid.GetNeighborCoordinates(1, 1).ToHashSet();

            var expected = new HashSet<(int x, int y)>
                {
                    (0,0),(0,1),(0,2),
                    (1,0),       (1,2),
                    (2,0),(2,1),(2,2)
                };

            // Assert
            Assert.Equal(8, neighbors.Count);
            Assert.Subset(expected, neighbors);
            Assert.DoesNotContain((1, 1), neighbors);
        }

        [Fact]
        public void GetNeighborCoordinates_ReturnsOnlyValidCoordinates()
        {
            // Arrange
            var grid = new Grid(3);

            // Act
            var neighbors = grid.GetNeighborCoordinates(1, 1);

            // Assert
            Assert.All(neighbors, n => Assert.True(grid.IsValidCoordinate(n.x, n.y)));
        }

        [Fact]
        public void GetNeighborCoordinates_InvalidStart_Throws()
        {
            // Arrange
            var grid = new Grid(3);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => grid.GetNeighborCoordinates(-1, 0).ToList());
        }

    }
}
