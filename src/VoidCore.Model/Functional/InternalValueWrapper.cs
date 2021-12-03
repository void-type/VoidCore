using System.Diagnostics.CodeAnalysis;
using VoidCore.Model.Guards;

namespace VoidCore.Model.Functional;

/// <summary>
/// This class wraps the inner value to allow Maybe and generic Result to work with non-nullable value types. We can set a reference of
/// this class to null whereas we can't do that with non-nullable value types.
/// </summary>
internal class InternalValueWrapper<T>
{
    [NotNull]
    internal readonly T Value;

    internal InternalValueWrapper(T value)
    {
        Value = value.EnsureNotNull(nameof(value));
    }
}
