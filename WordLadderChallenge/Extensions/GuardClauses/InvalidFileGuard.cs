using System.IO;
using WordLadderChallenge.Exceptions;

namespace Ardalis.GuardClauses
{
    public static class InvalidFileGuard
    {
        public static void InvalidFile(this IGuardClause guardClause, string input)
        {
            if(string.IsNullOrWhiteSpace(input))
            {
                throw new InvalidPathException("provided path is null or white space");
            }
            if (string.IsNullOrWhiteSpace(Path.GetExtension(input)))
            {
                throw new InvalidPathException(input);
            }
        }
    }
}
