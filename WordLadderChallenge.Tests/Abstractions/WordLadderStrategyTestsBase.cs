﻿using AutoFixture;
using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Exceptions;
using Xunit;

namespace WordLadderChallenge.Tests.Abstractions
{
    public abstract class WordLadderStrategyTestsBase<TWordLadderStrategy> where TWordLadderStrategy : IWordLadderStrategy
    {
        protected IWordLadderStrategy _sut;
        protected Fixture _fixture;
        protected List<string> _dictionary;

        protected readonly static List<string> _adjacentWords = new List<string> { "same", "came", "case", "cast", "cost" };

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Solve_WhenSourceWordIsEmptyOrNull_ShouldThrowInvalidWordException(string sourceWord)
        {
            // Arrange
            _sut.SourceWord = sourceWord;
            _sut.DestinationWord = _fixture.Create<string>();

            var expectedErrorMessage = "source word and/or destination word is null or empty";

            // Act & Assert
            var exception = Assert.Throws<InvalidWordException>(() => _sut.Solve());
            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Solve_WhenDestinationWordIsEmptyOrNull_ShouldThrowInvalidWordException(string destinationWord)
        {
            // Arrange
            _sut.DestinationWord = destinationWord;
            _sut.SourceWord = _fixture.Create<string>();

            var expectedErrorMessage = "source word and/or destination word is null or empty";

            // Act & Assert
            var exception = Assert.Throws<InvalidWordException>(() => _sut.Solve());
            Assert.Equal(expectedErrorMessage, exception.Message);
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
            _sut.Dictionary = isArrayEmpty ? dictionary : new string[0];

            var expectedErrorMessage = "Dictionary is either empty, null or does not contain the source and destination words";

            // Act & Assert
            var exception = Assert.Throws<InvalidDictionaryException>(() => _sut.Solve());
            Assert.Equal(expectedErrorMessage, exception.Message);
        }


        [Fact]
        public void Solve_WhenCalled_ShouldRemoveWordsWithLengthGreaterOrSmallerThanSourceAndDestination()
        {
            // Arrange
            var wordLength = _sut.SourceWord.Length;
            var expectedDictionary = _sut.Dictionary.Where(word => word.Length == wordLength);

            // Act
            _sut.Solve();

            // Assert
            Assert.Equal(expectedDictionary, _sut.Dictionary);
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
            _sut.Dictionary.Remove(wordToRemove);

            var expectedLadderStep = Enumerable.Empty<string>();

            // Act
            var actualLadderStep = _sut.Solve();

            // Assert
            Assert.Equal(expectedLadderStep, actualLadderStep);
        }
    }
}