using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using SampleSource.Extensions;
namespace SampleSource.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BCryptTestController : ControllerBase
{
    private readonly ILogger<BCryptTestController> _logger;

    public BCryptTestController(ILogger<BCryptTestController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Basic BCrypt hashing and verification
    /// </summary>
    [HttpGet("basic/{password}")]
    public IActionResult BasicBcrypt(string password)
    {
        try
        {
            // Hash the password
            string hashedPassword = BCryptCode.HashPassword(password);

            // Verify the password
            bool isValid = BCryptCode.Verify(password, hashedPassword);

            return Ok(new
            {
                OriginalPassword = password,
                HashedPassword = hashedPassword,
                HashLength = hashedPassword.Length,
                VerificationResult = isValid,
                Algorithm = "BCrypt",
                WorkFactor = ExtractWorkFactor(hashedPassword)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in BasicBcrypt");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Test BCrypt with different work factors
    /// </summary>
    [HttpGet("workfactor-test/{password}")]
    public IActionResult WorkFactorTest(string password)
    {
        var results = new List<object>();
        var workFactors = new[] { 10, 11, 12, 13, 14 };

        foreach (int workFactor in workFactors)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            string hashedPassword = BCryptCode.HashPassword(password, workFactor);
            bool isValid = BCryptCode.Verify(password, hashedPassword);

            stopwatch.Stop();

            results.Add(new
            {
                WorkFactor = workFactor,
                Hash = hashedPassword,
                Length = hashedPassword.Length,
                Verification = isValid,
                TimeMs = stopwatch.ElapsedMilliseconds
            });
        }

        return Ok(results);
    }

    /// <summary>
    /// Compare different passwords against the same hash
    /// </summary>
    [HttpGet("verification-test/{correctPassword}")]
    public IActionResult VerificationTest(string correctPassword, [FromQuery] string[] testPasswords)
    {
        if (testPasswords == null || testPasswords.Length == 0)
        {
            testPasswords = new[] { correctPassword, "wrong1", "wrong2", "123456", "" };
        }

        string hashedCorrect = BCryptCode.HashPassword(correctPassword);
        var results = new List<object>();

        foreach (var testPassword in testPasswords)
        {
            bool isValid = BCryptCode.Verify(testPassword, hashedCorrect);

            results.Add(new
            {
                TestPassword = testPassword,
                IsCorrect = isValid,
                IsEmpty = string.IsNullOrEmpty(testPassword)
            });
        }

        return Ok(new
        {
            CorrectPassword = correctPassword,
            Hash = hashedCorrect,
            Tests = results
        });
    }

    /// <summary>
    /// Test enhanced BCrypt version
    /// </summary>
    [HttpGet("enhanced/{password}")]
    public IActionResult EnhancedBcrypt(string password)
    {
        try
        {
            string enhancedHash = BCryptCode.EnhancedHashPassword(password);
            bool enhancedVerify = BCryptCode.EnhancedVerify(password, enhancedHash);
            bool standardVerify = BCryptCode.Verify(password, enhancedHash);

            return Ok(new
            {
                Password = password,
                EnhancedHash = enhancedHash,
                EnhancedVerification = enhancedVerify,
                StandardVerification = standardVerify,
                HashType = enhancedHash.StartsWith("$2b$") ? "Enhanced ($2b$)" : "Standard ($2a$)",
                WorkFactor = ExtractWorkFactor(enhancedHash)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in EnhancedBcrypt");
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Performance test - hash multiple times and measure average time
    /// </summary>
    [HttpGet("performance-test/{password}")]
    public IActionResult PerformanceTest(string password, [FromQuery] int iterations = 10, [FromQuery] int workFactor = 11)
    {
        var times = new List<long>();
        var hashes = new List<string>();

        for (int i = 0; i < iterations; i++)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            string hash = BCryptCode.HashPassword(password, workFactor);
            stopwatch.Stop();

            times.Add(stopwatch.ElapsedMilliseconds);
            hashes.Add(hash);
        }

        return Ok(new
        {
            Password = password,
            WorkFactor = workFactor,
            Iterations = iterations,
            AverageTimeMs = times.Average(),
            MinTimeMs = times.Min(),
            MaxTimeMs = times.Max(),
            TotalTimeMs = times.Sum(),
            SampleHash = hashes.First()
        });
    }

    /// <summary>
    /// Test error handling with invalid hashes
    /// </summary>
    [HttpGet("error-test")]
    public IActionResult ErrorTest()
    {
        var tests = new List<object>();

        // Test 1: Malformed hash
        try
        {
            BCryptCode.Verify("password", "invalid-hash-format");
            tests.Add(new { Test = "Malformed hash", Success = true });
        }
        catch (SaltParseException ex)
        {
            tests.Add(new { Test = "Malformed hash", Success = false, Error = ex.Message });
        }

        // Test 2: Empty password
        try
        {
            string hash = BCryptCode.HashPassword("");
            bool result = BCryptCode.Verify("", hash);
            tests.Add(new { Test = "Empty password", Success = true, Result = result });
        }
        catch (Exception ex)
        {
            tests.Add(new { Test = "Empty password", Success = false, Error = ex.Message });
        }

        // Test 3: Null password
        try
        {
            string hash = BCryptCode.HashPassword(null);
            tests.Add(new { Test = "Null password", Success = true });
        }
        catch (Exception ex)
        {
            tests.Add(new { Test = "Null password", Success = false, Error = ex.Message });
        }

        return Ok(tests);
    }

    /// <summary>
    /// Compare BCrypt with other hashing methods
    /// </summary>
    [HttpGet("comparison/{password}")]
    public IActionResult ComparisonTest(string password)
    {
        // BCrypt
        var bcryptStopwatch = System.Diagnostics.Stopwatch.StartNew();
        string bcryptHash = BCryptCode.HashPassword(password);
        bcryptStopwatch.Stop();

        // SHA256 (for comparison - not for passwords!)
        var shaStopwatch = System.Diagnostics.Stopwatch.StartNew();
        string shaHash = password.HashToLength(32);
        shaStopwatch.Stop();

        return Ok(new
        {
            Password = password,
            Algorithms = new[]
            {
                new
                {
                    Name = "BCrypt",
                    Hash = bcryptHash,
                    Length = bcryptHash.Length,
                    TimeMs = bcryptStopwatch.ElapsedMilliseconds,
                    RecommendedForPasswords = true
                },
                new
                {
                    Name = "SHA256",
                    Hash = shaHash,
                    Length = shaHash.Length,
                    TimeMs = shaStopwatch.ElapsedMilliseconds,
                    RecommendedForPasswords = false
                }
            }
        });
    }

    /// <summary>
    /// Generate multiple hashes for the same password to demonstrate salting
    /// </summary>
    [HttpGet("salting-demo/{password}")]
    public IActionResult SaltingDemo(string password, [FromQuery] int count = 5)
    {
        var hashes = new List<string>();

        for (int i = 0; i < count; i++)
        {
            hashes.Add(BCryptCode.HashPassword(password));
        }

        // Check if all hashes are different (they should be, due to salting)
        bool allUnique = hashes.Distinct().Count() == hashes.Count;
        bool allVerify = hashes.All(h => BCryptCode.Verify(password, h));

        return Ok(new
        {
            Password = password,
            Hashes = hashes,
            AllHashesUnique = allUnique,
            AllHashesVerify = allVerify,
            HashCount = hashes.Count
        });
    }

    /// <summary>
    /// Extract work factor from BCrypt hash string
    /// </summary>
    private static int ExtractWorkFactor(string hashedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword) || !hashedPassword.StartsWith("$2"))
            return -1;

        try
        {
            // BCrypt hash format: $2a$workfactor$salt+hash
            var parts = hashedPassword.Split('$');
            if (parts.Length >= 4 && int.TryParse(parts[2], out int workFactor))
            {
                return workFactor;
            }
        }
        catch
        {
            // Ignore parsing errors
        }

        return -1;
    }

    /// <summary>
    /// Health check endpoint to verify BCrypt is working
    /// </summary>
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        try
        {
            const string testPassword = "health-check-password";
            string hash = BCryptCode.HashPassword(testPassword);
            bool isValid = BCryptCode.Verify(testPassword, hash);

            return Ok(new
            {
                Status = "Healthy",
                BCryptWorking = isValid,
                Timestamp = DateTime.UtcNow,
                Version = "BCryptCode.Net-Next"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = "Unhealthy",
                Error = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}


