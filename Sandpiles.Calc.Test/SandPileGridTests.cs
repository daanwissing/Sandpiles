using NUnit.Framework;

namespace Sandpiles.Calc.Test
{
    public class SandPileGridTest
    {
        [Test]
        public void Test1()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();

            // Act
            var grid = sandPileGrid.Grid;

            // Assert
            Assert.That(grid.Length, Is.EqualTo(3));
            Assert.That(grid[0].Length, Is.EqualTo(3));
        }
    }
}