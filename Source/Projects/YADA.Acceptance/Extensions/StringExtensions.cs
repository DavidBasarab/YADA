using System.Text;

namespace YADA.Acceptance.Extensions
{
    internal static class StringExtensions
    {
        public static char GetRandomCharacter()
        {
            while (true)
            {
                var nextRan = NumberExtensions.NextRandom(47, 122);

                switch (nextRan)
                {
                    case 47:
                    case 58:
                    case 59:
                    case 60:
                    case 61:
                    case 62:
                    case 63:
                    case 64:
                    case 91:
                    case 92:
                    case 93:
                    case 94:
                    case 95:
                    case 96:
                        continue;
                    default:
                        return (char)(nextRan);
                }
            }
        }

        public static string GetRandomString(int length)
        {
            var retValue = new StringBuilder();

            for (var i = 0; i < length; i++) retValue.Append(GetRandomCharacter().ToString());

            return retValue.ToString();
        }
    }
}