using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBarn.Utilities
{
    /// <summary>
    /// Validate a ISBN string
    /// </summary>
    public static class Isbn
    {
        public enum IsbnType
        {
            ISBN10,
            ISBN13,
            UNKNOWN
        }

        /// <summary>
        /// Validate ISBN10
        /// </summary>
        /// <param name="isbn10"></param>
        /// <returns></returns>
        public static bool IsValidIsbn10(string isbn10)
        {
            if (string.IsNullOrEmpty(isbn10))
            {
                return false;
            }

            if (isbn10.Contains("-"))
            {
                isbn10 = isbn10.Replace("-", "");
            }

            if (isbn10.Length != 10)
            {
                return false;
            }

            long temp;
            if (!long.TryParse(isbn10.Substring(0, isbn10.Length - 1), out temp))
            {
                return false;
            }

            var sum = 0;
            for (var i = 0; i < 9; i++)
            {
                sum += (isbn10[i] - '0') * (i + 1);
            }

            var result = false;
            var remainder = sum % 11;
            var lastChar = isbn10[isbn10.Length - 1];

            if (lastChar == 'X')
            {
                result = (remainder == 10);
            }
            else if (int.TryParse(lastChar.ToString(), out sum))
            {
                result = (remainder == lastChar - '0');
            }

            return result;
        }

        /// <summary>
        /// Validate ISBN13
        /// </summary>
        /// <param name="isbn13"></param>
        /// <returns></returns>
        public static bool IsValidIsbn13(string isbn13)
        {
            if (string.IsNullOrEmpty(isbn13))
            {
                return false;
            }

            if (isbn13.Contains("-"))
            {
                isbn13 = isbn13.Replace("-", "");
            }

            if (isbn13.Length != 13)
            {
                return false;
            }

            long temp;
            if (!long.TryParse(isbn13, out temp))
            {
                return false;
            }

            var sum = 0;
            for (var i = 0; i < 12; i++)
            {
                sum += (isbn13[i] - '0') * (i % 2 == 1 ? 3 : 1);
            }

            var remainder = sum % 10;
            var checkDigit = 10 - remainder;
            if (checkDigit == 10)
            {
                checkDigit = 0;
            }
            var result = (checkDigit == isbn13[12] - '0');
            return result;
        }

        public static IsbnType GetIsbnType(this string isbn)
        {
            if (IsValidIsbn10(isbn))
            {
                return IsbnType.ISBN10;
            }
            else if (IsValidIsbn13(isbn))
            {
                return IsbnType.ISBN13;
            }
            else
            {
                return IsbnType.UNKNOWN;
            }
        }

        public static bool TryParseIsbn(this string isbn, out string parsed)
        {
            if (isbn.Contains("-"))
            {
                isbn = isbn.Replace("-", "");
            }

            if (IsValidIsbn10(isbn))
            {
                parsed = isbn;
                return true;
            }
            else if (IsValidIsbn13(isbn))
            {
                parsed = isbn;
                return true;
            }
            else
            {
                parsed = null;
                return false;
            }
        }

        public static bool IsValidIsbn(string isbn) => IsValidIsbn10(isbn) || IsValidIsbn13(isbn);
        public static string NormalizeIsbn(string isbn) => isbn.Contains("-") ? isbn.Replace("-", "") : isbn;

        public static string ConvertTo10(string isbn13)
        {
            string isbn10 = string.Empty;
            long temp;

            if (!(string.IsNullOrEmpty(isbn13) && isbn13.Length == 13 && long.TryParse(isbn13, out temp)))
            {
                isbn10 = isbn13.Substring(3, 9);
                int sum = 0;
                for (int i = 0; i <= 8; i++)
                {
                    sum += Int32.Parse(isbn10[i].ToString()) * (i + 1);
                }
                int result = sum % 11;
                char checkDigit = (result > 9) ? 'X' : result.ToString()[0];
                isbn10 += checkDigit;
            }

            return isbn10;
        }

        public static string ConvertTo13(string isbn10)
        {
            string isbn13 = string.Empty;
            long temp;
            if (!(string.IsNullOrEmpty(isbn10) && isbn10.Length == 10 && long.TryParse(isbn10.Substring(0, 9), out temp)))
            {
                int result = 0;
                isbn13 = "978" + isbn10.Substring(0, 9);
                for (int i = 0; i < isbn13.Length; i++)
                {
                    result += int.Parse(isbn13[i].ToString()) * ((i % 2 == 0) ? 1 : 3);
                }
                int checkDigit = (10 - (result % 10)) % 10;
                isbn13 += checkDigit.ToString();
            }
            return isbn13;
        }
    }
}
