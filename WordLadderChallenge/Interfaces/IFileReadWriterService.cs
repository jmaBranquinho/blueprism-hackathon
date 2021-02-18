using System.Collections.Generic;

namespace WordLadderChallenge.Interfaces
{
    /// <summary>
    /// Reads or writes text to files
    /// </summary>
    public interface IFileReadWriterService
    {
        /// <summary>
        /// Provides the path to a file to be read
        /// </summary>
        string ReadFilePath { get; set; }

        /// <summary>
        /// Provides the path to a file to be written
        /// </summary>
        string WriteFilePath { get; set; }

        /// <summary>
        /// Returns a collection of strings, each one representing a line of the file of the path provided
        /// </summary>
        /// <returns></returns>
        ICollection<string> GetAllFileLines();

        /// <summary>
        /// Writes all the content, in lines, to the path provided
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isToOverwriteContent"></param>
        void WriteToFile(IEnumerable<string> content, bool isToOverwriteContent = true);
    }
}