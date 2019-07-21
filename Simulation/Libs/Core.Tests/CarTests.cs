namespace Core.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;

    [TestClass]
    public class CarTests
    {
        [TestMethod]
        public void When_DrivingForward_Expect_StateAsForward()
        {
            // Arrange
            Car car = new Car();
            string expected = "Forward";

            // Act
            car.DriveForward();

            // Assert
            string actual = car.State;
            Assert.AreEqual(expected, actual, "Car state is not correct");
        }
    }
}
