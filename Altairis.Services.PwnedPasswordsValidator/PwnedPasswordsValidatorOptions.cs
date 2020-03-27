using System;

namespace Altairis.Services.PwnedPasswordsValidator {
    public class PwnedPasswordsValidatorOptions {
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(5);

        public Func<string> GetLocalizedErrorMessage { get; set; } = () => "Password was found in haveibeenpwned.com password dumps.";
    }
}
