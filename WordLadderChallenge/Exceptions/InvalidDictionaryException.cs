using System;

namespace WordLadderChallenge.Exceptions
{
    public class InvalidDictionaryException : Exception
    {
        public InvalidDictionaryException()
            : base($"Dictionary is either empty, null or does not contain the source and destination words")
        {
        }
    }
}
