using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Exceptions;
using WordLadderChallenge.Models;

namespace WordLadderChallenge.Solvers
{
    public class WordLadderSolver
    {
        public string SourceWord { get; set; }
        public string DestinationWord { get; set; }
        public string PathToResultFile { get; set; }
        public ICollection<string> Dictionary { get; set; }

        public WordLadderStep Solve()
        {
            ValidInputParameters();

            Dictionary = OptimizeDictionaryToWordLength(Dictionary, SourceWord.Length);

            var currentWordLadderStep = new WordLadderStep { Word = SourceWord };

            return currentWordLadderStep;
        }

        private void ValidInputParameters()
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

        private ICollection<string> OptimizeDictionaryToWordLength(ICollection<string> dictionary, int length)
        {
            return dictionary.Where(word => word.Length == length).ToList();
        }
    }
}