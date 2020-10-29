namespace GedcomParser.Extensions
{
    public static class StringExtender
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsSpecified(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}