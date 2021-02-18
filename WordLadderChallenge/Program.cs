using System;
using WordLadderChallenge.Extensions.Utils;
using WordLadderChallenge.Services;
using WordLadderChallenge.Strategies;

namespace WordLadderChallenge
{
    static class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 4)
            {
                Console.WriteLine("Invalid number of arguments provided");
                return;
            }
            try
            {
                var pathToDictionary = args[2];
                var pathToSolution = args[3];

                var fileReadWriter = new FileReadWriterService
                {
                    ReadFilePath = pathToDictionary,
                    WriteFilePath = pathToSolution,
                };

                var wordLadderSolver = new WordLadderBidirectionalSearchStrategy(fileReadWriter);

                var solution = wordLadderSolver.Solve(sourceWord: args[0], destinationWord: args[1]);

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
    }
}