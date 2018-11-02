using System;

namespace Alliterations.Api.Generator
{
    internal interface IRandomNumberGenerator
    {
        int GetNext(int max);
    }

    internal sealed class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random random;

        public RandomNumberGenerator()
        {
            this.random = new Random();
        }

        public int GetNext(int max)
        {
            return this.random.Next(max);
        }
    }
}