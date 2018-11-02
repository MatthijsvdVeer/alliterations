using System;
using System.Collections.Generic;
using System.Linq;

namespace Alliterations.Api.Generator
{
    internal interface IWordListBuilder
    {
        IDictionary<char, string[]> BuildDictionaryForCategory(AlliterationPart part, AlliterationCategory category);
    }

    internal sealed class WordListBuilder : IWordListBuilder
    {
        private readonly IWordListPathConfigurationProvider wordListPathConfigurationProvider;
        
        private readonly IWordFileReader wordFileReader;

        public WordListBuilder(
            IWordListPathConfigurationProvider wordListPathConfigurationProvider,
            IWordFileReader wordFileReader)
        {
            this.wordListPathConfigurationProvider = wordListPathConfigurationProvider;
            this.wordFileReader = wordFileReader;
        }
        public IDictionary<char, string[]> BuildDictionaryForCategory(AlliterationPart part, AlliterationCategory category)
        {
            var path = this.wordListPathConfigurationProvider
                .GetPathForPartAndCategory(part, category);
            var words = this.wordFileReader.ReadLines(path);

            var lookup = words.ToLookup(word => Char.ToUpper(word[0]));
            return lookup.ToDictionary(grouping => grouping.Key, grouping => grouping.ToArray());
        }
    }
}