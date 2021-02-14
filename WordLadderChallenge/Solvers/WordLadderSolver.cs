using System;
using System.Collections.Generic;
using WordLadderChallenge.Models;

namespace WordLadderChallenge.Solvers
{
    public class WordLadderSolver
    {
        private readonly string _startWord;
        private readonly string _endWord;
        private readonly string _pathToResultFile;
        private ICollection<string> _dictionary;

        public WordLadderSolver(string startWord, string endWord, ICollection<string> dictionary, string pathToResultFile)
        {
            _startWord = startWord;
            _endWord = endWord;
            _dictionary = dictionary;
            _pathToResultFile = pathToResultFile;
        }

        public WordLadderStep Solve()
        {
            throw new NotImplementedException();
        }
    }
}