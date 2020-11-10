using NUnit.Framework;

namespace Sandpiles.Calc.Test
{
    public class SandPileGridTest
    {
        [Test]
        public void WhenNewGrid_GridInitialized()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();

            // Act
            var grid = sandPileGrid.Grid;

            // Assert
            Assert.That(grid.Length, Is.EqualTo(3));
            Assert.That(grid[0].Length, Is.EqualTo(3));
        }

        [Test]
        public void GivenDimensions_WhenNewGrid_ThenGridHasGivenDimensions()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid(6,9);

            // Act
            var grid = sandPileGrid.Grid;

            // Assert
            Assert.That(grid.Length, Is.EqualTo(6));
            Assert.That(grid[0].Length, Is.EqualTo(9));
        }

        [Test]
        public void GivenEmptyGrid_WhenCollapse_ThenGridIsSame()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid(6,9);
            var copy = sandPileGrid.Grid;

            // Act
            sandPileGrid.Collapse();

            // Assert
            var result = sandPileGrid.Grid;
            for(int i = 0; i < copy.Length; i++)
            {
                for (int j = 0; j < copy[i].Length; j++)
                {
                    Assert.That(copy[i][j], Is.EqualTo(result[i][j]));
                }
            }
        }

        [Test]
        public void Given4SandInCell_WhenCollapse_ThenSandIsDistributed()
        {
                        // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[1][1] = 4;

            // Act
            sandPileGrid.Collapse();

            // Assert
            Assert.That(sandPileGrid.Grid[0][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][0], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][2], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[2][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(0));
        }
    }
}