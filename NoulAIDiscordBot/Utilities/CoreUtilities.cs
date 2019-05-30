using System;

namespace NoulAIBotNetCore.Utilities
{
    public class StringUtilities
    {
        public static int FindIndexOfString(string source, string find)
        {
            for (int i = 0; i < source.Length - find.Length; i++)
            {
                if (source.Substring(i).StartsWith(find))
                {
                    return i;
                }
            }
            return -1;
        }

        public static string Before(string source, char before, bool noStringIfNoMatch)
        {
            int index = source.IndexOf(before);
            if (index == -1)
            {
                return noStringIfNoMatch ? "" : source;
            }
            if (index == 0)
            {
                return "";
            }
            return source.Substring(0, index);
        }

        public static string Before(string source, char before)
        {
            return Before(source, before, false);
        }

        public static string Before(string source, string before, bool noStringIfNoMatch)
        {
            int index = FindIndexOfString(source, before);
            if (index == -1)
            {
                return noStringIfNoMatch ? "" : source;
            }
            if (index == 0)
            {
                return "";
            }
            return source.Substring(0, index);
        }

        public static string Before(string source, string before)
        {
            return Before(source, before, false);
        }

        public static string After(string source, char after, bool noStringIfNoMatch)
        {
            int index = source.IndexOf(after);
            if (index == -1)
            {
                return noStringIfNoMatch ? "" : source;
            }
            if (index == source.Length - 1)
            {
                return "";
            }
            return source.Substring(index + 1);
        }

        public static string After(string source, char after)
        {
            return After(source, after, false);
        }

        public static string After(string source, string after, bool noStringIfNoMatch)
        {
            int index = FindIndexOfString(source, after);
            if (index == -1)
            {
                return noStringIfNoMatch ? "" : source;
            }
            if (index == source.Length - after.Length - 1)
            {
                return "";
            }
            return source.Substring(index + after.Length + 1);
        }

        public static string After(string source, string after)
        {
            return After(source, after, false);
        }
    }
}
