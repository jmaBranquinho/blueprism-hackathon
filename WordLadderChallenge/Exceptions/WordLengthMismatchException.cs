using System;
using System.Runtime.Serialization;

namespace WordLadderChallenge.Exceptions
{
    [Serializable]
    public class WordLengthMismatchException : Exception
    {
        public WordLengthMismatchException(string sourceWord, string destinationWord)
            : base($"source word {sourceWord} and destination word {destinationWord} have different lengths")
        {
        }

        protected WordLengthMismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
