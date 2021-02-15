using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordLadderChallenge.Exceptions;

namespace WordLadderChallenge.Abstractions
{
    public abstract class WordLadderStrategyBase : IWordLadderStrategy
    {
        public string SourceWord { get; set; }
        public string DestinationWord { get; set; }
        public ICollection<string> Dictionary { get; set; }

        public virtual IEnumerable<string> Solve()
        {
            throw new System.NotImplementedException();
        }

        protected IEnumerable<string> ValidateExecuteAndTimeAlgorithm(Func<IEnumerable<string>> executeAlgorithm)
        {
            ValidInputParameters();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            SourceWord = SourceWord.ToLower();
            DestinationWord = DestinationWord.ToLower();
            OptimizeDictionaryToWordLength(SourceWord.Length);

            var result = executeAlgorithm();

            stopwatch.Stop();
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Solution found in {0} ms", stopwatch.Elapsed.TotalMilliseconds);
                if(!(result is null) && result.Any())
                {
                    Console.WriteLine($"Solution is: {string.Join(" => ", result)}");
                }                
            }

            return result;
        }

        protected bool HasOneCharacterDistance(string firstWord, string secondWord)
        {
            return IsCharacterDistanceWithinLimit(firstWord, secondWord, limit: 1);
        }

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

            return true;
        }

        protected void ValidInputParameters()
        {
            if (string.IsNullOrWhiteSpace(SourceWord) || string.IsNullOrWhiteSpace(DestinationWord))
            {
                throw new InvalidWordException();
            }
            if (SourceWord.Length != DestinationWord.Length)
            {
                throw new WordLengthMismatchException(SourceWord, DestinationWord);
            }
            var isDictionaryEmpty = Dictionary == null || !Dictionary.Any();
            var isValidDictionary = !isDictionaryEmpty && Dictionary.Contains(SourceWord) && Dictionary.Contains(DestinationWord);
            if (!isValidDictionary)
            {
                throw new InvalidDictionaryException();
            }
        }

        protected void OptimizeDictionaryToWordLength(int length)
        {
            Dictionary.Remove(SourceWord);
            Dictionary = new HashSet<string>(Dictionary.Where(word => word.Length == length).Select(word => word.ToLower()));
        }

    }
}
