using System;

namespace API.Helpers
{
    public static class Utilities
    {
        public static string RandomString()
        {
            var rand = new Random();
            var length = rand.Next(10, 101); //returns random number between 10-100
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[length];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }

        public static bool IsUppercaseLetter(char input)
        {
            return Char.IsUpper(input);
        }

        public static bool IsLetter(char input)
        {
            return Char.IsLetter(input);
        }

        public static bool IsNumber(char input)
        {
            return Char.IsDigit(input);
        }

        public static bool IsHyphen(char input)
        {
            return Char.Equals('-', input);
        }

        public static bool IsUnderscore(char input)
        {
            return Char.Equals('_', input);
        }

        public static bool IsValidMedicationName(string name)
        {
            var result = false;
            for (var i = 0; i < name.Length; i++)
            {
                var item = name[i];
                result = IsLetter(item) || IsNumber(item) || IsHyphen(item) || IsUnderscore(item);
                if (result)
                {
                    result = true;
                    continue;
                }
                result = false;
                break;
            }
            return result;
        }

        public static bool IsValidMedicationCode(string code)
        {
            var result = false;
            for (var i = 0; i < code.Length; i++)
            {
                var item = code[i];
                result = IsUppercaseLetter(item) || IsUnderscore(item) || IsNumber(item);
                if (result)
                {
                    result = true;
                    continue;
                }
                result = false;
                break;
            }
            return result;
        }
    }
}
