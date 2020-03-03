namespace LoopAr.Internals
{
    public static class FileAdditions
    {
            public const char FileNameAndTypSeparator = '.';
            public const char FilePrefixAndSuffixSeparator = '_';
    }

    public static class FileEndings
    {
        public const string Csv = ".csv";
        public const string Binary = ".dat";

        public static bool IsValidClassEnding(string possibleClassFile)
        {
            return Csv.Equals(possibleClassFile) || Binary.Equals(possibleClassFile);
        }
    }
}