namespace AntOnAChessboard
{
    using System;

    public static class Board
    {
        public static Point GetPointAfterStepsTaken(int totalStepsToTake)
        {
            // Input limits from problem description.
            if (totalStepsToTake <= 0 || totalStepsToTake > 2000000000)
            {
                throw new ArgumentOutOfRangeException("totalStepsToTake");
            }

            var startPoint = GetStartPoint(totalStepsToTake);

            Path path;

            if (IsLeftStartPoint(startPoint))
            {
                // Starting point is on the left side of the board.
                path = GetPathForLeftStartPoint(startPoint, GetRemainingSteps(startPoint, totalStepsToTake));
            }
            else
            {
                // Starting point is on the bottom of the board.
                path = GetPathForBottomStartPoint(startPoint, GetRemainingSteps(startPoint, totalStepsToTake));
            }

            return new Point
            {
                X = startPoint.X - path.Left + path.Right,
                Y = startPoint.Y - path.Down + path.Up
            };
        }

        internal static Point GetStartPoint(int totalStepsToTake)
        {
            // By inspecting the path taken we notice that:
            // The values in the first column (X = 1) are square numbers for Y is odd.
            // The values in the first row (Y = 1) are square for X is even.

            // Finding the floor of the root of the time will give us a good starting position.
            var root = (int) (Math.Sqrt(totalStepsToTake));

            if (IsEven(root))
            {
                return new Point { X = root, Y = 1 };
            }

            return new Point { X = 1, Y = root };
        }

        private static bool IsEven(int value)
        {
            return (value % 2 == 0);
        }

        internal static int GetRemainingSteps(Point startPoint, int totalStepsToTake)
        {
            // At the start point we will have taken some square number of steps. 
            // If we're on the left side of the board we have taken Y squared steps.
            // If we're on the bottom of the board we have taken X squared steps.
            if (IsLeftStartPoint(startPoint))
            {
                return totalStepsToTake - (startPoint.Y * startPoint.Y);
            }

            return totalStepsToTake - (startPoint.X * startPoint.X);
        }

        private static bool IsLeftStartPoint(Point startPoint)
        {
            // By inspecting the board and considering the path taken we notice 
            // we need to treat 1,1 (bottom left position) as "left" rather than bottom.
            return (startPoint.X == 1);
        }

        internal static Path GetPathForLeftStartPoint(Point start, int stepsRemaining)
        {
            // The path for starting on the left side of the board is Up, Right, then Down.
            var result = new Path();
            if (stepsRemaining > 0)
            {
                result.Up = 1;
                stepsRemaining--;
                if (stepsRemaining > 0)
                {
                    result.Right = stepsRemaining > start.Y ? start.Y : stepsRemaining;
                    stepsRemaining = stepsRemaining - result.Right;
                    if (stepsRemaining > 0)
                    {
                        result.Down = stepsRemaining;
                    }
                }
            }

            return result;
        }

        internal static Path GetPathForBottomStartPoint(Point start, int stepsRemaining)
        {
            // The path for starting on the bottom of the board is Right, Up, then Left.
            var result = new Path();
            if (stepsRemaining > 0)
            {
                result.Right = 1;
                stepsRemaining--;
                if (stepsRemaining > 0)
                {
                    result.Up = stepsRemaining > start.X ? start.X : stepsRemaining;
                    stepsRemaining = stepsRemaining - result.Up;
                    if (stepsRemaining > 0)
                    {
                        result.Left = stepsRemaining;
                    }
                }
            }

            return result;
        }
    }
}