using BirthdayWebsiteAPI.Exceptions;
using System.Text.RegularExpressions;

namespace BirthdayWebsiteAPI.Helpers
{
    public class PhoneNumberFormatterAndValidator
    {

        public string FormatWhatsappNumber(string whatsappNumber)
        {
            // Remove all non-numeric characters using regex
            string cleanedPhone = Regex.Replace(whatsappNumber, @"[^\d]", "");

            // If the phone number starts with "55" and has 13 digits, it already includes the country code
            if (cleanedPhone.Length == 13 && cleanedPhone.StartsWith("55"))
            {
                return "+" + cleanedPhone;
            }
            else if (cleanedPhone.Length == 11)
            {
                // If it's a valid 11-digit number (without +55), prepend "+55"
                return "+55" + cleanedPhone;
            }
            else
            {
                throw new WhatsappInvalidFormatException();
            }
        }

        public bool IsPhoneNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            var trimmed = input.Trim();

            // Remove all non-numeric characters using regex
            string cleanedPhone = Regex.Replace(trimmed, @"[^\d]", "");

            // Check if the cleaned phone number has the correct length (13 digits for +55XXXXXXXXXXX or 11 digits for XXXXXXXXXXX)
            if (cleanedPhone.Length == 11)
            {
                return true;  // Valid phone number with 11 digits (e.g., 81995084567)
            }
            else if (cleanedPhone.Length == 13 && cleanedPhone.StartsWith("55"))
            {
                return true;  // Valid phone number with +55 (e.g., +55819995084567)
            }
            else
            {
                return false;
            }
        }
    }
}
