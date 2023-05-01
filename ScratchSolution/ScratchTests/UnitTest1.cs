namespace ScratchTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            
        }

        [Fact]
        public void CanAddTwoNumbers() 
        {
            // "Given" - "Arrange"
            // You have to create the world from scratch here.
            int a = 10, b = 20, answer;

            // "When" - "Act"
            answer = a + b; // System Under Test (SUT)

            // "Then" - "Assert"
            Assert.Equal(30, answer);
        }

        [Theory]
        [InlineData(10, 20, 30)] // another "Given"
        [InlineData(2, 2, 4)]
        [InlineData(10, 5, 15)]
        public void CanAddOtherIntegers(int a, int b, int expected)
        {
            int answer = a + b;
            Assert.Equal(expected, answer);
        }

    }
}