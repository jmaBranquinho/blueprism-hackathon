using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Extensions.Utils;
using WordLadderChallenge.Interfaces;
using WordLadderChallenge.Models.BidirectionalSearchStrategy;

namespace WordLadderChallenge.Strategies
{
    public class WordLadderBidirectionalSearchStrategy : WordLadderStrategyBase
    {
        public WordLadderBidirectionalSearchStrategy(IFileReadWriterService fileReadWriterService) 
            : base(fileReadWriterService)
        {
        }

        protected override IEnumerable<string> ApplyAlgorithm()
        {
            return ApplyBdsAlgorithm();
        }

        private IEnumerable<string> ApplyBdsAlgorithm()
        {
            var (sourceIterationData, destinationIterationData) = GetSourceAndDestinationBdsIterationData();

            var iterationDataList = new List<WordLadderBdsIterationData>
            {
                sourceIterationData,
                destinationIterationData
            };

            Debug.Assert(!sourceIterationData.WordLadderQueue.IsNullOrEmpty() 
                && !destinationIterationData.WordLadderQueue.IsNullOrEmpty(), 
                $"One or more queues already start empty or null");
            Debug.Assert(!iterationDataList.IsNullOrEmpty(), $"Iteration data is null or empty");

            while (sourceIterationData.WordLadderQueue.Any() && destinationIterationData.WordLadderQueue.Any())
            {
                foreach (var iterationData in iterationDataList)
                {
                    var currentNode = iterationData.WordLadderQueue.Dequeue();

                    foreach (var dictionaryWord in Dictionary)
                    {
                        var isAdjacentAndNotVisited = HasOneCharacterDistance(currentNode.Word, dictionaryWord) && !iterationData.VisitedNodeList.ContainsKey(dictionaryWord);
                        if (isAdjacentAndNotVisited)
                        {
                            var unvisitedNode = AddUnvisitedWordToQueueAndVisitedList(iterationData, currentNode, dictionaryWord);

                            var hasMatchInOpositeIteration = iterationData.OpositeIterationData.VisitedNodeList.ContainsKey(unvisitedNode.Word);
                            if (hasMatchInOpositeIteration)
                            {
                                var wordLadder = BackTrack(iterationData, unvisitedNode);
                                return wordLadder;
                            }
                        }
                    }
                }
            }
            return Enumerable.Empty<string>();
        }

        private static WordNode AddUnvisitedWordToQueueAndVisitedList(WordLadderBdsIterationData iterationData, WordNode currentNode, string dictionaryWord)
        {
            var nextLevel = currentNode.Level + 1;
            var unvisitedNode = new WordNode
            {
                Word = dictionaryWord,
                Level = nextLevel,
            };
            iterationData.WordLadderQueue.Enqueue(unvisitedNode);
            iterationData.VisitedNodeList.Add(dictionaryWord, nextLevel);
            return unvisitedNode;
        }

        private List<string> BackTrack(WordLadderBdsIterationData iterationData, WordNode matchingNode)
        {
            iterationData = MirrorIterationDataIfOrderedFromStartToEnd(iterationData);

            var wordLadder = new List<string>();

            wordLadder.AddRange(BacktrackAndGetNodes(iterationData.OpositeIterationData, level: matchingNode.Level).Reverse());

            wordLadder.Add(matchingNode.Word);

            wordLadder.AddRange(BacktrackAndGetNodes(iterationData, level: iterationData.OpositeIterationData.VisitedNodeList[matchingNode.Word]));

            return wordLadder;
        }

        private WordLadderBdsIterationData MirrorIterationDataIfOrderedFromStartToEnd(WordLadderBdsIterationData iterationData)
        {
            var isIterationFromEndToStart = iterationData.StartingNode.Word == DestinationWord;
            if (!isIterationFromEndToStart)
            {
                iterationData = iterationData.OpositeIterationData;
            }

            return iterationData;
        }

        private ICollection<string> BacktrackAndGetNodes(WordLadderBdsIterationData iterationData, int level)
        {
            var halfWordLadder = new List<string>();
            for (int currentLevel = 1; currentLevel <= level; currentLevel++)
            {
                halfWordLadder.Add(GetPreviousWordNode(iterationData, currentLevel));
            }

            return halfWordLadder;
        }

        private string GetPreviousWordNode(WordLadderBdsIterationData iterationData, int currentLevel)
        {
            return iterationData.VisitedNodeList.First(
                word1 => iterationData.OpositeIterationData.VisitedNodeList.Any(
                    word2 => IsCharacterDistanceWithinLimit(word1.Key, word2.Key, currentLevel))).Key;
        }

        private (WordLadderBdsIterationData, WordLadderBdsIterationData) GetSourceAndDestinationBdsIterationData()
        {
            var sourceIterationData = GetBdsIterationData(SourceWord);
            var destinationIterationData = GetBdsIterationData(DestinationWord);

            sourceIterationData.OpositeIterationData = destinationIterationData;
            destinationIterationData.OpositeIterationData = sourceIterationData;

            return (sourceIterationData, destinationIterationData);
        }

        private WordLadderBdsIterationData GetBdsIterationData(string word)
        {
            var sourceIterationData = new WordLadderBdsIterationData
            {
                StartingNode = new WordNode
                {
                    Word = word,
                    Level = 0,
                },
                WordLadderQueue = new Queue<WordNode>(),
                VisitedNodeList = new Dictionary<String, int>() { { word, 0 } },
            };
            sourceIterationData.WordLadderQueue.Enqueue(sourceIterationData.StartingNode);
            return sourceIterationData;
        }

    }
}
