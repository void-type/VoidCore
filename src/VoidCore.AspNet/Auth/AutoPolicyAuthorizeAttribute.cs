using Microsoft.AspNetCore.Authorization;

namespace VoidCore.AspNet.Auth;

/// <summary>
/// Derivatives of this class will have their policies set by convention from the name of the class.
/// Ex: UserOnly will authorize the policy of "User". A class name that doesn't end in "Only" is used as the whole policy name.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AutoPolicyAuthorizeAttribute : AuthorizeAttribute
{
    private const string Suffix = "Only";

    /// <summary>
    /// Construct a new AutoNamingAuthorizeAttribute with a policy set by convention.
    /// </summary>
    protected AutoPolicyAuthorizeAttribute()
    {
        var name = GetType().Name;

        // Only a trailing "Only" is removed, so ReadOnlyUser stays ReadOnlyUser instead of becoming "Read". An empty policy would
        // fall back to the default policy and weaken authorization, so a class named just "Only" is rejected.
        var policy = name.EndsWith(Suffix, StringComparison.OrdinalIgnoreCase)
            ? name[..^Suffix.Length]
            : name;

        if (string.IsNullOrWhiteSpace(policy))
        {
            throw new InvalidOperationException($"{name} must have a name before the \"{Suffix}\" suffix to set its authorization policy.");
        }

        Policy = policy;
    }
}
