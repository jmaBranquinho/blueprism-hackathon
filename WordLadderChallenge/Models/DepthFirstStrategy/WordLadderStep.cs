using System.Collections.Generic;
using System.Linq;

namespace WordLadderChallenge.Models.DepthFirstStrategy
{
    /// <summary>
    /// Contains a possible solution to the word ladder puzzle
    /// </summary>
    public class WordLadderStep
    {
        /// <summary>
        /// Contains the actual steps since the source word until the current node of the iteration
        /// </summary>
        public ICollection<string> Ladder { get; set; }

        /// <summary>
        /// Returns the current node of the iteration or, if the puzzle is completed sucessfully, returns the destination word
        /// </summary>
        public string CurrentWord => Ladder.LastOrDefault();
    }
}
