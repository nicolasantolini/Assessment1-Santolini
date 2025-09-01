using Assessment1_Santolini.Entities;

namespace TestProject
{
    public class PlaneTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(42)]
        [InlineData(int.MaxValue)]
        public void Plane_WithNonNegativeValue_SetsValue_AndInitialLastVisitedTimeIsMinusOne(int value)
        {
            // Act
            var plane = new Plane(value);

            // Assert
            Assert.Equal(-1, plane.LastVisitedTime);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Plane_WithNegativeValue_ThrowsArgumentOutOfRangeException_WithParamNameValue(int invalid)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Plane(invalid));
            Assert.Equal("value", ex.ParamName);
            Assert.Contains("Plane value cannot be negative.", ex.Message);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(123)]
        [InlineData(int.MaxValue)]
        public void Visit_WithNonNegativeTime_SetsLastVisitedTime(int time)
        {
            // Arrange
            var plane = new Plane(7);

            // Act
            plane.Visit(time);

            // Assert
            Assert.Equal(time, plane.LastVisitedTime);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Visit_WithNegativeTime_ThrowsArgumentOutOfRangeException_WithParamNameTime(int invalidTime)
        {
            // Arrange
            var plane = new Plane(5);

            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => plane.Visit(invalidTime));

            // Assert
            Assert.Equal("time", ex.ParamName);
            Assert.Contains("Time cannot be negative.", ex.Message);
        }

        [Fact]
        public void Visit_MultipleTimes_UpdatesToLatestTime_Increasing()
        {
            // Arrange
            var plane = new Plane(10);

            // Act & Assert (x3)
            plane.Visit(0);
            Assert.Equal(0, plane.LastVisitedTime);

            plane.Visit(5);
            Assert.Equal(5, plane.LastVisitedTime);

            plane.Visit(100);
            Assert.Equal(100, plane.LastVisitedTime);
        }

        [Fact]
        public void Visit_MultipleTimes_UpdatesToLatestTime_DecreasingNotAllowed()
        {
            // Arrange
            var plane = new Plane(10);

            // Act & Assert
            plane.Visit(100);
            Assert.Equal(100, plane.LastVisitedTime);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => plane.Visit(50));
            Assert.Equal("time", ex.ParamName);
            Assert.Contains("Visit time cannot be earlier than the last visited time.", ex.Message);

            Assert.Equal(100, plane.LastVisitedTime); // LastVisitedTime remains unchanged

        }
    }
}
