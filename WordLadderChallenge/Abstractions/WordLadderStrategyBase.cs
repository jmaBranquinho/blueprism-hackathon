using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordLadderChallenge.Extensions.Utils;
using WordLadderChallenge.Interfaces;

namespace WordLadderChallenge.Abstractions
{
    /// <summary>
    /// Contains common methods for all the word ladder solving strategies
    /// </summary>
    public abstract class WordLadderStrategyBase : IWordLadderStrategy
    {
        protected string SourceWord { get; private set; }

        protected string DestinationWord { get; private set; }

        protected ICollection<string> Dictionary { get; private set; }

        protected readonly IFileReadWriterService FileReadWriterService;

        private bool _isDictionaryOptimized;

        protected WordLadderStrategyBase(IFileReadWriterService fileReadWriterService)
        {
            FileReadWriterService = fileReadWriterService;
            _isDictionaryOptimized = false;
        }

        /// <summary>
        /// Executes the algorithm after reading the dictionary and run some optimizations. Returns the word ladder solution if found.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Solve(string sourceWord, string destinationWord)
        {
            Guard.Against.NullOrWhiteSpace(sourceWord, nameof(sourceWord));
            Guard.Against.NullOrWhiteSpace(destinationWord, nameof(destinationWord));
            Guard.Against.InvalidWordLength(sourceWord, destinationWord);

            SourceWord = sourceWord.ToLower();
            DestinationWord = destinationWord.ToLower();

            return ValidateExecuteAndTimeAlgorithm(sourceWord, destinationWord, () => {
                return ApplyAlgorithm();
            });
        }

        /// <summary>
        /// Updates the dictionary with the words provided by the FileReadWriterService
        /// </summary>
        public void ReadDictionary()
        {
            Dictionary = FileReadWriterService.GetAllFileLines();
            _isDictionaryOptimized = false;
        }

        /// <summary>
        /// Executes dictionary optimizations, runs and times the algorithm. Returns the word ladder solution if found.
        /// </summary>
        /// <param name="executeAlgorithm"></param>
        /// <returns></returns>
        protected IEnumerable<string> ValidateExecuteAndTimeAlgorithm(string sourceWord, string destinationWord, 
            Func<IEnumerable<string>> executeAlgorithm)
        {
            if(Dictionary.IsNullOrEmpty() || !_isDictionaryOptimized)
            {
                ReadAndOptimizeDictionaryToWordLength(sourceWord, destinationWord);
            }

            var stopwatch = StartTimer();

            var result = executeAlgorithm();

            StopTimer(stopwatch, result);

            WriteSolutionIfSucessful(result);

            return result;
        }

        /// <summary>
        /// Overridable method to implement a new strategy.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> ApplyAlgorithm()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the word distance is one character.
        /// </summary>
        /// <param name="firstWord"></param>
        /// <param name="secondWord"></param>
        /// <returns></returns>
        protected bool HasOneCharacterDistance(string firstWord, string secondWord)
        {
            return IsCharacterDistanceWithinLimit(firstWord, secondWord, limit: 1);
        }

        /// <summary>
        /// Checks if the word distance is the same as the limit provided.
        /// </summary>
        /// <param name="firstWord"></param>
        /// <param name="secondWord"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        protected bool IsCharacterDistanceWithinLimit(string firstWord, string secondWord, int limit)
        {
            Debug.Assert(firstWord.Length == secondWord.Length, $"IsCharacterDistanceWithinLimit: {firstWord} and {secondWord} do not have the same length");

            var differenceCounter = 0;

            for (int i = 0; i < firstWord.Length; i++)
            {
                if (firstWord[i] != secondWord[i])
                {
                    differenceCounter++;

                    if (differenceCounter > limit)
                    {
                        return false;
                    }
                }
            }

            return differenceCounter == limit;
        }

        private void WriteSolutionIfSucessful(IEnumerable<string> result)
        {
            var wasSuccessful = !result.IsNullOrEmpty();
            if (wasSuccessful)
            {
                FileReadWriterService.WriteToFile(result);
            }
        }

        private Stopwatch StartTimer()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            return stopwatch;
        }

        private void StopTimer(Stopwatch stopwatch, IEnumerable<string> result)
        {
            stopwatch.Stop();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Solution found in {0} ms", stopwatch.Elapsed.TotalMilliseconds);
                if (!result.IsNullOrEmpty())
                {
                    Console.WriteLine($"Solution is: {string.Join(" => ", result)}");
                }
            }
        }

        private void ReadAndOptimizeDictionaryToWordLength(string sourceWord, string destinationWord)
        {
            if (Dictionary.IsNullOrEmpty())
            {
                ReadDictionary();
            }
            Guard.Against.InvalidDictionary(Dictionary, new List<string> { sourceWord, destinationWord });

            Dictionary = new HashSet<string>(Dictionary.Where(word => word.Length == sourceWord.Length).Select(word => word.ToLower()));
            _isDictionaryOptimized = true;
        }

    }
}
