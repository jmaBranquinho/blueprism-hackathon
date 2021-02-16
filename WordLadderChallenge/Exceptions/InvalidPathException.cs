using System;
using System.Runtime.Serialization;

namespace WordLadderChallenge.Exceptions
{
    [Serializable]
    public class InvalidPathException : Exception
    {
        public InvalidPathException(string path)
            : base($"Invalid Path: {path}")
        {
        }

        protected InvalidPathException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
