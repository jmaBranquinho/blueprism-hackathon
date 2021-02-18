using AutoFixture;
using NSubstitute;
using System.Collections.Generic;
using WordLadderChallenge.Interfaces;
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

            _pathToDictionary = $"{_fixture.Create<string>()}.txt";
            _pathToSolution = $"{_fixture.Create<string>()}.txt";

            _fileReadWriterServiceMock = Substitute.For<IFileReadWriterService>();
            _fileReadWriterServiceMock.GetAllFileLines().Returns(_dictionary);

            _sut = new WordLadderBidirectionalSearchStrategy(_fileReadWriterServiceMock);
        }
    }
}
