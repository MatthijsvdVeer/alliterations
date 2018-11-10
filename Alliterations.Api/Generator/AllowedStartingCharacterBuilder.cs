using System.Collections.Generic;
using System.Linq;

namespace Alliterations.Api.Generator
{
    internal interface IAllowedStartingCharacterBuilder 
    {
        char[] GetAllowedStartingCharacters(IEnumerable<char> keys1, IEnumerable<char> keys2);
    }

    internal sealed class AllowedStartingCharacterBuilder : IAllowedStartingCharacterBuilder
    {
        public char[] GetAllowedStartingCharacters(IEnumerable<char> keys1, IEnumerable<char> keys2)
        {
            return keys1.Where(keys2.Contains).ToArray();
        }
    }
}