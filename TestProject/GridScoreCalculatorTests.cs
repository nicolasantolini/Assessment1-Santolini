using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;

namespace TestProject
{
    public class GridScoreCalculatorTests
    {
        [Fact]
        public void CalculatePlaneScore_NeverVisited_ReturnsBaseValue()
        {
            // Arrange
            var calculator = new ScoreCalculator(100);
            var plane = new Plane(1);
            // plane.LastVisitedTime is -1 by default

            // Act
            var score = calculator.CalculatePlaneScore(plane, 50);

            // Assert
            Assert.Equal(1, score);
        }

        [Fact]
        public void CalculatePlaneScore_JustVisited_ReturnsZero()
        {
            // Arrange
            var calculator = new ScoreCalculator(100);
            var plane = new Plane(2);
            plane.Visit(50); // Visited at time 50

            // Act
            var score = calculator.CalculatePlaneScore(plane, 50); // Not regenerated yet

            // Assert
            Assert.Equal(0, score); 
        }

        [Fact]
        public void CalculatePlaneScore_PartiallyRegenerated_ReturnsPartialValue()
        {
            // Arrange
            var calculator = new ScoreCalculator(100);
            var plane = new Plane(2);
            plane.Visit(0);

            // Act
            var score = calculator.CalculatePlaneScore(plane, 50); // Halfway regenerated

            // Assert
            Assert.Equal(1, score);
        }

        [Fact]
        public void CalculatePlaneScore_FullyRegenerated_ReturnsFullValue()
        {
            // Arrange
            var calculator = new ScoreCalculator(100);
            var plane = new Plane(2);
            plane.Visit(0);

            // Act
            var score = calculator.CalculatePlaneScore(plane, 110); // Fully regenerated

            // Assert
            Assert.Equal(2, score); 
        }
        [Fact]
        public void CalculatePlaneScore_NegativeCurrentTime_ThrowsException()
        {
            // Arrange
            var calculator = new ScoreCalculator();
            var plane = new Plane(100);
            plane.Visit(0);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => calculator.CalculatePlaneScore(plane, -1));
        }

        [Fact]
        public void CalculatePlaneScore_VisitedInFuture_ThrowsException()
        {
            // Arrange
            var calculator = new ScoreCalculator(100);
            var plane = new Plane(2);
            plane.Visit(100);

            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => calculator.CalculatePlaneScore(plane, 50));

            // Assert
            Assert.Equal("currentTime", ex.ParamName);
            Assert.Contains("Current time cannot be earlier than the plane's last visited time.", ex.Message);
        }

    }
}
