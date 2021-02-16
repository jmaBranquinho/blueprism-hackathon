using System.IO;
using WordLadderChallenge.Exceptions;

namespace Ardalis.GuardClauses
{
    public static class InvalidPathGuard
    {
        public static void InvalidPath(this IGuardClause guardClause, string input)
        {
            if(!string.IsNullOrEmpty(input.Trim(new char[] { '\\', '/' })) && Path.HasExtension(input))
            {
                throw new InvalidPathException(input);
            }
        }
    }
}