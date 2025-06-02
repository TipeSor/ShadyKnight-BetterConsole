using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BetterConsole
{
    public static class ReflectionUtility
    {
        public const BindingFlags FULL_BINDING = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static Type[] GetSubclassesOf(Type baseType)
        {
            return [.. AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsSubclassOf(baseType))];
        }

        public static (Type, T)[] GetClassesWithAttribute<T>() where T : Attribute
        {
            return GetClassesWithAttribute<T>(
                    AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(static a => a.GetTypes()));
        }

        public static (Type, T)[] GetClassesWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
        {
            return [.. types
                    .Select(static t => TryGetCustomAttribute(t, out T attr) ? (t, attr) : default)
                    .Where(static x => x != default)];
        }

        public static (MethodInfo, T)[] GetMethodsWithAttribute<T>(BindingFlags flags = FULL_BINDING) where T : Attribute
        {
            return GetMethodsWithAttribute<T>(
                    AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(static a => a.GetTypes()), flags);
        }

        public static (MethodInfo, T)[] GetMethodsWithAttribute<T>(IEnumerable<Type> types, BindingFlags flags = FULL_BINDING) where T : Attribute
        {
            return [.. types
                    .SelectMany(t => t.GetMethods(flags))
                    .Select(static m => TryGetCustomAttribute(m, out T attr) ? (m, attr) : default)
                    .Where(static x => x != default)];
        }

        public static (FieldInfo, T)[] GetFieldsWithAttribute<T>(Type type, BindingFlags flags = FULL_BINDING) where T : Attribute
        {
            return [.. type.GetFields(flags)
                    .Select(static t => TryGetCustomAttribute(t, out T attr) ? (t, attr) : default)
                    .Where(static x => x != default)];
        }

        public static (PropertyInfo, T)[] GetPropertiesWithAttribute<T>(Type type, BindingFlags flags = FULL_BINDING) where T : Attribute
        {
            return [.. type.GetProperties(flags)
                    .Select(static p => TryGetCustomAttribute(p, out T attr) ? (p, attr) : default)
                    .Where(static x => x != default)];
        }


        public static PropertyInfo FindProperty(Type type, string propertyName, bool throwNotFound = true, BindingFlags flags = FULL_BINDING)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            PropertyInfo property = type.GetProperty(propertyName, flags);
            return property == null && throwNotFound ? throw new MissingMemberException(type.FullName, propertyName) : property;
        }

        public static void SetInternalProperty(object obj, string propertyName, object value)
        {
            FindProperty(obj.GetType(), propertyName).SetValue(obj, value, null);
        }

        public static bool TryGetCustomAttribute<T>(MemberInfo element, out T attribute) where T : Attribute
        {
            try
            {
                if (Attribute.IsDefined(element, typeof(T)))
                {
                    attribute = element.GetCustomAttribute<T>();
                    return attribute != null;
                }
                attribute = default;
                return false;
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError(ex);
                Plugin.Logger.LogError($"Failed to get custom attribute of type {typeof(T)} from {element}, see previous message");
                attribute = default;
                return false;
            }
        }
    }
}
