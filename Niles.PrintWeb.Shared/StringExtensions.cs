namespace Niles.PrintWeb.Shared
{
    public static class StringExtensions
    {
        public static string GetShortening(this string source)
        {
            string shortening = "";

            var words = source.Split(' ');
            foreach (var word in words)
            {
                if (word.Length == 1)
                    shortening += word[0];
                else
                    shortening += word.ToUpper()[0];
            }

            return shortening;
        }
    }
}