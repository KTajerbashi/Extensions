using System.Security.Cryptography;
using System.Text;

namespace SampleSource.Extensions;


/// <summary>
/// Provides extension methods for various hashing operations with custom output lengths.
/// </summary>
public static class HashingExtensions
{
    /// <summary>
    /// Generates a short secure hash using SHA256 with added entropy from GUID.
    /// Suitable for non-cryptographic purposes like generating unique identifiers.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <param name="length">The desired output length in bytes (1-32). Default is 16.</param>
    /// <returns>Base64 encoded truncated hash.</returns>
    /// <exception cref="ArgumentException">Thrown when length is invalid.</exception>
    public static string ShortSecureHash(this string input, int length = 16)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));

        if (length < 1 || length > 32)
            throw new ArgumentException("Length must be between 1 and 32 bytes", nameof(length));

        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + Guid.NewGuid()));

        byte[] truncated = new byte[length];
        Array.Copy(hash, truncated, length);

        return Convert.ToBase64String(truncated);
    }

    /// <summary>
    /// Generates an HMAC with custom output length.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <param name="key">The secret key for HMAC.</param>
    /// <param name="length">The desired output length in bytes (1-32).</param>
    /// <returns>Hexadecimal string representation of the truncated HMAC.</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    public static string HmacToLength(this string input, string key, int length)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));

        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        if (length < 1 || length > 32)
            throw new ArgumentException("Length must be between 1 and 32 bytes", nameof(length));

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));

        byte[] truncatedHash = new byte[length];
        Array.Copy(hashBytes, truncatedHash, length);

        return BitConverter.ToString(truncatedHash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// Generates an HMAC with custom output length using Base64 encoding.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <param name="key">The secret key for HMAC.</param>
    /// <param name="length">The desired output length in bytes (1-32).</param>
    /// <returns>Base64 encoded truncated HMAC.</returns>
    public static string HmacToLengthBase64(this string input, string key, int length)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));

        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        if (length < 1 || length > 32)
            throw new ArgumentException("Length must be between 1 and 32 bytes", nameof(length));

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));

        byte[] truncatedHash = new byte[length];
        Array.Copy(hashBytes, truncatedHash, length);

        return Convert.ToBase64String(truncatedHash);
    }

    /// <summary>
    /// Generates a SHA512 hash with custom output length.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <param name="length">The desired output length in bytes (1-64).</param>
    /// <returns>Hexadecimal string representation of the truncated hash.</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    public static string HashToLength(this string input, int length)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));

        if (length < 1 || length > 64)
            throw new ArgumentException("Length must be between 1 and 64 bytes", nameof(length));

        using var sha512 = SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));

        byte[] truncatedHash = new byte[length];
        Array.Copy(hashBytes, truncatedHash, length);

        return BitConverter.ToString(truncatedHash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// Generates a SHA512 hash with custom output length using Base64 encoding.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <param name="length">The desired output length in bytes (1-64).</param>
    /// <returns>Base64 encoded truncated hash.</returns>
    public static string HashToLengthBase64(this string input, int length)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));

        if (length < 1 || length > 64)
            throw new ArgumentException("Length must be between 1 and 64 bytes", nameof(length));

        using var sha512 = SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));

        byte[] truncatedHash = new byte[length];
        Array.Copy(hashBytes, truncatedHash, length);

        return Convert.ToBase64String(truncatedHash);
    }

    /// <summary>
    /// Generates a SHA256 hash with custom output length.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <param name="length">The desired output length in bytes (1-32).</param>
    /// <returns>Hexadecimal string representation of the truncated hash.</returns>
    public static string Sha256ToLength(this string input, int length)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));

        if (length < 1 || length > 32)
            throw new ArgumentException("Length must be between 1 and 32 bytes", nameof(length));

        using var sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

        byte[] truncatedHash = new byte[length];
        Array.Copy(hashBytes, truncatedHash, length);

        return BitConverter.ToString(truncatedHash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// Generates a secure random token of specified length.
    /// </summary>
    /// <param name="length">The desired token length in bytes.</param>
    /// <returns>Base64 encoded random bytes.</returns>
    public static string GenerateSecureToken(int length = 32)
    {
        if (length < 1)
            throw new ArgumentException("Length must be at least 1 byte", nameof(length));

        byte[] tokenBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);

        return Convert.ToBase64String(tokenBytes);
    }

    /// <summary>
    /// Verifies if a input matches the expected hash (timing-attack safe for HMAC).
    /// </summary>
    /// <param name="input">The input to verify.</param>
    /// <param name="hash">The expected hash.</param>
    /// <param name="key">The HMAC key (if using HMAC).</param>
    /// <returns>True if the input matches the hash.</returns>
    public static bool VerifyHash(this string input, string hash, string? key = null)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(hash))
            return false;

        string computedHash;

        if (key != null)
        {
            computedHash = input.HmacToLength(key, hash.Length / 2); // Hex chars: 2 per byte
        }
        else
        {
            computedHash = input.HashToLength(hash.Length / 2);
        }

        // Use constant-time comparison to prevent timing attacks
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedHash),
            Encoding.UTF8.GetBytes(hash)
        );
    }

    /// <summary>
    /// Generates a hash with salt for additional security.
    /// </summary>
    /// <param name="input">The input string to hash.</param>
    /// <param name="salt">The salt to add.</param>
    /// <param name="length">The desired output length in bytes.</param>
    /// <returns>Hexadecimal string representation of the hashed result.</returns>
    public static string HashWithSalt(this string input, string salt, int length = 32)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));

        if (string.IsNullOrEmpty(salt))
            throw new ArgumentException("Salt cannot be null or empty", nameof(salt));

        if (length < 1 || length > 64)
            throw new ArgumentException("Length must be between 1 and 64 bytes", nameof(length));

        using var sha512 = SHA512.Create();
        byte[] combinedBytes = Encoding.UTF8.GetBytes(input + salt);
        byte[] hashBytes = sha512.ComputeHash(combinedBytes);

        byte[] truncatedHash = new byte[length];
        Array.Copy(hashBytes, truncatedHash, length);

        return BitConverter.ToString(truncatedHash).Replace("-", "").ToLower();
    }
}
