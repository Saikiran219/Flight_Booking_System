using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Services
{
    public class InMemoryTokenStore
    {
        private static readonly ConcurrentDictionary<string, (string Token, DateTime Expiration)> _tokens = new();

        public void StoreToken(string email, string token, TimeSpan expiration)
        {
            var expirationTime = DateTime.UtcNow.Add(expiration);
            _tokens[email] = (token, expirationTime);
        }

        public (string Token, DateTime Expiration)? GetToken(string email)
        {
            _tokens.TryGetValue(email, out var tokenInfo);
            return tokenInfo;
        }

        public void InvalidateToken(string email)
        {
            _tokens.TryRemove(email, out _);
        }
    }
}