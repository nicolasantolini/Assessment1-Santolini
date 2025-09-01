using Assessment1_Santolini.Algorithms;
using Assessment1_Santolini.Display;
using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Navigation;
using Assessment1_Santolini.Simulation;
using Moq;

namespace TestProject
{
    public class DroneManagerTests
    {
        private readonly Mock<IScoreCalculator> _scoreCalculatorMock;
        private readonly Mock<IGridNavigator> _gridNavigatorMock;
        private readonly Mock<IPathRenderer> _pathRendererMock;
        private readonly DroneManager _droneManager;

        public DroneManagerTests()
        {
            _scoreCalculatorMock = new Mock<IScoreCalculator>();
            _gridNavigatorMock = new Mock<IGridNavigator>();
            _pathRendererMock = new Mock<IPathRenderer>();
            _droneManager = new DroneManager(
                _scoreCalculatorMock.Object,
                _gridNavigatorMock.Object,
                _pathRendererMock.Object
            );
        }

        [Fact]
        public void Constructor_ValidDependencies_InitializesSuccessfully()
        {
            // Act & Assert
            Assert.NotNull(_droneManager);
        }

        [Theory]
        [InlineData(10, 1, 1)] // Invalid grid size
        [InlineData(20, 0, 1)] // Invalid steps
        [InlineData(100, 1, 0)] // Invalid time limit
        public void GetOptimalPath_InvalidInputs_ThrowsArgumentException(int N, int t, int T)
        {
            // Arrange
            var grid = new Grid(N);
            var start = (0, 0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _droneManager.GetOptimalPath(grid, N, t, T, start));
        }

        [Fact]
        public void GetOptimalPath_InvalidStartPosition_ThrowsArgumentException()
        {
            // Arrange
            var grid = new Grid(20);
            var start = (25, 25); // Outside grid bounds

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _droneManager.GetOptimalPath(grid, 20, 10, 1000, start));
        }


    }
}