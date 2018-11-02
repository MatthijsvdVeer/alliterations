namespace Alliterations.Api.Generator
{
    internal interface IAlliterationOptionsBuilder
    {
        AlliterationOptions CreateAlliterationOptionsForCategory(AlliterationCategory category);
    }

    internal sealed class AlliterationOptionsBuilder : IAlliterationOptionsBuilder
    {
        private readonly IWordListBuilder wordListBuilder;
        
        private readonly IAllowedStartingCharacterBuilder allowedStartingCharacterBuilder;

        public AlliterationOptionsBuilder(
            IWordListBuilder wordListBuilder, 
            IAllowedStartingCharacterBuilder allowedStartingCharacterBuilder)
        {
            this.wordListBuilder = wordListBuilder;
            this.allowedStartingCharacterBuilder = allowedStartingCharacterBuilder;
        }
        public AlliterationOptions CreateAlliterationOptionsForCategory(AlliterationCategory category)
        {
            var adjectives = this.wordListBuilder.BuildDictionaryForCategory(AlliterationPart.Adjective, category);
            var nouns = this.wordListBuilder.BuildDictionaryForCategory(AlliterationPart.Noun, category);
            var startingCharacters = this.allowedStartingCharacterBuilder
                .GetAllowedStartingCharacters(adjectives.Keys, nouns.Keys);

            return new AlliterationOptions
            {
                Adjectives = adjectives,
                Nouns = nouns,
                AllowedStartingCharacters = startingCharacters
            };
        }
    }
}