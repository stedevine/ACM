using System;
using System.IO;
using System.Reflection;

namespace AntOnAChessboard
{
    class Program
    {
        // See problem description at http://web.cs.ucdavis.edu/~filkov/acm/ Grid problem #1
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "AntOnAChessboard.sampleInput.txt";

            // Read the whole file into a string. 
            // Trust that the input is valid (no error handling).
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                foreach (var input in result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    // Input values are in seconds, but the problem assumes one step per second, so treat the inputs
                    // as number of steps. 
                    var totalNumberOfSteps = int.Parse(input);
                    if (totalNumberOfSteps == 0)
                    {
                        break;
                    }
                    var point = Board.GetPointAfterStepsTaken(totalNumberOfSteps);
                    
                    // For each input time display the final position on the board.
                    Console.WriteLine("Time {0} Final position {1} {2}", totalNumberOfSteps, point.X, point.Y);
                }

                Console.ReadLine();
            }
        }
    }
}
