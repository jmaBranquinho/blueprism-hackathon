using System.Collections.Generic;

namespace WordLadderChallenge.Interfaces
{
    /// <summary>
    /// Provides methods to solve a word ladder puzzle
    /// </summary>
    public interface IWordLadderStrategy
    {
        /// <summary>
        /// Starting word
        /// </summary>
        string SourceWord { get; set; }

        /// <summary>
        /// Word to be found
        /// </summary>
        string DestinationWord { get; set; }

        /// <summary>
        /// Path to the dictionary file
        /// </summary>
        string PathToDictionary { get; set; }

        /// <summary>
        /// Path to where the solution will be written
        /// </summary>
        string PathToSolution { get; set; }

        /// <summary>
        /// If the solution is found returns true and writes the solution to the solution file, otherwise false is returned and
        /// nothing is written
        /// Otherwise returns null
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> Solve();
    }
}