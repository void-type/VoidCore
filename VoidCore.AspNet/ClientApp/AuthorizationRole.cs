namespace VoidCore.AspNet.ClientApp
{
    /// <summary>
    /// A role to authorize actions against.
    /// </summary>
    public class AuthorizationRole
    {
        /// <summary>
        /// The name of the role. Typically the name of an AD group that fulfills the policy.
        /// </summary>
        public string Name { get; set; }
    }
}
