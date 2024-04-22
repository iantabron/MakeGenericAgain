using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MakeGenericAgain
{
    public static class CommandLineArgs
    {
        public static T Parse<T>(string[] args) where T : new()
        {
            var @params = GetParamsFromArgs(args);
            return Parse<T>(@params);
        }

        public static T Parse<T>(Dictionary<string, string> args) where T : new()
        {
            var result = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var paramName = property.GetCustomAttribute<FromCommandLineAttribute>()?.ParamName.EnsureStartsWith("-").ToLower();
                if (!args.Keys.Contains(paramName))
                    continue;

                var value = args[paramName];
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (IsCollection(property))
                    {
                        var values = value.Replace(" ", string.Empty).Split(',').ToList();
                        property.SetValue(result, values);
                    }
                    else
                    {
                        property.SetValue(result, value);
                    }
                }
            }

            return result;
        }

        private static Dictionary<string, string> GetParamsFromArgs(string[] args)
        {
            var @params = new Dictionary<string, string>();
            for (var i = 0; i < args.Length; i += 2)
            {
                args[i] = args[i].EnsureStartsWith("-").ToLower();
                @params.Add(args[i], args[i + 1]);
            }

            return @params;
        }

        private static string EnsureStartsWith(this string str, string toStartWith)
        {
            if (!str.StartsWith(toStartWith))
                str = $"{toStartWith}{str}";

            return str;
        }

        private static bool IsCollection(PropertyInfo prop)
        {
            return prop.PropertyType != typeof(string) && prop.PropertyType == typeof(List<string>);
        }
    }
}