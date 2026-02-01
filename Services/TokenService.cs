using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace PasswordPortalApp.Services
{
    public static class TokenService
    {
        // Token → metadata
        private static readonly ConcurrentDictionary<string, TokenRecord> _tokens
            = new();

        private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(15);

        // =========================
        // CREATE TOKEN
        // =========================
        public static string GenerateToken(string userIdentifier)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            var token = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            var record = new TokenRecord
            {
                UserIdentifier = userIdentifier,
                ExpirationUtc = DateTime.UtcNow.Add(TokenLifetime),
                Used = false
            };

            _tokens[token] = record;
            return token;
        }

        // =========================
        // VALIDATE TOKEN
        // =========================
        public static bool ValidateToken(string token, out string userIdentifier)
        {
            userIdentifier = string.Empty;

            if (!_tokens.TryGetValue(token, out var record))
                return false;

            if (record.Used)
                return false;

            if (DateTime.UtcNow > record.ExpirationUtc)
                return false;

            userIdentifier = record.UserIdentifier;
            return true;
        }

        // =========================
        // CONSUME TOKEN (ONE-TIME USE)
        // =========================
        public static bool ConsumeToken(string token)
        {
            if (!_tokens.TryGetValue(token, out var record))
                return false;

            if (record.Used)
                return false;

            record.Used = true;
            return true;
        }

        // =========================
        // CLEANUP (OPTIONAL)
        // =========================
        public static void CleanupExpiredTokens()
        {
            foreach (var kvp in _tokens)
            {
                if (DateTime.UtcNow > kvp.Value.ExpirationUtc)
                {
                    _tokens.TryRemove(kvp.Key, out _);
                }
            }
        }

        // =========================
        // INTERNAL MODEL
        // =========================
        private class TokenRecord
        {
            public string UserIdentifier { get; set; } = string.Empty;
            public DateTime ExpirationUtc { get; set; }
            public bool Used { get; set; }
        }
    }
}
