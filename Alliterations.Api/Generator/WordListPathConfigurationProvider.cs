namespace Alliterations.Api.Generator
{
    internal interface IWordListPathConfigurationProvider
    {
        string GetPathForPartAndCategory(AlliterationPart part, AlliterationCategory category);
    }

    internal sealed class WordListPathConfigurationProvider : IWordListPathConfigurationProvider
    {
        public string GetPathForPartAndCategory(AlliterationPart part, AlliterationCategory category)
        {
            return part == AlliterationPart.Adjective
                ? ".\\WordLists\\WordNet\\adjectives.txt"
                : ".\\WordLists\\WordNet\\nouns.txt";
        }
    }
}