using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordLadderChallenge.Abstractions;
using WordLadderChallenge.Interfaces;

namespace WordLadderChallenge.Services
{
    public class FileReadWriterService : IFileReadWriterService
    {
        public ICollection<string> GetAllFileLines(string pathToDictionaryFile)
        {
            Guard.Against.InvalidFile(pathToDictionaryFile);

            return File.ReadAllLines(pathToDictionaryFile).ToList();
        }

        public void WriteToFile(string pathToSolutionFile, IEnumerable<string> content)
        {
            Guard.Against.InvalidFile(pathToSolutionFile);
            Guard.Against.NullOrEmpty(content, nameof(content));

            File.WriteAllLines(pathToSolutionFile, content);
        }
    }
}
