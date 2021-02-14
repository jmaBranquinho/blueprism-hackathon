using AutoFixture;
using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Strategies;
using WordLadderChallenge.Tests.Abstractions;

namespace WordLadderChallenge.Tests
{
    public class WordLadderDepthFirstStrategyTests : WordLadderStrategyTestsBase<IWordLadderStrategy>
    {
        public WordLadderDepthFirstStrategyTests()
        {
            _fixture = new Fixture();

            _dictionary = _fixture.Create<List<string>>();
            _dictionary.AddRange(_adjacentWords);

            _sut = new WordLadderDepthFirstStrategy() 
            {
                Dictionary = _dictionary,
                SourceWord = _adjacentWords.First(),
                DestinationWord = _adjacentWords.Last(),
            };
        }
    }
}
