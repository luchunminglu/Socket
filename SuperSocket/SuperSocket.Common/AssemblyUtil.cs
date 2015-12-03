using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Common
{
    /// <summary>
    /// Assembly帮助类
    /// </summary>
    public static class AssemblyUtil
    {

        /// <summary>
        /// Creates the instance from type name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string type)
        {
            return CreateInstance<T>(type, new object[0]);
        }

        /// <summary>
        /// 创建类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(string type, object[] parameters)
        {
            Type instanceType = Type.GetType(type,true);
            if (instanceType == null)
            {
                throw new Exception("type {0} not found".FormatWith(type));
            }

            object instance = Activator.CreateInstance(instanceType, parameters);
            return (T)instance;
        }

        /// <summary>
        /// 返回字符串表示的类型，
        /// return matched generic type without checking generic type parameters in the name.
        /// </summary>
        /// <param name="fullTypeName"></param>
        /// <param name="throwOnError">if true, throw an error</param>
        /// <param name="ignoreCase">if true, ignor case</param>
        /// <returns></returns>
        public static Type GetType(string fullTypeName, bool throwOnError, bool ignoreCase)
        {
            var targetType = Type.GetType(fullTypeName, throwOnError, ignoreCase);

            if (targetType != null)
            {
                return targetType;
            }
            
            try
            {
                //System.Collections.Generic.List`1[System.IO.FileInfo]
                //System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer
                var names = fullTypeName.Split(',');
                var assemblyName = names[1].Trim();

                var assembly = Assembly.Load(assemblyName);
                var typeNamePrefix = names[0].Trim() + "`";

                var matchedTypes =
                    assembly.GetExportedTypes()
                        .Where(
                            t =>
                                t.IsGenericType &&
                                t.FullName.StartsWith(typeNamePrefix, ignoreCase, CultureInfo.InvariantCulture))
                        .ToArray();

                if (matchedTypes.Length != 1)
                {
                    return null;
                }

                return matchedTypes[0];

            }
            catch (Exception e)
            {
                if (throwOnError)
                    throw e;

                return null;
            }
        }

        /// <summary>
        /// Gets the implement types from assembly
        /// </summary>
        /// <typeparam name="TBaseType"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetImplementTypes<TBaseType>(this Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(t => t.IsSubclassOf(typeof (TBaseType)) && t.IsClass && !t.IsAbstract);
        }

        /// <summary>
        /// 获取接口TBaseInterface的实现类
        /// </summary>
        /// <typeparam name="TBaseInterface"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<TBaseInterface> GetImplementedObjectsByInterfaces<TBaseInterface>(this Assembly assembly) where TBaseInterface : class
        {
            return GetImplementedObjectsByInterfaces<TBaseInterface>(assembly,typeof(TBaseInterface));
        }

        /// <summary>
        /// 获取接口TBaseInterface的实现类
        /// </summary>
        /// <typeparam name="TBaseInterface"></typeparam>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerable<TBaseInterface> GetImplementedObjectsByInterfaces<TBaseInterface>(Assembly assembly, Type type) where TBaseInterface:class
        {
            return assembly.GetExportedTypes()
                    .Where(r => !r.IsAbstract && type.IsAssignableFrom(r))
                    .Select(r => (TBaseInterface) Activator.CreateInstance(r));
        }

        /// <summary>
        /// 复制对象到二进制形式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T BinaryClone<T>(this T target)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms,target);
                ms.Position = 0;
                return (T) formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 复制source的Property到target中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T CopyPropertiesTo<T>(this T source,  T target)
        {
            return source.CopyPropertiesTo(p => true, target);
        }

        /// <summary>
        /// 复制source的Property到target中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predict">用来过滤target的property</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T CopyPropertiesTo<T>(this T source, Predicate<PropertyInfo> predict, T target)
        {
            PropertyInfo[] properties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            Dictionary<string, PropertyInfo> sourcePropertiesDict = properties.ToDictionary(p => p.Name);

            PropertyInfo[] targetProperties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty).Where(p=>predict(p)).ToArray();

            for (int i = 0; i < targetProperties.Length; i++)
            {
                var targetProperty = targetProperties[i];
                PropertyInfo sourceProperty;

                if (sourcePropertiesDict.TryGetValue(targetProperty.Name, out sourceProperty))
                {
                    if (sourceProperty.PropertyType != targetProperty.PropertyType)
                    {
                        continue;
                    }

                    if (!sourceProperty.PropertyType.IsSerializable)
                    {
                        continue;
                    }

                    targetProperty.SetValue(target,sourceProperty.GetValue(source,null),null);
                }
            }

            return target;
        }

        /// <summary>
        /// get assemblies from string
        /// </summary>
        /// <param name="assemblyDef"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssembliesFromString(string assemblyDef)
        {
            return GetAssembliesFromStrings(assemblyDef.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        ///  get assemblies from string
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssembliesFromStrings(string[] assemblies)
        {
            List<Assembly> result = new List<Assembly>(assemblies.Length);

            foreach (var a in assemblies)
            {
                result.Add(Assembly.Load(a));
            }

            return result;
        } 

    }
}
