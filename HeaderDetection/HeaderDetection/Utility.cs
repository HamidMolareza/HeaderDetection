using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeaderDetection
{
    public static class Utility
    {
        public static string ConvertIndexToName(int indexZeroBase)
        {
            if (indexZeroBase < 0)
                throw new ArgumentOutOfRangeException(nameof(indexZeroBase), "Value Can not be less than 0.");

            var name = "";
            while (indexZeroBase > 0)
            {
                // Find remainder
                var rem = indexZeroBase % 26;

                // If remainder is 0, then a
                // 'Z' must be there in output
                if (rem == 0)
                {
                    name += "Z";
                    indexZeroBase = (indexZeroBase / 26) - 1;
                }

                // If remainder is non-zero
                else
                {
                    name += (char) (rem - 1 + 'A');
                    indexZeroBase /= 26;
                }
            }

            // Reverse the string
            name = Reverse(name);

            return name;
        }

        private static string Reverse(string input)
        {
            char[] reversedString = input.ToCharArray();
            Array.Reverse(reversedString);
            return new string(reversedString);
        }

        public static int ConvertNameToIndex(string name)
        {
            name = NameMustValid(name);

            var sum = 0;
            for (var i = 0; i < name.Length; i++)
            {
                var index = name.Length - i - 1;
                sum += (int) Math.Pow(26, i) * (name[index] - 'A' + 1);
            }

            return sum;
        }

        private static string NameMustValid(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            name = name.ToUpper();
            if (!IsNameValid(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Characters must be A-Z.");
            return name;
        }

        public static bool IsNameValid(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return name.ToUpper().All(c => c >= 'A' && c <= 'Z');
        }

        public static string GetNextName(string name)
        {
            name = NameMustValid(name);

            var sb = new StringBuilder(name);
            for (var i = name.Length - 1; i >= 0; i--)
            {
                if (sb[i] < 'Z')
                {
                    sb[i] = (char) (sb[i] + 1);
                    return sb.ToString();
                }

                sb[i] = 'A';
            }

            sb.Append('A');
            return sb.ToString();
        }

        public static string GetNextName(string currentName, int count)
        {
            currentName = NameMustValid(currentName);
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Value can not be less than 0.");
            
            var resultName = currentName;

            for (var i = 0; i < count; i++)
                resultName = GetNextName(resultName);

            return resultName;
        }

        public static string[] GetNexNames(string currentName, int howMany)
        {
            if (howMany < 0)
                throw new ArgumentOutOfRangeException(nameof(howMany), "Value can not be less than 0.");
            currentName = NameMustValid(currentName);

            var resultNames = new List<string>(howMany);
            var name = currentName;

            for (var i = 0; i < howMany; i++)
            {
                name = GetNextName(name);
                resultNames.Add(name);
            }

            return resultNames.ToArray();
        }
    }
}