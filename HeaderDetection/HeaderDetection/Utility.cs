using System;
using System.Collections.Generic;
using System.Text;

namespace HeaderDetection
{
    public static class Utility
    {
        public static string ConvertIndexToName(int index)
        {
            var name = "";
            while (index > 0)
            {
                // Find remainder
                var rem = index % 26;

                // If remainder is 0, then a
                // 'Z' must be there in output
                if (rem == 0)
                {
                    name += "Z";
                    index = (index / 26) - 1;
                }

                // If remainder is non-zero
                else
                {
                    name += (char) (rem - 1 + 'A');
                    index /= 26;
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
            var sum = 0;
            for (var i = 0; i < name.Length; i++)
            {
                var index = name.Length - i - 1;
                sum += (int) Math.Pow(26, i) * (name[index] - 'A' + 1);
            }

            return sum;
        }

        public static string GetNextName(string name)
        {
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
            var resultName = currentName;

            for (var i = 0; i < count; i++)
                resultName = GetNextName(resultName);

            return resultName;
        }

        public static string[] GetNexNames(string currentName, int count)
        {
            var resultNames = new List<string>(count);
            var name = currentName;

            for (var i = 0; i < count; i++)
            {
                name = GetNextName(name);
                resultNames.Add(name);
            }

            return resultNames.ToArray();
        }
    }
}