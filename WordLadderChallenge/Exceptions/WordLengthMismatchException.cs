using System;

namespace WordLadderChallenge.Exceptions
{
    public class WordLengthMismatchException : Exception
    {
        public WordLengthMismatchException(string sourceWord, string destinationWord)
            : base($"source word {sourceWord} and destination word {destinationWord} have different lengths")
        {
        }
    }
}
