using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.Services.PwnedPasswordsValidator {
    public class PwnedPasswordsValidatorOptions {
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(5);
    }
}
