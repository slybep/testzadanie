using System.Numerics;

namespace URLShorter.Repositories
{
    public class ShortCodeGenerator
    {
        private const string Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly Random _random = new Random();

        public string Generate(int length = 6)
        {
            var code = new char[length];

            for (int i = 0; i < length; i++)
            {
                code[i] = Characters[_random.Next(Characters.Length)];
            }
            return new string(code);
        }
        
    }
}
