using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SS.GiftShop.Application.Infrastructure
{
    public class CsvWriter
    {
        private const string EscapedQuote = "\"\"\"";
        private const char Quote = '"';
        private const char Separator = ',';
        private const char CollectionSeparator = ',';
        private static readonly char[] EscapableChars = { Separator, Quote, '\r', '\n' };

        private readonly TextWriter _writer;

        public CsvWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public async Task WriteLineAsync(IEnumerable<string> values)
        {
            var index = 0;
            foreach (var item in values)
            {
                if (index > 0)
                {
                    await _writer.WriteAsync(Separator);
                }

                await WriteAsync(item);
                index++;
            }

            await _writer.WriteLineAsync();
        }

        public async Task WriteAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var shouldBeQuoted = ShouldQuote(value);
            if (shouldBeQuoted)
            {
                await _writer.WriteAsync(Quote);
            }

            for (var i = 0; i < value.Length; i++)
            {
                var c = value[i];
                switch (c)
                {
                    case Quote:
                        await _writer.WriteAsync(EscapedQuote);
                        break;

                    default:
                        await _writer.WriteAsync(c);
                        break;
                }
            }

            if (shouldBeQuoted)
            {
                await _writer.WriteAsync(Quote);
            }
        }

        private static bool ShouldQuote(string value)
        {
            if (value.IndexOfAny(EscapableChars) >= 0)
            {
                // Contains scapable characters
                return true;
            }

            if (value[0] == ' ' || value[value.Length - 1] == ' ')
            {
                // Starts with a space or ends with a space
                return true;
            }

            return false;
        }
    }
}
