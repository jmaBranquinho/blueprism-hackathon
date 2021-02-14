using System.Collections.Generic;

namespace WordLadderChallenge.Abstractions
{
    public interface IWordLadderStrategy
    {
        string DestinationWord { get; set; }
        ICollection<string> Dictionary { get; set; }
        string SourceWord { get; set; }

        /// <summary>
        /// If the solution is found returns the WordLadderStep with all the steps from the source word to the destination
        /// Otherwise returns null
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> Solve();
    }
}