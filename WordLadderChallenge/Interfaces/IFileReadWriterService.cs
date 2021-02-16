using System.Collections.Generic;

namespace WordLadderChallenge.Interfaces
{
    /// <summary>
    /// Reads or writes text to files
    /// </summary>
    public interface IFileReadWriterService
    {
        /// <summary>
        /// Returns a collection of strings, each one representing a line of the file of the path provided
        /// </summary>
        /// <param name="pathToDictionaryFile"></param>
        /// <returns></returns>
        ICollection<string> GetAllFileLines(string pathToDictionaryFile);

        /// <summary>
        /// Writes all the content, in lines, to the path provided
        /// </summary>
        /// <param name="pathToSolutionFile"></param>
        /// <param name="content"></param>
        void WriteToFile(string pathToSolutionFile, IEnumerable<string> content);
    }
}