using System.Numerics;

namespace URLShorter.Repositories
{
    public class ShortCodeGenerator
    {
        private const string Characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly Random _random = new Random();

        public string Generate(int length = 6) // выбран данный тип генератора, т.к. очень низкая вероятность получить коллизию, аналогично можно было сделать через GUID
        {
            var code = new char[length];

            for (int i = 0; i < length; i++) //выбираем рандомный символ из строки и присваиваем 
            {
                code[i] = Characters[_random.Next(Characters.Length)];
            }
            return new string(code);
        }
        // возможно добавить проверку, в случае если такой код уже есть, генерировать новый
    }
}
