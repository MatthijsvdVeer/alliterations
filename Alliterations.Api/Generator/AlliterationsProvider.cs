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

    internal sealed class   AlliterationsProvider : IAlliterationsProvider
    {
        private readonly IAlliterationOptionsBuilder alliterationOptionsBuilder;

        private readonly IRandomNumberGenerator randomNumberGenerator;

        public AlliterationsProvider(
            IAlliterationOptionsBuilder alliterationOptionsBuilder,
            IRandomNumberGenerator randomNumberGenerator)
        {
            this.alliterationOptionsBuilder = alliterationOptionsBuilder;
            this.randomNumberGenerator = randomNumberGenerator;
        }

        public IEnumerable<string> GetAlliterationsByCategory(AlliterationCategory category, int count)
        {
            System.Console.WriteLine("HELLO!?");
            if (count <= 0)
            {
                throw new ArgumentException(nameof(count));
            }

            var options = this.alliterationOptionsBuilder.CreateAlliterationOptionsForCategory(category);

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
            
            var options = this.alliterationOptionsBuilder.CreateAlliterationOptionsForCategory(category);

            if(!options.AllowedStartingCharacters.Contains(start))
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

        private E GetRandomEntryFromArray<E>(E[] items)
        {
            return items[this.randomNumberGenerator.GetNext(items.Length)];
        }
    }
}