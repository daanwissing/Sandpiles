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
            var sandPileGrid = new SandPileGrid(6, 9);

            // Act
            var grid = sandPileGrid.Grid;

            // Assert
            Assert.That(grid.Length, Is.EqualTo(6));
            Assert.That(grid[0].Length, Is.EqualTo(9));
        }

        [Test]
        public void GivenEmptyGrid_WhenTopple_ThenGridIsSame()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid(6, 9);
            var copy = sandPileGrid.Grid;

            // Act
            sandPileGrid.Topple();

            // Assert
            var result = sandPileGrid.Grid;
            for (int i = 0; i < copy.Length; i++)
            {
                for (int j = 0; j < copy[i].Length; j++)
                {
                    Assert.That(copy[i][j], Is.EqualTo(result[i][j]));
                }
            }
        }

        [Test]
        public void Given4SandInMiddleCell_WhenTopple_ThenSandIsDistributed()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[1][1] = 4;

            // Act
            sandPileGrid.Topple();

            // Assert
            Assert.That(sandPileGrid.Grid[0][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][0], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][2], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[2][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(0));
        }

        [Test]
        public void Given8SandInMiddleCell_WhenTopple_ThenSandIsDistributed()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[1][1] = 8;

            // Act
            sandPileGrid.Topple();

            // Assert
            Assert.That(sandPileGrid.Grid[0][1], Is.EqualTo(2));
            Assert.That(sandPileGrid.Grid[1][0], Is.EqualTo(2));
            Assert.That(sandPileGrid.Grid[1][2], Is.EqualTo(2));
            Assert.That(sandPileGrid.Grid[2][1], Is.EqualTo(2));
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(0));
        }


        [Test]
        public void Given4SandInAdjacentCell_WhenTopple_ThenSandIsDistributed()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[1][1] = 4;
            sandPileGrid.Grid[1][2] = 4;

            // Act
            sandPileGrid.Topple();

            // Assert
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][2], Is.EqualTo(1));
        }


        [Test]
        public void Given4SandInTopRight_WhenTopple_ThenSandIsDistributed()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[0][0] = 4;

            // Act
            sandPileGrid.Topple();

            // Assert
            Assert.That(sandPileGrid.Grid[0][0], Is.EqualTo(0));
            Assert.That(sandPileGrid.Grid[1][0], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[0][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(0));
        }

        [Test]
        public void Given4SandInBottomLeft_WhenTopple_ThenSandIsDistributed()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[2][2] = 4;

            // Act
            sandPileGrid.Topple();

            // Assert
            Assert.That(sandPileGrid.Grid[2][2], Is.EqualTo(0));
            Assert.That(sandPileGrid.Grid[1][2], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[2][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(0));
        }

        [Test]
        public void Given4Sand_WhenTopple_ThenChangedIsTrue()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[2][2] = 4;

            // Act
            bool changed = sandPileGrid.Topple();

            // Assert
            Assert.That(changed);
        }

        [Test]
        public void Given3Sand_WhenTopple_ThenChangedIsFalse()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[2][2] = 3;

            // Act
            bool changed = sandPileGrid.Topple();

            // Assert
            Assert.That(changed, Is.EqualTo(false));
        }
        [Test]
        public void Given4SandInMiddleCell_WhenToppleInPlace_ThenSandIsDistributed()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[1][1] = 4;

            // Act
            sandPileGrid.ToppleInPlace();

            // Assert
            Assert.That(sandPileGrid.Grid[0][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][0], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][2], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[2][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(0));
        }

        [Test]
        public void Given8SandInMiddleCell_WhenToppleInPlace_ThenSandIsDistributed()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[1][1] = 8;

            // Act
            sandPileGrid.ToppleInPlace();

            // Assert
            Assert.That(sandPileGrid.Grid[0][1], Is.EqualTo(2));
            Assert.That(sandPileGrid.Grid[1][0], Is.EqualTo(2));
            Assert.That(sandPileGrid.Grid[1][2], Is.EqualTo(2));
            Assert.That(sandPileGrid.Grid[2][1], Is.EqualTo(2));
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(0));
        }


        [Test]
        public void Given4SandInAdjacentCell_WhenToppleInPlace_ThenSandIsDistributed()
        {
            // Arrange
            var sandPileGrid = new SandPileGrid();
            sandPileGrid.Grid[1][1] = 4;
            sandPileGrid.Grid[1][2] = 4;

            // Act
            sandPileGrid.ToppleInPlace();

            // Assert
            Assert.That(sandPileGrid.Grid[1][1], Is.EqualTo(1));
            Assert.That(sandPileGrid.Grid[1][2], Is.EqualTo(1));
        }
    }
}