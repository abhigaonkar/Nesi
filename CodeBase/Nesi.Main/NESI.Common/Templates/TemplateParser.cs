using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NESI.Common.Exceptions;

namespace NESI.Common.Templates
{
    public static class TemplateParser
    {
        /// <summary>
        /// Parse the given template
        /// </summary>
        /// <param name="template">Template to parse, non null</param>
        /// <param name="tokenReplacementDictionary">Replacement tokens, non null</param>
        /// <param name="format">Replacement format <see cref="TemplateParameterFormat"/></param>
        /// <param name="validationMode"></param>
        /// <returns></returns>
        public static string Parse(
           string template, 
           IDictionary<string, object> tokenReplacementDictionary, 
           TemplateParameterFormat format = TemplateParameterFormat.DoubleBrace,
           TemplateParseValidationMode validationMode = TemplateParseValidationMode.All)
        {
            if (tokenReplacementDictionary == null) throw new ArgumentNullException(nameof(tokenReplacementDictionary));
            if (string.IsNullOrEmpty(template)) throw new ArgumentNullException(nameof(template));

            var rex = new Regex(GetReplacementExpression(format), RegexOptions.None);

            var foundKeys = new HashSet<string>();
            var notFoundKeys = new HashSet<string>();

            var result = rex.Replace(template, match =>
            {
                var tokenKey = match.Groups[1].Value;

                if (tokenReplacementDictionary.ContainsKey(tokenKey))
                {
                    foundKeys.Add(tokenKey);
                    return tokenReplacementDictionary[tokenKey]?.ToString();
                }

                notFoundKeys.Add(tokenKey);
                return match.Value;

            });

            if (validationMode.HasFlag(TemplateParseValidationMode.EnsureAllTokensFound) && notFoundKeys.Any())
            {
                throw new NesiValidationException($"Tokens in template without replacement values: {string.Join(",",notFoundKeys)}");
            }

            if (validationMode.HasFlag(TemplateParseValidationMode.EnsureNoUnreplacedTokensRemaning) && 
                foundKeys.Count < tokenReplacementDictionary.Keys.Count)
            {
                var unmatchedInInput = tokenReplacementDictionary.Keys.Where(k => !foundKeys.Contains(k));
                throw new NesiValidationException($"Tokens not found in template: {string.Join(",", unmatchedInInput)}");
            }

            return result;
        }

        /// <summary>
        /// Gets matching regex for a known pattern
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string GetReplacementExpression(TemplateParameterFormat format)
        {
            switch (format)
            {
                case TemplateParameterFormat.DoubleBrace:
                    return "{{([^}]+)}}";
                case TemplateParameterFormat.SingleSquareBrace:
                    return "\\[([\\s\\S]*?)\\]";
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}
