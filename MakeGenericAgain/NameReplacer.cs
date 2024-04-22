using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MakeGenericAgain
{
    public static class NameReplacer
    {
        private const string TEMP_TYPE_NAME = "[#X#]";

        public static string ReplaceToGeneric(string str, ICollection<string> typesToIgnore)
        {
            var toSplit = Regex.Replace(str, @"[^\w\.@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5)).Replace(".", " ");
            foreach (var word in toSplit.Split(" "))
            {
                var typeToReinstate = typesToIgnore.FirstOrDefault(word.Contains);
                var wordToReplace = !string.IsNullOrWhiteSpace(typeToReinstate) ? word.Replace(typeToReinstate, TEMP_TYPE_NAME) : word;

                if (wordToReplace.Contains("Of") &&
                   !wordToReplace.StartsWith("Of") &&
                   !wordToReplace.EndsWith("Of") &&
                   !wordToReplace.StartsWith("DateTime"))
                {
                    var genericWordFor = GetGenericWordFor(wordToReplace);
                    if (wordToReplace != genericWordFor)
                    {
                        if (!string.IsNullOrWhiteSpace(typeToReinstate))
                            genericWordFor = genericWordFor.Replace(TEMP_TYPE_NAME, typeToReinstate);

                        str = str.Replace(word, genericWordFor);
                    }
                }
            }

            return str;
        }

        internal static string GetGenericWordFor(string word)
        {
            var parts = word.Split("Of");

            var closing = string.Join("",Enumerable.Repeat(">", parts.Length - 1));
            var opening = string.Join("", parts.Select((p,i) => i < parts.Length - 1 ? $"{TypeNameFor(p)}<" : TypeNameFor(p)));
            var result = opening + closing;

            return result;
        }

        private static string TypeNameFor(string type)
        {
            if (type.Contains("And"))
            {
                return string.Join(",", type.Split("And").Select(TypeNameFor));
            }

            if (type == "Integer")
                return "int";
            if (type == "String")
                return "string";
            if (type == "Boolean" || type == "Bool")
                return "bool";
            if (type == "Double")
                return "double";
            if (type == "Float")
                return "float";
            if (type == "Decimal")
                return "decimal";
            return type;
        }
    }
}