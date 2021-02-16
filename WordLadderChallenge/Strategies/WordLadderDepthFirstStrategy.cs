using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Extensions.Utils;
using WordLadderChallenge.Interfaces;
using WordLadderChallenge.Models.DepthFirstStrategy;

namespace WordLadderChallenge.Strategies
{
    public class WordLadderDepthFirstStrategy : WordLadderStrategyBase
    {
        public WordLadderDepthFirstStrategy(IFileReadWriterService fileReadWriterService) 
            : base(fileReadWriterService)
        {
        }

        protected override IEnumerable<string> ApplyAlgorithm()
        {
            var wordLadderStepList = new List<WordLadderStep>() { GetWordLadderStepForSourceWord() };
            return ApplyRecursiveDfsAlgorithm(wordLadderStepList)
                .FirstOrDefault()?.Ladder
                ?? Enumerable.Empty<string>();
        }

        private IEnumerable<WordLadderStep> ApplyRecursiveDfsAlgorithm(ICollection<WordLadderStep> wordLadders)
        {
            if (wordLadders.IsNullOrEmpty())
            {
                return Enumerable.Empty<WordLadderStep>();
            }

            var nextIterationWordLadderStepList = new List<WordLadderStep>();

            foreach (var wordLadder in wordLadders)
            {
                var neighborList = _dictionary.Where(word => wordLadder.CurrentWord != word && HasOneCharacterDistance(word, wordLadder.CurrentWord)).ToList();

                var containsLastWord = neighborList.Contains(DestinationWord);
                if (containsLastWord)
                {
                    wordLadder.Ladder.Add(DestinationWord);
                    return new List<WordLadderStep> { wordLadder };
                }
                else
                {
                    foreach (var neighbor in neighborList)
                    {
                        var newWordLadder = new WordLadderStep
                        {
                            Ladder = wordLadder.Ladder.Append(neighbor).ToList()
                        };
                        nextIterationWordLadderStepList.Add(newWordLadder);
                    }
                }
            }
            return ApplyRecursiveDfsAlgorithm(nextIterationWordLadderStepList);
        }

        private WordLadderStep GetWordLadderStepForSourceWord()
        {
            return new WordLadderStep { Ladder = new List<string> { SourceWord } };
        }
    }
}