using System.Collections.Generic;

namespace WordLadderChallenge.Abstractions
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
        /// List of all the available words
        /// </summary>
        ICollection<string> Dictionary { get; set; }
        
        /// <summary>
        /// If the solution is found returns the WordLadderStep with all the steps from the source word to the destination
        /// Otherwise returns null
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> Solve();
    }
}