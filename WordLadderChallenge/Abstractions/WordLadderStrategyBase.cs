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
        public string SourceWord { get; set; }
        public string DestinationWord { get; set; }
        public string PathToDictionary { get; set; }
        public string PathToSolution { get; set; }

        protected ICollection<string> _dictionary;
        protected readonly IFileReadWriterService _fileReadWriterService;

        protected WordLadderStrategyBase(IFileReadWriterService fileReadWriterService)
        {
            _fileReadWriterService = fileReadWriterService;
        }

        /// <summary>
        /// Executes the algorithm after reading the dictionary and run some optimizations. Returns the word ladder solution if found.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Solve()
        {
            return ValidateExecuteAndTimeAlgorithm(() =>
            {
                return ApplyAlgorithm();
            });
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
        /// Removes words great or smaller than the provided length. Also turns all the words to lower case and removes the source word
        /// </summary>
        /// <param name="length"></param>
        protected virtual void OptimizeDictionaryToWordLength(int length)
        {
            _dictionary.Remove(SourceWord);
            _dictionary = new HashSet<string>(_dictionary.Where(word => word.Length == length).Select(word => word.ToLower()));
        }

        /// <summary>
        /// Executes dictionary optimizations, runs and times the algorithm. Returns the word ladder solution if found.
        /// </summary>
        /// <param name="executeAlgorithm"></param>
        /// <returns></returns>
        protected IEnumerable<string> ValidateExecuteAndTimeAlgorithm(Func<IEnumerable<string>> executeAlgorithm)
        {
            ReadDictionary();

            Guard.Against.NullOrWhiteSpace(SourceWord, nameof(SourceWord));
            Guard.Against.NullOrWhiteSpace(DestinationWord, nameof(DestinationWord));
            Guard.Against.InvalidWordLength(SourceWord, DestinationWord);
            Guard.Against.InvalidDictionary(_dictionary, new List<string> { SourceWord, DestinationWord });
            Guard.Against.InvalidFile(PathToSolution);

            var stopwatch = StartTimer();

            OptimizeWordCapitalizationForSearch();
            OptimizeDictionaryToWordLength(SourceWord.Length);

            var result = executeAlgorithm();

            StopTimer(stopwatch, result);
            WriteSolutionIfSucessful(result);

            return result;
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
                _fileReadWriterService.WriteToFile(PathToSolution, result);
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

        private void OptimizeWordCapitalizationForSearch()
        {
            SourceWord = SourceWord.ToLower();
            DestinationWord = DestinationWord.ToLower();
        }

        private void ReadDictionary()
        {
            _dictionary = _fileReadWriterService.GetAllFileLines(PathToDictionary);
        }

    }
}
