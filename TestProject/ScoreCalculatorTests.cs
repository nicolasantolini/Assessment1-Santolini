using Assessment1_Santolini.Entities;
using Assessment1_Santolini.Simulation;

namespace TestProject
{
    public class ScoreCalculatorTests
    {
        [Fact]
        public void ScoreCalc_Default_DoesNotThrow()
        {
            _ = new ScoreCalculator();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ScoreCalc_NonPositiveRegenerationTime_Throws(int regenerationTime)
        {
            var ex = Assert.Throws<ArgumentException>(() => new ScoreCalculator(regenerationTime));
            Assert.Equal("regenerationTime", ex.ParamName);
        }

        [Fact]
        public void CalculatePlaneScore_NegativeCurrentTime_Throws()
        {
            var calc = new ScoreCalculator(50);
            var plane = new Plane(2);

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => calc.CalculatePlaneScore(plane, -1));
            Assert.Equal("currentTime", ex.ParamName);
        }

        [Fact]
        public void CalculatePlaneScore_NullPlane_Throws()
        {
            var calc = new ScoreCalculator(50);
            var ex = Assert.Throws<ArgumentNullException>(() => calc.CalculatePlaneScore(null!, 10));
            Assert.Contains("Plane cannot be null.", ex.Message);
        }

        [Fact]
        public void CalculatePlaneScore_PlaneValueZeroOrNegative_ReturnsZero()
        {
            var calc = new ScoreCalculator(50);
            var planeZero = new Plane(0);
            int scoreZero = calc.CalculatePlaneScore(planeZero, 10);
            Assert.Equal(0, scoreZero);
        }
    }
}
