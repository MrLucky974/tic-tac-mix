namespace LuckiusDev.Utils
{
    public static class NumberFormatter
    {
        public static string FormatNumberWithSuffix(float number)
        {
            string[] suffixes = { "", "K", "M", "B", "T" }; // Add more suffixes as needed
        
            int suffixIndex = 0;
            while (number >= 1000f && suffixIndex < suffixes.Length - 1)
            {
                number /= 1000f;
                suffixIndex++;
            }

            string formattedNumber = number.ToString("F0") + suffixes[suffixIndex];
            return formattedNumber;
        }
    }
}