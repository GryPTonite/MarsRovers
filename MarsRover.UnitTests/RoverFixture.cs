using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarsRover.Lib;
using NUnit.Framework;
using Moq;

namespace MarsRover.UnitTests
{
    [TestFixture]
    public class RoverFixture
    {
        private Mock<IPosition> _position = null;
        private Mock<IPlateau> _pleateau = null;
        private IRover _roverOne = null;
        private IRover _roverTwo = null;
        private IRover _roverThree = null;

        [SetUp]
        public void Setup()
        {
            _pleateau = new Mock<IPlateau>();
            _position = new Mock<IPosition>();
        }

        private void SharedSetupRover(ref IRover rover, bool? allowBoundaryRiders, int startX, int startY, Orientations orientation)
        {
            //enable property tracking
            _position.SetupProperty(x => x.X, startX);
            _position.SetupProperty(x => x.Y, startY);
            _pleateau.Setup(x => x.Boundary).Returns(new Position(5, 5));
            _pleateau.SetupProperty(x => x.AllowOutsideBoundary, allowBoundaryRiders);
            rover = new Rover(_position.Object, orientation,_pleateau.Object);
        }

        private void SetupRoverOne()
        {
            SharedSetupRover(ref _roverOne, true, 1, 2, Orientations.N);
        }

        private void SetupRoverTwo()
        {
            SharedSetupRover(ref _roverTwo, true, 3, 3, Orientations.E);
        }

        private void SetupRoverThree(bool? allowOutOfBounds)
        {
            SharedSetupRover(ref _roverThree, allowOutOfBounds, 0, 0, Orientations.W);
        }

        [Test]
        [Category("Positive")]
        public void Move_Rover_Outside_Borders()
        {
            //Arrange
            SetupRoverOne();

            //Act
            _roverOne.Process("MMRMMRMRRMMRRMRMRMMMMMMRMMMM");

            //Assert
            string result = _roverOne.ToString();
            Console.WriteLine(result);
            Assert.IsTrue(result.Contains("Rover outside the plateau"));
        }

        [Test]
        [Category("Positive")]
        public void Move_RoverOne_Check_Output()
        {
            //Arrange
            SetupRoverOne();

            //Act
            _roverOne.Process("LMLMLMLMM");

            //Assert
            string result = _roverOne.ToString();
            Console.WriteLine(result);
            Assert.AreEqual(result, "1 3 N");
        }

        [Test]
        [Category("Positive")]
        public void Move_RoverTwo_Check_Output()
        {
            //Arrange
            SetupRoverTwo();

            //Act
            _roverTwo.Process("MMRMMRMRRM");

            //Assert
            string result = _roverTwo.ToString();
            Console.WriteLine(result);
            Assert.AreEqual(result, "5 1 E");
        }

        [Test]
        [Category("Exceptions")]
        public void Move_RoverOne_Incorrect_Input()
        {
            //Arrange
            SetupRoverOne();

            //Act //Assert
            var ex = Assert.Throws<System.ArgumentException>(() => _roverOne.Process("LMLMLMFLMM"));
            Assert.IsTrue(ex.Message.StartsWith("Invalid value"));
        }

        [Test]
        [Category("Exceptions")]
        public void Move_RoverThree_OutsideBordersWithNotAllow()
        {
            //Arrange //Act
            SetupRoverThree(null);

            //Act 
            Assert.Throws<System.InvalidOperationException>(() => _roverThree.Process("MRM"));

            //Assert - RoverThree has not moved
            string result = _roverThree.ToString();
            Console.WriteLine(result);
            Assert.AreEqual(result, "0 0 W");
        }

        [Test]
        [Category("Positive")]
        public void Move_RoverThree_OutsideBordersWithAllow()
        {
            //Arrange //Act
            SetupRoverThree(false);

            //Act 
            _roverThree.Process("MRM");

            //Assert - RoverThree has moved North. However, the first move to move west has been ignored because it is outside boundary
            string result = _roverThree.ToString();
            Console.WriteLine(result);
            Assert.AreEqual(result, "0 1 N");
        }
    }
}
