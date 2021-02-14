using System.Collections.Generic;

namespace WordLadderChallenge.Models
{
    /// <summary>
    /// Contains the information to iterate a dictionary using BDS algorithm
    /// </summary>
    public class WordLadderBdsIterationData
    {
        /// <summary>
        /// The starting node. Can either be the source or the destination word
        /// </summary>
        public WordNode StartingNode { get; set; }

        /// <summary>
        /// Contains the queue for the next unvisited nodes to iterate
        /// </summary>
        public Queue<WordNode> WordLadderQueue { get; set; }

        /// <summary>
        /// Contains the list of already visited nodes
        /// </summary>
        public Dictionary<string, int> VisitedNodeList { get; set; }

        /// <summary>
        /// Reference to the iteration data of the oposite node
        /// Example: if this object contains data from the source word
        /// Then this value contains the reference to the data of the destination word
        /// </summary>
        public WordLadderBdsIterationData OpositeIterationData { get; set; }
    }
}
