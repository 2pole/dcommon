using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections;
using DCommon.Data;
using DCommon.Search;
using DCommon.Utility;

namespace DCommon
{
    /// <summary>
    /// Contains various extension methods for types.
    /// </summary>
    public static class TypeExtensions
    {
        public static bool IsEntity(this Type type)
        {
            return typeof(IEntity).IsAssignableFrom(type);
        }

        public static bool IsSearchCriteria(this Type type)
        {
            var criteriaType = typeof(SearchCriteria);
            return (type == criteriaType || criteriaType.IsAssignableFrom(type));
        }

        /// <summary>
        /// Determines if the given type implements <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>Returns the first concrete interface supported by the candidate type that
        /// closes the provided open generic service type.</summary>
        /// <param name="this">The type that is being checked for the interface.</param>
        /// <param name="openGeneric">The open generic type to locate.</param>
        /// <returns>The type of the interface.</returns>
        public static IEnumerable<Type> GetTypesThatClose(this Type @this, Type openGeneric)
        {
            return FindAssignableTypesThatClose(@this, openGeneric);
        }

        /// <summary>
        /// Looks for an interface on the candidate type that closes the provided open generic interface type.
        /// </summary>
        /// <param name="candidateType">The type that is being checked for the interface.</param>
        /// <param name="openGenericServiceType">The open generic service type to locate.</param>
        /// <returns>True if a closed implementation was found; otherwise false.</returns>
        static IEnumerable<Type> FindAssignableTypesThatClose(Type candidateType, Type openGenericServiceType)
        {
            return TypesAssignableFrom(candidateType)
                .Where(t => IsGenericTypeDefinedBy(t, openGenericServiceType));
        }

        static IEnumerable<Type> TypesAssignableFrom(Type candidateType)
        {
            return candidateType.GetInterfaces().Concat(
                Traverse.Across(candidateType, t => t.BaseType));
        }

        public static bool IsGenericTypeDefinedBy(this Type @this, Type openGeneric)
        {
            if (@this == null) throw new ArgumentNullException("this");
            if (openGeneric == null) throw new ArgumentNullException("openGeneric");

            return !@this.ContainsGenericParameters && @this.IsGenericType && @this.GetGenericTypeDefinition() == openGeneric;
        }

        public static bool IsDelegate(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return type.IsSubclassOf(typeof(Delegate));
        }

        public static bool IsCompatibleWith(this Type type, Type that)
        {

#if !(SILVERLIGHT || NET35)
            return type.IsEquivalentTo(that);
#else
            return type.Equals(that);
#endif
        }

        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        internal static object DefaultValue(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static MemberInfo FindPropertyOrField(this Type type, string memberName)
        {
            MemberInfo info = type.FindPropertyOrField(memberName, false);
            if (info == null)
            {
                info = type.FindPropertyOrField(memberName, true);
            }
            return info;
        }

        public static MemberInfo FindPropertyOrField(this Type type, string memberName, bool staticAccess)
        {
            BindingFlags bindingAttr = (BindingFlags.Public | BindingFlags.DeclaredOnly) | (!staticAccess ? BindingFlags.Instance : BindingFlags.Static);
            foreach (Type type2 in type.SelfAndBaseTypes())
            {
                MemberInfo[] infoArray = type2.FindMembers(MemberTypes.Property | MemberTypes.Field, bindingAttr, Type.FilterNameIgnoreCase, memberName);
                if (infoArray.Length != 0)
                {
                    return infoArray[0];
                }
            }
            return null;
        }

        internal static IEnumerable<Type> SelfAndBaseTypes(this Type type)
        {
            if (type.IsInterface)
            {
                List<Type> types = new List<Type>();
                AddInterface(types, type);
                return types;
            }
            return type.SelfAndBaseClasses();
        }

        private static void AddInterface(List<Type> types, Type type)
        {
            if (!types.Contains(type))
            {
                types.Add(type);
                foreach (Type type2 in type.GetInterfaces())
                {
                    AddInterface(types, type2);
                }
            }
        }

        internal static IEnumerable<Type> SelfAndBaseClasses(this Type type)
        {
            yield return type;

            Type baseType = type.BaseType;
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType;
            }
        }

        internal static string GetTypeName(this Type type)
        {
            Type nonNullableType = type.GetNonNullableType();
            string name = nonNullableType.Name;
            if (type != nonNullableType)
            {
                name = name + '?';
            }
            return name;
        }

        public static Type GetNonNullableType(this Type type)
        {
            return (!type.IsNullableType() ? type : type.GetGenericArguments()[0]);
        }

        public static bool IsEnumType(this Type type)
        {
            return type.GetNonNullableType().IsEnum;
        }

        public static int GetCompatibleHashCode(this Type type)
        {
#if !WINDOWS_PHONE
            if (type.IsImport)
                return type.GUID.GetHashCode();
#endif
            return type.GetHashCode();
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, Type[] genericTypeArgs, Type[] paramTypes, bool complain = true)
        {
            foreach (MethodInfo m in type.GetMethods())
            {
                if (m.Name == name)
                {
                    ParameterInfo[] pa = m.GetParameters();
                    if (pa.Length == paramTypes.Length)
                    {
                        MethodInfo c = m.MakeGenericMethod(genericTypeArgs);
                        if (c.GetParameters().Select(p => p.ParameterType).SequenceEqual(paramTypes))
                            return c;
                    }
                }
            }
            if (complain)
                throw new Exception("Could not find a method matching the signature " + type + "." + name +
                  "<" + String.Join(", ", genericTypeArgs.AsEnumerable()) + ">" +
                  "(" + String.Join(", ", paramTypes.AsEnumerable()) + ").");
            return null;
        }
    }
}
