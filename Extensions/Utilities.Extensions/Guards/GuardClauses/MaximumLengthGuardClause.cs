using System.Collections;
using Utilities.Extensions.Guards;

namespace Utilities.Extensions.Guards.GuardClauses;

public static class MaximumLengthGuardClause
{
    public static void MaximumLength(this Guard guard, string value, int maximumLength, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value.Length > maximumLength)
            throw new InvalidOperationException(message);
    }
    public static void MaximumLength(this Guard guard, ICollection value, int maximumLength, string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        if (value.Count > maximumLength)
            throw new InvalidOperationException(message);
    }
}
