using System.Collections.Generic;

namespace WordLadderChallenge.Interfaces
{
    /// <summary>
    /// Provides methods to solve word ladder puzzles
    /// </summary>
    public interface IWordLadderStrategy
    {
        /// <summary>
        /// Returns the word ladder as a list of strings if a solution is found, or an empty list otherwise
        /// </summary>
        /// <param name="sourceWord"></param>
        /// <param name="destinationWord"></param>
        /// <returns></returns>
        IEnumerable<string> Solve(string sourceWord, string destinationWord);

        /// <summary>
        /// Updates the dictionary with the words provided by the FileReadWriterService
        /// </summary>
        void ReadDictionary();
    }
}