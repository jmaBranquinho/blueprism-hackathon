using System;
using System.Diagnostics;
using System.IO;
using WordLadderChallenge.Extensions.Utils;
using WordLadderChallenge.Services;
using WordLadderChallenge.Strategies;

namespace WordLadderChallenge
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                args = ProvideDefaultArguments();
            }

            try
            {
                var wordLadderSolver = new WordLadderBidirectionalSearchStrategy(new FileReadWriterService())
                {
                    SourceWord = args[0],
                    DestinationWord = args[1],
                    PathToDictionary = args[2],
                    PathToSolution = args[3],
                };
                var solution = wordLadderSolver.Solve();

                if (solution.IsNullOrEmpty())
                {
                    Console.WriteLine("No solution found");
                }
            } 
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
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