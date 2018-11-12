using System;
using System.Collections.Generic;
using System.Linq;

namespace Alliterations.Api.Generator
{
    public interface IAlliterationsProvider
    {
        IEnumerable<string> GetAlliterationsByCategory(AlliterationCategory category, int count);

        IEnumerable<string> GetAlliterationsByCategoryAndStartingChar(AlliterationCategory category, char start, int count);
    }

    internal sealed class AlliterationsProvider : IAlliterationsProvider
    {
        private readonly IAlliterationOptionsFactory alliterationOptionsFactory;
        private readonly IRandomNumberGenerator randomNumberGenerator;
        private readonly ICachingProvider cachingProvider;

        public AlliterationsProvider(
            IAlliterationOptionsFactory alliterationOptionsFactory,
            IRandomNumberGenerator randomNumberGenerator,
            ICachingProvider cachingProvider)
        {
            this.alliterationOptionsFactory = alliterationOptionsFactory;
            this.randomNumberGenerator = randomNumberGenerator;
            this.cachingProvider = cachingProvider;
        }

        public IEnumerable<string> GetAlliterationsByCategory(AlliterationCategory category, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException(nameof(count));
            }

            var options = this.cachingProvider.GetOrSetInCache($"category_{category}",
                () => this.alliterationOptionsFactory.CreateAlliterationOptionsForCategory(category));

            for (var i = 0; i < count; i++)
            {
                var startingCharacter = this.GetRandomEntryFromArray(options.AllowedStartingCharacters);
                yield return this.GetAlliteration(options, startingCharacter);
            }
        }

        public IEnumerable<string> GetAlliterationsByCategoryAndStartingChar(AlliterationCategory category, char start, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException(nameof(count));
            }

            var options = this.cachingProvider.GetOrSetInCache($"category_{category}",
                () => this.alliterationOptionsFactory.CreateAlliterationOptionsForCategory(category));

            start = char.ToUpper(start);
            if (!options.AllowedStartingCharacters.Contains(start))
            {
                throw new ArgumentException(nameof(start));
            }

            for (var i = 0; i < count; i++)
            {
                yield return this.GetAlliteration(options, start);
            }
        }

        private string GetAlliteration(AlliterationOptions options, char startingCharacter)
        {
            var randomAdjective = this.GetRandomEntryFromArray(options.Adjectives[startingCharacter]);
            var randomNoun = this.GetRandomEntryFromArray(options.Nouns[startingCharacter]);
            return $"{randomAdjective} {randomNoun}";
        }

        private T GetRandomEntryFromArray<T>(T[] items)
        {
            return items[this.randomNumberGenerator.GetNext(items.Length)];
        }
    }
}