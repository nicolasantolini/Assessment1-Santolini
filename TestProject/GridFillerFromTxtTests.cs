using Assessment1_Santolini.Data;

namespace TestProject
{
    public class GridFillerFromTxtTests 
    {
        private readonly string _testDir;
        public GridFillerFromTxtTests()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\"));
            projectDirectory = projectDirectory.Replace("TestProject", "Assessment1-Santolini");
            _testDir = Path.Combine(projectDirectory, "GridsFiles");
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(99)]
        [InlineData(101)]
        [InlineData(999)]
        [InlineData(1001)]
        public void LoadGridFromFile_InvalidSize_ThrowsArgumentException(int size)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => GridFillerFromTxt.LoadGridFromFile(size));
            Assert.Contains("Invalid grid size", ex.Message);
        }


    }
}
