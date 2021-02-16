using System.Collections.Generic;
using System.Linq;

namespace WordLadderChallenge.Extensions.Utils
{
    public static class CollectionUtils
    {
        /// <summary>
        /// Checks if a collection is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection is null || !collection.Any();
        }
    }
}
