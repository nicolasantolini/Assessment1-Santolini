using Assessment1_Santolini.Display;
using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;
using Moq;

namespace TestProject
{
    public class PathRendererTests : IDisposable
    {
        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOutput;
        private readonly PathRenderer _renderer;

        public PathRendererTests()
        {
            _originalOutput = Console.Out;
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
            _renderer = new PathRenderer();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }

        [Fact]
        public void PrintStart_OutputsCorrectInformation()
        {
            // Arrange
            var start = (5, 10);
            int maxSteps = 100;
            int timeLimitMs = 500;

            // Act
            _renderer.PrintStart(start, maxSteps, timeLimitMs);
            var output = _stringWriter.ToString();

            // Assert
            Assert.Contains($"Starting path at position ({start.Item1}, {start.Item2})", output);
            Assert.Contains($"Maximum steps: {maxSteps}", output);
            Assert.Contains($"Time limit: {timeLimitMs}ms", output);
        }

        [Fact]
        public void PrintStepHeader_OutputsCorrectInformation()
        {
            // Arrange
            int step = 5;
            long elapsedMs = 150;
            var currentPosition = (3, 7);
            int planeValue = 42;
            int score = 100;

            // Act
            _renderer.PrintStepHeader(step, elapsedMs, currentPosition, planeValue, score);
            var output = _stringWriter.ToString();

            // Assert
            Assert.Contains($"--- Step: {step} (Time elapsed: {elapsedMs}ms) ---", output);
            Assert.Contains($"Current position: ({currentPosition.Item1}, {currentPosition.Item2})", output);
            Assert.Contains($"Plane value: {planeValue}", output);
            Assert.Contains($"Current score: {score}", output);
        }


        [Fact]
        public void PrintSurroundingGrid_CornerPosition_OutputsCorrectGrid()
        {
            // Arrange
            var grid = new Grid(5);
            var calculatorMock = new Mock<IScoreCalculator>();
            calculatorMock.Setup(c => c.CalculatePlaneScore(It.IsAny<Plane>(), It.IsAny<int>()))
                         .Returns<int, int>((val, time) => val);

            int currentTime = 10;
            var currentPosition = (0, 0); // Top-left corner

            // Act
            _renderer.PrintSurroundingGrid(grid, calculatorMock.Object, currentTime, currentPosition);
            var output = _stringWriter.ToString();

            // Assert
            Assert.Contains("Surrounding grid values:", output);
            // Should show 3x3 grid (from 0,0 to 2,2)
            Assert.Contains("[  0]", output); // Corner plane should be highlighted
        }

        [Fact]
        public void PrintCompleteGrid_OutputsFullGrid()
        {
            // Arrange
            var grid = new Grid(2);
            var calculatorMock = new Mock<IScoreCalculator>();
            calculatorMock.Setup(c => c.CalculatePlaneScore(It.IsAny<Plane>(), It.IsAny<int>()))
                         .Returns(10);

            int currentTime = 10;
            var currentPosition = (1, 1);

            // Act
            _renderer.PrintCompleteGrid(grid, calculatorMock.Object, currentTime, currentPosition);
            var output = _stringWriter.ToString();

            // Assert
            Assert.Contains("Complete grid state:", output);
            Assert.Contains("-----", output); // Separator line
            Assert.Contains("[ 10]", output); // Highlighted plane
            Assert.Contains("  10 ", output); // Regular plane
        }

        [Fact]
        public void PrintTimeLimitReached_OutputsCorrectMessage()
        {
            // Arrange
            int timeLimitMs = 500;
            int stepsCompleted = 42;

            // Act
            _renderer.PrintTimeLimitReached(timeLimitMs, stepsCompleted);
            var output = _stringWriter.ToString();

            // Assert
            Assert.Contains($"Time limit of {timeLimitMs}ms reached after {stepsCompleted} steps", output);
        }

        [Fact]
        public void PrintMove_OutputsCorrectPosition()
        {
            // Arrange
            var nextPosition = (3, 7);

            // Act
            _renderer.PrintMove(nextPosition);
            var output = _stringWriter.ToString();

            // Assert
            Assert.Contains($"Moving to: ({nextPosition.Item1}, {nextPosition.Item2})", output);
        }

        [Fact]
        public void PrintCompletion_OutputsCorrectTime()
        {
            // Arrange
            long elapsedMs = 1500;

            // Act
            _renderer.PrintCompletion(elapsedMs);
            var output = _stringWriter.ToString();

            // Assert
            Assert.Contains($"Path finding completed in {elapsedMs}ms", output);
        }
    }
}