using AutoFixture;
using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Strategies;
using WordLadderChallenge.Tests.Abstractions;

namespace WordLadderChallenge.Tests
{
    public class WordLadderBidirectionalSearchStrategyTests : WordLadderStrategyTestsBase<IWordLadderStrategy>
    {
        public WordLadderBidirectionalSearchStrategyTests()
        {
            _fixture = new Fixture();

            _dictionary = _fixture.Create<List<string>>();
            _dictionary.AddRange(_adjacentWords);

            _sut = new WordLadderBidirectionalSearchStrategy()
            {
                Dictionary = _dictionary,
                SourceWord = _adjacentWords.First(),
                DestinationWord = _adjacentWords.Last(),
            };
        }
    }
}
