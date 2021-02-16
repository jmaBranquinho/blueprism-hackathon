using AutoFixture;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Interfaces;
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

            _pathToDictionary = $"{_fixture.Create<string>()}.txt";
            _pathToSolution = $"{_fixture.Create<string>()}.txt";

            _fileReadWriterServiceMock = Substitute.For<IFileReadWriterService>();
            _fileReadWriterServiceMock.GetAllFileLines(_pathToDictionary).Returns(_dictionary);

            _sut = new WordLadderDepthFirstStrategy(_fileReadWriterServiceMock) 
            {
                SourceWord = _adjacentWords.First(),
                DestinationWord = _adjacentWords.Last(),
                PathToDictionary = _pathToDictionary,
                PathToSolution = _pathToSolution,
            };
        }
    }
}
