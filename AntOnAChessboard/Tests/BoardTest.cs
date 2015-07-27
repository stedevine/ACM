namespace Tests
{
    using System;
    using AntOnAChessboard;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void GetStartPointShouldReturnFirstPointForOneStepTaken()
        {
            var result = Board.GetPointAfterStepsTaken(1);
            Assert.AreEqual(1, result.X, "X");
            Assert.AreEqual(1, result.Y, "Y");
        }

        [TestMethod]
        public void GetStartPointShouldThrowArgumentOutOfRangeExpectionForTotalStepsIsOutsideLimit()
        {
            Action<int> test = (n) =>
            {
                try
                {
                    Board.GetPointAfterStepsTaken(n);
                    Assert.Fail("Should throw argument out of range exception for value {0}", n);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Assert.AreEqual("totalStepsToTake", e.ParamName);
                }
            };

            test(-1);
            test(0);
            test(2000000001);
        }

        [TestMethod]
        public void GetPointAtTimeShouldReturnExpectedValuesForSampleInput()
        {
            Action<int, Point> test = (n, expected) =>
            {
                var result = Board.GetPointAfterStepsTaken(n);
                Assert.AreEqual(expected.X, result.X, "X");
                Assert.AreEqual(expected.Y, result.Y, "Y");
            };

            // Sample input from problem description.
            test(8, new Point { X = 2, Y = 3 });
            test(20, new Point { X = 5, Y = 4 });
            test(25, new Point { X = 1, Y = 5 });
        }

        [TestMethod]
        public void GetPointAtTimeShouldReturnExpectedValuesForMaximumInput()
        {
            // Max input = 2 * 10^9
            // floor of root of 2 * 10^9 = 44721
            // start position X = 1, Y = 44721 (left side of board)
            // steps to take from this position (2*10^9) - (44721)^2 = 32159
            // path is Up (up to 1 step), Right (up to 44721 steps), Down (remaining steps)
            // Up, then 32158 steps right
            // expected result X = 32159 Y = 44722
            var result = Board.GetPointAfterStepsTaken(2000000000);
            Assert.AreEqual(32159, result.X);
            Assert.AreEqual(44722, result.Y);
        }

        [TestMethod]
        public void GetStartPointShouldReturnFirstColumnValueForFloorSquareRootIsOdd()
        {
            Action<int> test = n =>
            {
                var result = Board.GetStartPoint(n);
                Assert.AreEqual(1, result.X, "X should be 1 for {0}", n);
            };

            foreach (var n in new[] { 9, 10, 15, 50 })
            {
                test(n);
            }
        }

        [TestMethod]
        public void GetStartPointShouldReturnFirstRowValueForFloorSquareRootIsEven()
        {
            Action<int> test = n =>
            {
                var result = Board.GetStartPoint(n);
                Assert.AreEqual(1, result.Y, "Y should be 1 for {0}", n);
            };

            foreach (var n in new[] { 4, 5, 17, 24 })
            {
                test(n);
            }
        }

        [TestMethod]
        public void GetReaminingStepsShouldReturnExpectedValueForLeftStartPoint()
        {
            Action<Point, int, int> test = (point, totalStepsToTake, expected) =>
            {
                var result = Board.GetRemainingSteps(point, totalStepsToTake);
                Assert.AreEqual(expected, result, "remaining steps should be {0} for point x={1}, y={2} totalSteps={3}", expected, point.X, point.Y, totalStepsToTake);
            };

            test(new Point { X = 1, Y = 1 }, 1, 0);
            test(new Point { X = 1, Y = 1 }, 2, 1);
            test(new Point { X = 1, Y = 7 }, 51, 2);
        }

        [TestMethod]
        public void GetReaminingStepsShouldReturnExpectedValueForBottomStartPoint()
        {
            Action<Point, int, int> test = (startPoint, totalStepsToTake, expected) =>
            {
                var result = Board.GetRemainingSteps(startPoint, totalStepsToTake);
                Assert.AreEqual(expected, result, "result should be {0} for point x={1}, y={2} totalSteps={3}", expected, startPoint.X, startPoint.Y, totalStepsToTake);
            };

            test(new Point { X = 2, Y = 1 }, 4, 0);
            test(new Point { X = 2, Y = 1 }, 5, 1);
            test(new Point { X = 6, Y = 1 }, 38, 2);
        }

        [TestMethod]
        public void GetPathForLeftStartingPointShouldReturnExpectedPaths()
        {
            Action<Point, int, Path> test = (startPoint, stepsRemaining, expected) =>
            {
                var result = Board.GetPathForLeftStartPoint(startPoint, stepsRemaining);
                Assert.AreEqual(expected.Up, result.Up, "Up result should be {0} for startPoint x={1}, y={2} stepsRemaining={3}", expected.Up, startPoint.X, startPoint.Y, stepsRemaining);
                Assert.AreEqual(expected.Down, result.Down, "Down result should be {0} for startPoint x={1}, y={2} stepsRemaining={3}", expected.Down, startPoint.X, startPoint.Y, stepsRemaining);
                Assert.AreEqual(expected.Left, result.Left, "Left result should be {0} for startPoint x={1}, y={2} stepsRemaining={3}", expected.Left, startPoint.X, startPoint.Y, stepsRemaining);
                Assert.AreEqual(expected.Right, result.Right, "Right result should be {0} for startPoint x={1}, y={2} stepsRemaining={3}", expected.Right, startPoint.X, startPoint.Y, stepsRemaining);
            };

            test(new Point { X = 1, Y = 3 }, 0, new Path());
            test(new Point { X = 1, Y = 3 }, 1, new Path { Up = 1 });
            test(new Point { X = 1, Y = 3 }, 2, new Path { Up = 1, Right = 1 });
            test(new Point { X = 1, Y = 3 }, 6, new Path { Up = 1, Right = 3, Down = 2 });
        }

        [TestMethod]
        public void GetPathForBotomStartingPointShouldReturnExpectedPaths()
        {
            Action<Point, int, Path> test = (startPoint, stepsRemaining, expected) =>
            {
                var result = Board.GetPathForBottomStartPoint(startPoint, stepsRemaining);
                Assert.AreEqual(expected.Up, result.Up, "Up result should be {0} for startPoint x={1}, y={2} stepsRemaining={3}", expected.Up, startPoint.X, startPoint.Y, stepsRemaining);
                Assert.AreEqual(expected.Down, result.Down, "Down result should be {0} for startPoint x={1}, y={2} stepsRemaining={3}", expected.Down, startPoint.X, startPoint.Y, stepsRemaining);
                Assert.AreEqual(expected.Left, result.Left, "Left result should be {0} for startPoint x={1}, y={2} stepsRemaining={3}", expected.Left, startPoint.X, startPoint.Y, stepsRemaining);
                Assert.AreEqual(expected.Right, result.Right, "Right result should be {0} for startPoint x={1}, y={2} stepsRemaining={3}", expected.Right, startPoint.X, startPoint.Y, stepsRemaining);
            };

            test(new Point { X = 4, Y = 1 }, 0, new Path());
            test(new Point { X = 4, Y = 1 }, 1, new Path { Right = 1 });
            test(new Point { X = 4, Y = 1 }, 2, new Path { Right = 1, Up = 1 });
            test(new Point { X = 4, Y = 1 }, 6, new Path { Right = 1, Up = 4, Left = 1 });
        }
    }
}