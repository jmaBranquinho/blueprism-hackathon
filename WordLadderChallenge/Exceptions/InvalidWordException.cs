using System;

namespace WordLadderChallenge.Exceptions
{
    public class InvalidWordException : Exception
    {
        public InvalidWordException()
            : base($"source word and/or destination word is null or empty")
        {
        }
    }
}
