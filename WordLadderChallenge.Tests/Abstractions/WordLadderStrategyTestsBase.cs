using AutoFixture;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Exceptions;
using WordLadderChallenge.Interfaces;
using Xunit;

namespace WordLadderChallenge.Tests.Abstractions
{
    public abstract class WordLadderStrategyTestsBase<TWordLadderStrategy> where TWordLadderStrategy : IWordLadderStrategy
    {
        protected IWordLadderStrategy _sut;

        protected Fixture _fixture;
        protected List<string> _dictionary;

        protected string _pathToDictionary;
        protected string _pathToSolution;
        protected IFileReadWriterService _fileReadWriterServiceMock;

        protected readonly static List<string> _adjacentWords = new List<string> { "same", "came", "case", "cast", "cost" };

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Solve_WhenSourceWordIsEmptyOrWhiteSpace_ShouldThrowArgumentException(string sourceWord)
        {
            // Arrange
            _sut.SourceWord = sourceWord;
            _sut.DestinationWord = _fixture.Create<string>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _sut.Solve());
        }

        [Fact]
        public void Solve_WhenSourceWordIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            _sut.SourceWord = null;
            _sut.DestinationWord = _fixture.Create<string>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sut.Solve());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Solve_WhenDestinationWordIsEmptyOrWhiteSpace_ShouldThrowArgumentException(string destinationWord)
        {
            // Arrange
            _sut.DestinationWord = destinationWord;
            _sut.SourceWord = _fixture.Create<string>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _sut.Solve());
        }


        [Fact]
        public void Solve_WhenDestinationWordIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            _sut.DestinationWord = null;
            _sut.SourceWord = _fixture.Create<string>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sut.Solve());
        }

        [Theory]
        [InlineData("shortSrcWord", "verylongDstWord")]
        [InlineData("verylongSrcWord", "shortDstWord")]
        public void Solve_WhenSourceAndDestinationsWordsHaveDifferentLengths_ShouldThrowWordLengthMismatchException(string sourceWord, string destinationWord)
        {
            // Arrange
            _sut.SourceWord = sourceWord;
            _sut.DestinationWord = destinationWord;

            var expectedErrorMessage = $"source word {sourceWord} and destination word {destinationWord} have different lengths";

            // Act & Assert
            var exception = Assert.Throws<WordLengthMismatchException>(() => _sut.Solve());
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "")]
        [InlineData(false, "someRandomWord")]
        public void Solve_WhenDictionaryIsNullOrEmptyOrDoesNotContainSourceOrDestinationWords_ShouldThrowInvalidDictionaryException(bool isArrayEmpty, params string[] dictionary)
        {
            // Arrange
            _fileReadWriterServiceMock.GetAllFileLines(_pathToDictionary).Returns(isArrayEmpty ? dictionary : new string[0]);

            var expectedErrorMessage = "Dictionary is either empty, null or does not contain the source and destination words";

            // Act & Assert
            var exception = Assert.Throws<InvalidDictionaryException>(() => _sut.Solve());
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void Solve_WhenWordLadderIsFoundSucessfully_ShouldReturnWordLadderSteps()
        {
            // Arrange
            var expectedLadderSteps = _adjacentWords;

            // Act
            var actualLadderStep = _sut.Solve();

            // Assert
            Assert.Equal(expectedLadderSteps, actualLadderStep);
        }

        [Fact]
        public void Solve_WhenWordLadderIsNotFound_ShouldReturnWordLadderSteps()
        {
            // Arrange
            var wordToRemove = _adjacentWords.Skip(1).Take(1).First();
            _dictionary.Remove(wordToRemove);

            var expectedLadderStep = Enumerable.Empty<string>();

            // Act
            var actualLadderStep = _sut.Solve();

            // Assert
            Assert.Equal(expectedLadderStep, actualLadderStep);
        }
    }
}
