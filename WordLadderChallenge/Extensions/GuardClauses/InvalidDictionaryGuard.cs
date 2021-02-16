using System.Collections.Generic;
using System.Linq;
using WordLadderChallenge.Exceptions;

namespace Ardalis.GuardClauses
{
    public static class InvalidDictionaryGuard
    {
        public static void InvalidDictionary(this IGuardClause guardClause, IEnumerable<string> dictionary, IEnumerable<string> requiredWords)
        {
            var isDictionaryEmpty = dictionary == null || !dictionary.Any();
            var isValidDictionary = !isDictionaryEmpty &&
                requiredWords.All(requiredWord =>
                    dictionary.Any(dictionaryWord => requiredWord == dictionaryWord));
            if (!isValidDictionary)
            {
                throw new InvalidDictionaryException();
            }
        }
    }
}