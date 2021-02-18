using AutoFixture;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var destinationWord = _fixture.Create<string>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _sut.Solve(sourceWord, destinationWord));
        }

        [Fact]
        public void Solve_WhenSourceWordIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            string sourceWord = null;
            var destinationWord = _fixture.Create<string>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sut.Solve(sourceWord, destinationWord));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Solve_WhenDestinationWordIsEmptyOrWhiteSpace_ShouldThrowArgumentException(string destinationWord)
        {
            // Arrange
            var sourceWord = _fixture.Create<string>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _sut.Solve(sourceWord, destinationWord));
        }


        [Fact]
        public void Solve_WhenDestinationWordIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            string destinationWord = null;
            var sourceWord = _fixture.Create<string>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sut.Solve(sourceWord, destinationWord));
        }

        [Theory]
        [InlineData("shortSrcWord", "verylongDstWord")]
        [InlineData("verylongSrcWord", "shortDstWord")]
        public void Solve_WhenSourceAndDestinationsWordsHaveDifferentLengths_ShouldThrowWordLengthMismatchException(string sourceWord, string destinationWord)
        {
            // Arrange
            var expectedErrorMessage = $"source word {sourceWord} and destination word {destinationWord} have different lengths";

            // Act & Assert
            var exception = Assert.Throws<WordLengthMismatchException>(() => _sut.Solve(sourceWord, destinationWord));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "")]
        [InlineData(false, "someRandomWord")]
        public void Solve_WhenDictionaryIsNullOrEmptyOrDoesNotContainSourceOrDestinationWords_ShouldThrowInvalidDictionaryException(bool isArrayEmpty, params string[] dictionary)
        {
            // Arrange
            var sourceWord = _fixture.Create<string>();
            var destinationWord = _fixture.Create<string>();

            _fileReadWriterServiceMock.GetAllFileLines().Returns(isArrayEmpty ? dictionary : new string[0]);

            var expectedErrorMessage = "Dictionary is either empty, null or does not contain the source and destination words";

            // Act & Assert
            var exception = Assert.Throws<InvalidDictionaryException>(() => _sut.Solve(sourceWord, destinationWord));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void Solve_WhenWordLadderIsFoundSucessfully_ShouldReturnWordLadderSteps()
        {
            // Arrange
            var sourceWord = _adjacentWords.First();
            var destinationWord = _adjacentWords.Last();

            var expectedLadderSteps = _adjacentWords;

            // Act
            var actualLadderStep = _sut.Solve(sourceWord, destinationWord);

            // Assert
            Assert.Equal(expectedLadderSteps, actualLadderStep);
        }

        [Fact]
        public void Solve_WhenWordLadderIsNotFound_ShouldReturnWordLadderSteps()
        {
            // Arrange
            var sourceWord = _adjacentWords.First();
            var destinationWord = _adjacentWords.Last();

            var wordToRemove = _adjacentWords.Skip(1).Take(1).First();

            _dictionary.Remove(wordToRemove);

            var expectedLadderStep = Enumerable.Empty<string>();

            // Act
            var actualLadderStep = _sut.Solve(sourceWord, destinationWord);

            // Assert
            Assert.Equal(expectedLadderStep, actualLadderStep);
        }


        [Fact]
        public void ReadDictionary_WhenDictionaryChanges_ShouldUpdateItWithNewWords()
        {
            // Act
            _sut.ReadDictionary();

            // Assert
            _fileReadWriterServiceMock.Received().GetAllFileLines();

        }
    }
}
