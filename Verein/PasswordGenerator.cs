using System;
using System.Linq;
using System.Text;

namespace Verein
{
    [Flags]
    public enum PasswordRules
    {
        AlphabeticCharacters,
        NumericCharacters,
        SpecialChararcters
    }

    public class PasswordGenerator
    {
        public string Generate(int length, PasswordRules rules)
        {
            string password = string.Empty;

            // Generate one third of length for Numeric and special each,
            // Fill the rest with alphanumeric characters
            int quarter = length / 4;

            if (rules.HasFlag(PasswordRules.NumericCharacters))
            {
                password += GenerateCharacters(quarter, PasswordRules.NumericCharacters);
            }

            if (rules.HasFlag(PasswordRules.SpecialChararcters))
            {
                password += GenerateCharacters(quarter, PasswordRules.SpecialChararcters);
            }

            password += GenerateCharacters(length - password.Length, PasswordRules.AlphabeticCharacters);
            return Shuffle(password);
        }

        private string GenerateCharacters(int length, PasswordRules rules)
        {
            const string alphabetic = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "1234567890";
            const string special = "!@#$%^&*()_-+=[{]};:<>|./?";

            switch (rules)
            {
                case PasswordRules.AlphabeticCharacters:
                    return GenerateRandom(length, alphabetic);
                case PasswordRules.NumericCharacters:
                    return GenerateRandom(length, numbers);
                case PasswordRules.SpecialChararcters:
                    return GenerateRandom(length, special);
                default:
                    return string.Empty;
            }
        }

        private string GenerateRandom(int length, string valid)
        {
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private string Shuffle(string s)
        {
            return new string(s.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
        }

    }
}