using System;
using System.Diagnostics;
using System.IO;
using WordLadderChallenge.Solvers;

namespace WordLadderChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                args = ProvideDefaultArguments();
            }

            if (HasValidArguments(args))
            {
                var dictionary = File.ReadAllLines(path: args[2]);
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
                    File.WriteAllLines(args[3], solution.Ladder);
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
            if (!File.Exists(args[2]))
            {
                Console.WriteLine("Path to dictionary is not valid");
                return false;
            }

            return true;
        }

        private static string[] ProvideDefaultArguments()
        {
            var solutionFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var pathToDictionary = Path.Combine(solutionFolder, @"Dictionary.txt");
            var pathToResults = Path.Combine(solutionFolder, @"Results.txt");
            return new string[] { "same", "cost", pathToDictionary, pathToResults };
        }
    }
}
