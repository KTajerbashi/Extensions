using Microsoft.AspNetCore.Mvc;
using BCryptCode = BCrypt.Net.BCrypt;
namespace SampleSource.Controllers;

public class BCryptController : AuthController
{
    [HttpGet("{text}")]
    public IActionResult HashPassword(string text)
    {
        // Hash with the default work factor (currently 11)
        string hashedPassword = BCryptCode.HashPassword(text, workFactor: 12);
        string passwordAttempt = text;

        bool isPasswordCorrect = BCryptCode.Verify(passwordAttempt, hashedPassword);

        if (isPasswordCorrect)
        {
            Console.WriteLine("Password is valid! Grant access.");
        }
        else
        {
            Console.WriteLine("Invalid password. Deny access.");
        }
        return Ok(new
        {
            text = text,
            hashedPassword = hashedPassword,
            passwordAttempt = passwordAttempt,
            isPasswordCorrect = isPasswordCorrect,
        });
    }
}