using System;
using System.Runtime.Serialization;

namespace WordLadderChallenge.Exceptions
{
    [Serializable]
    public class InvalidDictionaryException : Exception
    {
        public InvalidDictionaryException()
            : base($"Dictionary is either empty, null or does not contain the source and destination words")
        {
        }

        protected InvalidDictionaryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
