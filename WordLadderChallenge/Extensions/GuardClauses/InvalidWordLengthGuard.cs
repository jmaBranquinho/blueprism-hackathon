using WordLadderChallenge.Exceptions;

namespace Ardalis.GuardClauses
{
    public static class InvalidWordLengthGuard
    {
        public static void InvalidWordLength(this IGuardClause guardClause, string word1, string word2)
        {
            if (word1?.Length != word2?.Length)
            {
                throw new WordLengthMismatchException(word1, word2);
            }
        }
    }
}