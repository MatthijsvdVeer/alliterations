using System;
using System.Collections.Generic;
using System.IO;

namespace Alliterations.Api.Generator
{
    internal interface IWordFileReader
    {
        IEnumerable<string> ReadLines(string path);
    }

    internal sealed class WordFileReader : IWordFileReader
    {
        public IEnumerable<string> ReadLines(string path)
        {
            if (!File.Exists(path))
            {
                throw new InvalidOperationException();
            }

            return File.ReadLines(path);
        }
    }
}