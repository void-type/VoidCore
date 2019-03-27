namespace VoidCore.Domain
{
    /// <summary>
    /// This class wraps the inner value to allow Maybe to work with non-nullable value types. We can set a reference to
    /// this type to null whereas we can't do that with non-nullable value types.
    /// </summary>
    internal class MaybeValueWrapper<T>
    {
        internal readonly T Value;

        internal MaybeValueWrapper(T value)
        {
            Value = value;
        }
    }
}
