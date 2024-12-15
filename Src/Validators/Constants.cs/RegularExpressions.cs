using System.Text.RegularExpressions;

namespace UserManagementService.Src.Validators.Constants
{
        public static partial class RegularExpressions
    {
        public const string PasswordValidation = @"^(?=.*[a-zA-Z])(?=.*\d).+$";

        [GeneratedRegex("^([0-9]+-[0-9K])$", RegexOptions.Compiled)]
        public static partial Regex RutRegex();

        [GeneratedRegex("^([a-zA-Z]+\\.)*ucn\\.cl$", RegexOptions.Compiled)]
        public static partial Regex UCNEmailDomainRegex();
    }
}