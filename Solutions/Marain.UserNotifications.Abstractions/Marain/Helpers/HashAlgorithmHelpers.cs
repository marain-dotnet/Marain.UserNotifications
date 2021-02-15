// <copyright file="HashAlgorithmHelpers.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.Helpers
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Hashing algorithms helper class.
    /// </summary>
    public static class HashAlgorithmHelpers
    {
        /// <summary>
        /// Get hash.
        /// </summary>
        /// <param name="input">The input string which needs to be hashed.</param>
        /// <returns>The byte array.</returns>
        public static byte[] GetSHA256Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}
