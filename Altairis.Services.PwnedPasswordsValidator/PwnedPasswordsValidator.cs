using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Altairis.Services.PwnedPasswordsValidator {
    public class PwnedPasswordsValidator<TUser> : IPasswordValidator<TUser> where TUser : class {
        private const string ApiBaseUrl = "https://api.pwnedpasswords.com/range/";

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password) {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            // Compute SHA 1 hash of password
            var passwordHash = this.ComputeHashString(password);
            var hashPrefix = passwordHash.Substring(0, 5);
            var apiUrl = ApiBaseUrl + hashPrefix;

            // Ask PwnedPasswords for result
            var text = await this.DownloadString(apiUrl);

            // Try to find password entered by user
            var hashRest = passwordHash.Substring(5);
            var wasPwned = text.Contains(hashRest);

            // Return IdentityResult
            return wasPwned
                ? IdentityResult.Failed(new IdentityError {
                    Code = "PasswordIsPwned",
                    Description = "Password was found in haveibeenpwned.com password dumps."
                })
                : IdentityResult.Success;
        }

        private async Task<string> DownloadString(string url) {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(url));

            using (var hc = new System.Net.Http.HttpClient()) {
                using (var response = await hc.GetAsync(url)) {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        private string ComputeHashString(string password) {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            var passwordBytes = Encoding.UTF8.GetBytes(password);

            using (var sha = SHA1Managed.Create()) {
                var hashBytes = sha.ComputeHash(passwordBytes);
                return string.Join(string.Empty, hashBytes.Select(x => x.ToString("X2")));
            }
        }

    }
}
