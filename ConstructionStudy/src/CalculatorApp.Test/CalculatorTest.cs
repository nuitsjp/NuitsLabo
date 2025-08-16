namespace CalculatorApp.Test
{
    public class CalculatorTest
    {
        [Fact]
        public void Add_TwoPossitiveNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 5;
            int b = 3;
            int expected = 8;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_TwoNegativeNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();
            int a = -5;
            int b = -3;
            int expected = -8;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_PositiveAndNegativeNumbers_ReturnsCorrectSum()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 10;
            int b = -3;
            int expected = 7;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_ZeroAndNumber_ReturnsNumber()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 0;
            int b = 5;
            int expected = 5;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Add_MaxIntValues_DoesNotOverflow()
        {
            // Arrange
            var calculator = new Calculator();
            int a = int.MaxValue - 1;
            int b = 1;
            int expected = int.MaxValue;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
