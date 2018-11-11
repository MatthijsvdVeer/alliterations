using Alliterations.Api.Generator;
using Microsoft.Extensions.DependencyInjection;

namespace Alliterations.Api
{
    internal static class Dependencies
    {
        public static void BootstrapDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAlliterationOptionsFactory, AlliterationOptionsFactory>();
            services.AddTransient<IAlliterationsProvider, AlliterationsProvider>();
            services.AddTransient<IAllowedStartingCharacterBuilder, AllowedStartingCharacterBuilder>();
            services.AddTransient<IWordFileReader, WordFileReader>();
            services.AddTransient<IWordListBuilder, WordListBuilder>();
            services.AddTransient<IWordListPathConfigurationProvider, WordListPathConfigurationProvider>();
            services.AddTransient<ICachingProvider, CachingProvider>();

            services.AddSingleton<IRandomNumberGenerator, RandomNumberGenerator>();
        }
    }
}