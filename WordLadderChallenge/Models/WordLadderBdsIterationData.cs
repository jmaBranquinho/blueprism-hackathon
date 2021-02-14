using System.Collections.Generic;

namespace WordLadderChallenge.Models
{
    public class WordLadderBdsIterationData
    {
        public Node Node { get; set; }

        public Queue<Node> WordLadderQueue { get; set; }

        public Dictionary<string, int> VisitedNodeList { get; set; }

        public WordLadderBdsIterationData OpositeIterationData { get; set; }
    }
}
