namespace Pdsr.Core.Extensions
{
    public static class StringExtensions
    {
        private const string _redactedText = "***REDACTED***";

        /// <summary>
        /// Redacts a secure text/password to surface in Logs
        /// </summary>
        /// <param name="text">The text to redact</param>
        /// <param name="charsToKeep">number of characters to keep</param>
        /// <param name="includeRedactText">Should add the text ***Redact*** or not</param>
        /// <param name="redactFromStart">Redact from start or end.</param>
        /// <param name="redactedText">What text to show? default is ***REDACTED***</param>
        /// <returns></returns>
        public static string Redact(this string text, int charsToKeep = 5, bool includeRedactText = true, bool redactFromStart = true, string redactedText = _redactedText)
        {
            string redacted = text;
            try
            {
                if (!string.IsNullOrEmpty(text) && text.Length > charsToKeep + 1)
                {
                    if (redactFromStart)
                        redacted = text.Substring(text.Length - charsToKeep);
                    else
                        redacted = text.Substring(0, charsToKeep);

                    if (includeRedactText)
                        redacted = $"{redactedText} " + redacted;
                }
                return redacted;
            }
            catch { }
            return redacted;
        }

        /// <summary>
        /// Redacts a secure text/password to surface in Logs
        /// </summary>
        /// <param name="text">The text to redact</param>
        /// <param name="charsToKeep">number of characters to keep</param>
        /// <param name="includeRedactText">Should add the text ***Redact*** or not</param>
        /// <param name="redactFromStart">Redact from start or end.</param>
        /// <returns>Redacted text</returns>
        public static string Redact(this string text, int charsToKeep = 5, bool includeRedactText = true, bool redactFromStart = true)
        {
            return text.Redact(charsToKeep, includeRedactText, redactFromStart, _redactedText);
        }

        /// <summary>
        /// Redacts a secure text/password to surface in Logs from start of the string
        /// </summary>
        /// <param name="text">The text to redact</param>
        /// <param name="charsToKeep">number of characters to keep</param>
        /// <param name="includeRedactText">Should add the text ***Redact*** or not</param>
        /// <returns>Redacted Text</returns>
        public static string Redact(this string text, int charsToKeep = 5, bool includeRedactText = true)
        {
            return text.Redact(charsToKeep, includeRedactText, true, _redactedText);
        }

        /// <summary>
        /// Redacts a secure text/password to surface in Logs from start of the string
        /// </summary>
        /// <param name="text">The text to redact</param>
        /// <param name="charsToKeep">number of characters to keep</param>
        /// <returns>Redacted Text</returns>
        public static string Redact(this string text, int charsToKeep = 5)
        {
            return text.Redact(charsToKeep, true, true, _redactedText);
        }

        /// <summary>
        /// Redacts a secure text/password to surface in Logs
        /// Keeps 5 characters from start of the string
        /// </summary>
        /// <param name="text">The text to redact</param>
        /// <returns>Redacted Text</returns>
        public static string Redact(this string text)
        {
            return text.Redact(5, true, true, _redactedText);
        }
    }
}
