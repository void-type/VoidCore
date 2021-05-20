using System;
using System.Linq;

namespace VoidCore.Model.Configuration
{
    /// <summary>
    /// Settings configuration extension methods for service collections.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Check a type to see if inherits from or is assignable to a base class or interface, including open generics.
        /// </summary>
        /// <param name="type">This type</param>
        /// <param name="targetType">The type to check the inheritance tree for</param>
        public static bool Implements(this Type type, Type targetType)
        {
            if (type.IsAssignableTo(targetType))
            {
                return true;
            }

            // Check desiredType against open generic interfaces
            if (targetType.IsInterface)
            {
                return type
                    .GetInterfaces()
                    .Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i)
                    .Contains(targetType);
            }

            // Check desiredType against open generic base types
            var baseType = type;
            do
            {
                if (baseType.IsGenericType && targetType == baseType.GetGenericTypeDefinition())
                {
                    return true;
                }

                baseType = baseType.BaseType;
            } while (baseType != null);

            return false;
        }
    }
}
