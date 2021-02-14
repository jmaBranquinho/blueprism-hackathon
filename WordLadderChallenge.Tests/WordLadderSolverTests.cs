using AutoFixture;
using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Solvers;

namespace WordLadderChallenge.Tests
{
    public class WordLadderSolverTests
    {
        private readonly WordLadderSolver _sut;
        private readonly Fixture _fixture;
        private readonly List<string> _dictionary;
        private readonly string _pathToSolution;

        private readonly static List<string> _adjacentWords = new List<string> { "same", "came", "case", "cast", "cost" };

        public WordLadderSolverTests()
        {
            _fixture = new Fixture();

            _dictionary = _fixture.Create<List<string>>();
            _dictionary.AddRange(_adjacentWords);
            _pathToSolution = _fixture.Create<string>();

            _sut = new WordLadderSolver(startWord: _adjacentWords.First(), endWord: _adjacentWords.Last(),
                _dictionary, _pathToSolution);
        }
    }
}
