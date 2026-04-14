using System;

namespace NESI.Common.Templates
{
    [Flags]
    public enum TemplateParseValidationMode
    {
        None = 0,
        /// <summary>
        /// Check that all the tokens in the template have replacement values provided
        /// This will ensure that no unreplaced tokens remain in the template after processing
        /// </summary>
        EnsureAllTokensFound = 1,
        /// <summary>
        /// Check that all token values passed in for replacement are actually contained in the
        /// template. This ensures that if template is changed behind the scenes, caller is made aware
        /// </summary>
        EnsureNoUnreplacedTokensRemaning = 2,
        All = EnsureAllTokensFound | EnsureNoUnreplacedTokensRemaning
    }
}