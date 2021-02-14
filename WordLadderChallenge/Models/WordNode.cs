namespace WordLadderChallenge.Models
{
    /// <summary>
    /// Contains a word and the distance to the starting node
    /// </summary>
    public class WordNode
    {
        /// <summary>
        /// The word found
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Distance in both node and number of characters from the starting node
        /// </summary>
        public int Level { get; set; }
    }
}
