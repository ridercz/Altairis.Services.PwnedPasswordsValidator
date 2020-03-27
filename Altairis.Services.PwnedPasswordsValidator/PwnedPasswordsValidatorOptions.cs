using System;

namespace Altairis.Services.PwnedPasswordsValidator {
    public class PwnedPasswordsValidatorOptions {
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(5);
    }
}
