using System.Reflection;

#if NETSTANDARD
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class ReflectionExtensions
    {
        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetTypeInfo().GetProperty(name);
        }

        public static bool IsAssignableFrom(this Type type, Type otherType)
        {
            return type.GetTypeInfo().IsAssignableFrom(otherType);
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments();
        }

        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(attributeType, inherit);
        }

        public static MethodInfo GetMethodInfo(this Delegate d)
        {
            return RuntimeReflectionExtensions.GetMethodInfo(d);
        }

        public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags bindingFlags)
        {
            return type.GetTypeInfo().GetMethod(methodName, bindingFlags);
        }

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags bindingFlags)
        {
            return type.GetTypeInfo().GetProperties(bindingFlags);
        }
    }
}
#else
// ReSharper disable once CheckNamespace
namespace System
{
    internal static class ReflectionExtensions
    {
        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }

        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }

        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        public static MethodInfo GetMethodInfo(this Delegate d)
        {
            return d.Method;
        }
    }
}
#endif