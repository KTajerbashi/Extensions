namespace SampleSource.Extensions;

// Example usage class
public static class HashingExamples
{
    public static void DemonstrateUsage()
    {
        const string sampleText = "Hello World";
        const string secretKey = "my-secret-key";

        // Short secure hash
        string shortHash = sampleText.ShortSecureHash(16);
        Console.WriteLine($"Short Secure Hash: {shortHash}");

        // HMAC with custom length
        string hmacHash = sampleText.HmacToLength(secretKey, 16);
        Console.WriteLine($"HMAC Hash: {hmacHash}");

        // SHA512 with custom length
        string sha512Hash = sampleText.HashToLength(32);
        Console.WriteLine($"SHA512 Hash: {sha512Hash}");

        // Generate secure token
        string token = HashingExtensions.GenerateSecureToken(24);
        Console.WriteLine($"Secure Token: {token}");

        // Verify hash
        bool isValid = sampleText.VerifyHash(hmacHash, secretKey);
        Console.WriteLine($"Hash verification: {isValid}");

        // Hash with salt
        string saltedHash = sampleText.HashWithSalt("my-salt", 24);
        Console.WriteLine($"Salted Hash: {saltedHash}");
    }
}