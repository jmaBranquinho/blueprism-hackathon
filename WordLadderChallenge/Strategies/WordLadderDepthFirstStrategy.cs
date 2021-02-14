﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Models;

namespace WordLadderChallenge.Strategies
{
    public class WordLadderDepthFirstStrategy : WordLadderStrategyBase
    {
        public override IEnumerable<string> Solve()
        {
            ValidInputParameters();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            OptimizeDictionaryToWordLength(SourceWord.Length);

            var wordLadderStepList = new List<WordLadderStep>() { GetWordLadderStepForSourceWord() };
            var result = ApplyRecursiveDfsAlgorithm(wordLadderStepList).FirstOrDefault();

            stopwatch.Stop();
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Solution found in {0} ms", stopwatch.Elapsed);
            }

            return result?.Ladder ?? Enumerable.Empty<string>();
        }

        private IEnumerable<WordLadderStep> ApplyRecursiveDfsAlgorithm(ICollection<WordLadderStep> wordLadders)
        {
            if (wordLadders is null || !wordLadders.Any())
            {
                return Enumerable.Empty<WordLadderStep>();
            }

            var nextIterationWordLadderStepList = new List<WordLadderStep>();

            foreach (var wordLadder in wordLadders)
            {
                var neighborList = Dictionary.Where(word => wordLadder.CurrentWord != word && HasOneCharacterDistance(word, wordLadder.CurrentWord)).ToList();

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