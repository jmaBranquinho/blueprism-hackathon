using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordLadderChallenge.Interfaces;

namespace WordLadderChallenge.Services
{
    /// <summary>
    /// Reads or writes text to files
    /// </summary>
    public class FileReadWriterService : IFileReadWriterService
    {
        /// <summary>
        /// Provides the path to a file to be read
        /// </summary>
        public string ReadFilePath { get; set; }

        /// <summary>
        /// Provides the path to a file to be written
        /// </summary>
        public string WriteFilePath { get; set; }

        /// <summary>
        /// Returns a collection of strings, each one representing a line of the file of the path provided
        /// </summary>
        /// <returns></returns>
        public ICollection<string> GetAllFileLines()
        {
            Guard.Against.InvalidFile(ReadFilePath);

            return File.ReadAllLines(ReadFilePath).ToList();
        }

        /// <summary>
        /// Writes all the content, in lines, to the path provided
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isToOverwriteContent"></param>
        public void WriteToFile(IEnumerable<string> content, bool isToOverwriteContent = true)
        {
            Guard.Against.InvalidFile(WriteFilePath);
            Guard.Against.NullOrEmpty(content, nameof(content));

            if (isToOverwriteContent)
            {
                File.WriteAllLines(WriteFilePath, content);
            }
            else
            {
                File.AppendAllLines(WriteFilePath, content);
            }
        }
    }
}
