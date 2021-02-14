using System;
using System.IO;
using WordLadderChallenge.Solvers;

namespace WordLadderChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            if(HasValidArguments(args))
            {
                var dictionary = File.ReadAllLines(path: args[3]);
                var wordLadderSolver = new WordLadderSolver()
                {
                    SourceWord = args[0],
                    DestinationWord = args[1],
                    Dictionary = dictionary,
                };
                var solution = wordLadderSolver.Solve();
                if(solution is null)
                {
                    Console.WriteLine("No solution found");
                } else
                {
                    File.WriteAllLines(args[4], solution.Ladder);
                }
            }
        }

        private static bool HasValidArguments(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Please provide the required arguments");
                return false;
            }
            if (string.IsNullOrWhiteSpace(args[0]) || string.IsNullOrWhiteSpace(args[1]))
            {
                Console.WriteLine("Source and/or destination word is not valid");
                return false;
            }
            if (!File.Exists(args[3]) || !Uri.IsWellFormedUriString(args[4], UriKind.RelativeOrAbsolute))
            {
                Console.WriteLine("Path to dictionary and/or path to result file is not valid");
                return false;
            }

            return true;
        }
    }
}
