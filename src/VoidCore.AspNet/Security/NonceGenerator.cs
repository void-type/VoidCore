using System;
using System.Security.Cryptography;

namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// Generate a nonce to be used by Content Security Policy.
    /// Adopted from https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders
    /// </summary>
    internal class NonceGenerator : IDisposable
    {
        private readonly RandomNumberGenerator _random;
        private bool _disposedValue;

        /// <summary>
        /// Construct a new NonceGenerator
        /// </summary>
        public NonceGenerator()
        {
            _random = RandomNumberGenerator.Create();
        }

        /// <summary>
        /// Get a new Nonce
        /// </summary>
        public string GetNonce()
        {
            var bytes = new byte[32];
            _random.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _random.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
