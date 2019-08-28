﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Net.Http;

namespace Altairis.Services.PwnedPasswordsValidator {
    public class PwnedPasswordsValidator<TUser> : IPasswordValidator<TUser> where TUser : class {
        private const string ApiBaseUrl = "https://api.pwnedpasswords.com/range/";

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password) {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            // Compute SHA 1 hash of password
            var passwordHash = this.ComputeSha1(password);
            var hashPrefix = passwordHash.Substring(0, 5);
            var apiUrl = ApiBaseUrl + hashPrefix;

            // Ask PwnedPasswords for result
            string text;
            using (var wc = new System.Net.WebClient()) {
                text = await wc.DownloadStringTaskAsync(apiUrl);
            }

            // Try to find password entered by user
            var lines = text.Split(new[] { "\r\n" }, StringSplitOptions.None);
            var hashRest = passwordHash.Substring(5);
            var wasPwned = lines.Any(l => l.StartsWith(hashRest, StringComparison.OrdinalIgnoreCase));

            // Return IdentityResult
            return wasPwned
                ? IdentityResult.Failed(new IdentityError {
                    Code = "PasswordIsPwned",
                    Description = "Password was found in haveibeenpwned.com password dumps."
                })
                : IdentityResult.Success;
        }

        private string ComputeSha1(string password) {
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
