using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Models;

namespace WordLadderChallenge.Strategies
{
    public class WordLadderBidirectionalSearchStrategy : WordLadderStrategyBase
    {
        public override IEnumerable<string> Solve()
        {
            ValidInputParameters();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            OptimizeDictionaryToWordLength(SourceWord.Length);

            var wordLadder = ApplyBdsAlgorithm();

            stopwatch.Stop();
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Solution found in {0} ms", stopwatch.Elapsed);
            }

            return wordLadder;
        }

        public IEnumerable<string> ApplyBdsAlgorithm()
        {
            var sourceIterationData = new WordLadderBdsIterationData
            {
                Node = new Node 
                {
                    Word = SourceWord,
                    Level = 1,
                },
                WordLadderQueue = new Queue<Node>(),
                VisitedNodeList = new Dictionary<String, int>() { { SourceWord, 1 } },
            };
            sourceIterationData.WordLadderQueue.Enqueue(sourceIterationData.Node);

            var destinationIterationData = new WordLadderBdsIterationData
            {
                Node = new Node
                {
                    Word = DestinationWord,
                    Level = 1,
                },
                WordLadderQueue = new Queue<Node>(),
                VisitedNodeList = new Dictionary<String, int>() { { DestinationWord, 1 } },
            };
            destinationIterationData.WordLadderQueue.Enqueue(destinationIterationData.Node);

            sourceIterationData.OpositeIterationData = destinationIterationData;
            destinationIterationData.OpositeIterationData = sourceIterationData;
            var iterationDataList = new List<WordLadderBdsIterationData>
            {
                sourceIterationData,
                destinationIterationData
            };

            while (sourceIterationData.WordLadderQueue.Count > 0 && destinationIterationData.WordLadderQueue.Count > 0)
            {
                foreach (var iterationData in iterationDataList)
                {
                    var currentNode = iterationData.WordLadderQueue.Dequeue();

                    foreach(var dictionaryWord in Dictionary)
                    {
                        var isAdjacentAndNotVisited = HasOneCharacterDistance(currentNode.Word, dictionaryWord) && !iterationData.VisitedNodeList.ContainsKey(dictionaryWord);
                        if (isAdjacentAndNotVisited)
                        {
                            var unvisitedNode = new Node {
                                Word = dictionaryWord,
                                Level = currentNode.Level + 1,
                            };
                            iterationData.WordLadderQueue.Enqueue(unvisitedNode);
                            iterationData.VisitedNodeList.Add(dictionaryWord, currentNode.Level + 1);

                            if (iterationData.OpositeIterationData.VisitedNodeList.ContainsKey(unvisitedNode.Word))
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

        private List<string> BackTrack(WordLadderBdsIterationData iterationData, Node unvisitedNode)
        {
            var isIterationFromEndToStart = iterationData.Node.Word == DestinationWord;
            if(!isIterationFromEndToStart)
            {
                iterationData = iterationData.OpositeIterationData;
            }

            var levelDifferenceToOpposite = iterationData.OpositeIterationData.VisitedNodeList[unvisitedNode.Word];
            var back = new List<string>();
            for (int level = 0; level <= levelDifferenceToOpposite; level++)
            {
                back.Add(iterationData.VisitedNodeList.First(x => iterationData.OpositeIterationData.VisitedNodeList.Any(y => IsCharacterDistanceWithinLimit(x.Key, y.Key, level))).Key);
            }

            var levelDifferenceToSame = unvisitedNode.Level;
            var front = new List<string>();
            for (int level = levelDifferenceToSame; level >= 0; level--)
            {
                front.Add(iterationData.OpositeIterationData.VisitedNodeList.First(x => iterationData.VisitedNodeList.Any(y => IsCharacterDistanceWithinLimit(x.Key, y.Key, level))).Key);
            }

            front.AddRange(back);
            return new HashSet<string>(front).ToList();
        }

    }
}
