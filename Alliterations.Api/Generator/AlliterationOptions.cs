using System.Collections.Generic;

namespace Alliterations.Api.Generator 
{
    internal sealed class AlliterationOptions
    {
        public IDictionary<char, string[]> Adjectives { get; set; }

        public IDictionary<char, string[]> Nouns { get; set; }

        public char[] AllowedStartingCharacters { get; set; }

    }
}