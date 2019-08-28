using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Altairis.Services.PwnedPasswordsValidator.Tests {
    public class PwnedTests {
        [Fact]
        public async Task PwnedPassword() {
            var validator = new PwnedPasswordsValidator<object>();
            var result = await validator.ValidateAsync(null, null, "password");
            Assert.Contains(result.Errors, e => e.Code.Equals("PasswordIsPwned", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task NotPwnedPassword() {
            var validator = new PwnedPasswordsValidator<object>();
            var result = await validator.ValidateAsync(null, null, "xHE5+9Vg+Y/yr#hXjhRP");
            Assert.Equal(result, IdentityResult.Success);
        }

    }
}
