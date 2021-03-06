﻿namespace VoidCore.AspNet.Security
{
    /// <summary>
    /// A wrapper for CspDirectiveBuilder that adds methods for "src" directives.
    /// Adapted from https://www.c-sharpcorner.com/article/using-csp-header-in-asp-net-core-2-0/
    /// </summary>
    public sealed class CspSourceDirectiveBuilder
    {
        private readonly CspDirectiveBuilder _internalBuilder;

        internal CspSourceDirectiveBuilder(CspDirectiveBuilder internalBuilder)
        {
            _internalBuilder = internalBuilder;
        }

        /// <inheritdoc/>
        public CspSourceDirectiveBuilder Allow(string source)
        {
            _internalBuilder.Allow(source);
            return this;
        }

        /// <inheritdoc/>
        public CspSourceDirectiveBuilder AllowAny()
        {
            _internalBuilder.AllowAny();
            return this;
        }

        /// <inheritdoc/>
        public CspSourceDirectiveBuilder AllowNone()
        {
            _internalBuilder.AllowNone();
            return this;
        }

        /// <inheritdoc/>
        public CspSourceDirectiveBuilder AllowSelf()
        {
            _internalBuilder.AllowSelf();
            return this;
        }

        /// <summary>
        /// Allow inline sources defined between tags.
        /// </summary>
        public CspSourceDirectiveBuilder AllowUnsafeInline() => Allow("'unsafe-inline'");

        /// <summary>
        /// Allow the use of eval() to create code from strings.
        /// </summary>
        public CspSourceDirectiveBuilder AllowUnsafeEval() => Allow("'unsafe-eval'");

        /// <summary>
        /// Use a nonce generated by the server upon every request to whitelist sources.
        /// </summary>
        /// <param name="base64Value">The base 64 nonce value used to identify the source.</param>
        public CspSourceDirectiveBuilder AllowNonce(string base64Value) => Allow($"'nonce-{base64Value}'");

        /// <summary>
        /// Use a sha256, sha384, sha512 hash to identify scripts or styles.
        /// </summary>
        /// <param name="algorithm">The string name of the algorithm used to derive the hash.</param>
        /// <param name="base64Value">The base 64 value of the hash.</param>
        public CspSourceDirectiveBuilder AllowHash(string algorithm, string base64Value) => Allow($"'{algorithm}-{base64Value}'");
    }
}
