using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordLadderChallenge.Exceptions;
using WordLadderChallenge.Models;

namespace WordLadderChallenge.Solvers
{
    public class WordLadderSolver
    {
        public string SourceWord { get; set; }
        public string DestinationWord { get; set; }
        public ICollection<string> Dictionary { get; set; }

        public WordLadderStep Solve()
        {
            ValidInputParameters();

            OptimizeDictionaryToWordLength(SourceWord.Length);

            var wordLadderStepList = new List<WordLadderStep>() { GetWordLadderStepForSourceWord() };

            bool wasDestinationFound;
            do
            {
                wasDestinationFound = FindNextLadderStep(wordLadderStepList);
            }
            while (!wasDestinationFound && wordLadderStepList.Any());

            return wordLadderStepList.FirstOrDefault();
        }

        private bool FindNextLadderStep(List<WordLadderStep> wordLadderStepList)
        {
            var allWordLadderStepsCopy = wordLadderStepList.ToList();
            wordLadderStepList.Clear();

            foreach (var wordLadder in allWordLadderStepsCopy)
            {
                var neighborList = Dictionary.Where(word => wordLadder.CurrentWord != word && HasOneCharacterDistance(word, wordLadder.CurrentWord)).ToList();

                var ladderContainsLastWord = neighborList.Contains(DestinationWord);
                if (ladderContainsLastWord)
                {
                    wordLadderStepList.Clear();
                    wordLadder.Ladder.Add(DestinationWord);
                    wordLadderStepList.Add(wordLadder);
                    return true;
                }
                else
                {
                    foreach (var neighbor in neighborList)
                    {
                        var newWordLadderStep = new WordLadderStep
                        {
                            Ladder = wordLadder.Ladder.Append(neighbor).ToList()
                        };
                        wordLadderStepList.Add(newWordLadderStep);
                    }
                }
            }

            return false;
        }

        private bool HasOneCharacterDistance(string firstWord, string secondWord)
        {
            return IsCharacterDistanceWithinLimit(firstWord, secondWord, limit: 1);
        }

        private bool IsCharacterDistanceWithinLimit(string firstWord, string secondWord, int limit)
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

        private WordLadderStep GetWordLadderStepForSourceWord()
        {
            return new WordLadderStep { Ladder = new List<string> { SourceWord } };
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

        private void OptimizeDictionaryToWordLength(int length)
        {
            Dictionary = Dictionary.Where(word => word.Length == length).ToList();
        }
    }
}