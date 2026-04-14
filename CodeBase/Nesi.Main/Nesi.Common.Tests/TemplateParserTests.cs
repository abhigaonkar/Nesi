using FluentAssertions;
using NESI.Common.Exceptions;
using NESI.Common.Templates;
using System;
using System.Collections.Generic;
using Xunit;
using static NESI.Common.Templates.TemplateParser;

namespace Nesi.Common.Tests
{
    public class TemplateParserTests
    {
        private readonly IDictionary<string, object> _tokenMap = new Dictionary<string, object>
            {
                {"TOKEN1", "REPLACED_TOKEN_1"},
                {"TOKEN2", "REPLACED_TOKEN_2"},

            };

        [Fact]
        public void Parse_NullString_ShouldThrow()
        {
            Action act = () => Parse(null, _tokenMap);
            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("template");
        }

        [Fact]
        public void Parse_EmptyString_ShouldThrow()
        {
            Action act = () => Parse(null, _tokenMap);
            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("template");
        }

        [Fact]
        public void Parse_WithNoMatches_ShouldReturnOriginal()
        {
            Parse("Some value", _tokenMap, validationMode: TemplateParseValidationMode.None)
                .Should().Be("Some value");
        }

        [Fact]
        public void Parse_WithNoTokens_ShouldReturnOriginal()
        {
            Parse("Some value {{UNUSED}}", new Dictionary<string, object>(),
                validationMode: TemplateParseValidationMode.None).Should().Be("Some value {{UNUSED}}");
        }

        [Theory]
        [InlineData(TemplateParameterFormat.DoubleBrace, "{{TOKEN1}}", "REPLACED_TOKEN_1")]
        [InlineData(TemplateParameterFormat.DoubleBrace, "{{TOKEN1}}{{TOKEN1}}",
            "REPLACED_TOKEN_1REPLACED_TOKEN_1")]
        [InlineData(TemplateParameterFormat.DoubleBrace, "{{TOKEN2}}{{TOKEN1}}",
            "REPLACED_TOKEN_2REPLACED_TOKEN_1")]
        [InlineData(TemplateParameterFormat.DoubleBrace, "A {{TOKEN1}} B {{TOKEN2}} C",
            "A REPLACED_TOKEN_1 B REPLACED_TOKEN_2 C")]
        [InlineData(TemplateParameterFormat.DoubleBrace, "{{TOKEN1}}{{TOKEN3}}", "REPLACED_TOKEN_1{{TOKEN3}}")]

        [InlineData(TemplateParameterFormat.SingleSquareBrace, "[TOKEN1]", "REPLACED_TOKEN_1")]
        [InlineData(TemplateParameterFormat.SingleSquareBrace, "[TOKEN1][TOKEN1]",
            "REPLACED_TOKEN_1REPLACED_TOKEN_1")]
        [InlineData(TemplateParameterFormat.SingleSquareBrace, "[TOKEN2][TOKEN1]",
            "REPLACED_TOKEN_2REPLACED_TOKEN_1")]
        [InlineData(TemplateParameterFormat.SingleSquareBrace, "A [TOKEN1] B [TOKEN2] C",
            "A REPLACED_TOKEN_1 B REPLACED_TOKEN_2 C")]
        [InlineData(TemplateParameterFormat.SingleSquareBrace, "[TOKEN1][TOKEN3]", "REPLACED_TOKEN_1[TOKEN3]")]

        public void Parse_WithTokens_ShouldReturnCorrectOutput(TemplateParameterFormat format, string template,
            string expectedOutput)
        {
            Parse(template, _tokenMap, format, TemplateParseValidationMode.None).Should().Be(expectedOutput);
        }

        [Fact]
        public void Parse_WithMissingTokens_ShouldThrow()
        {
            Action act = () => Parse("{{TOKEN1}}{{TOKEN2}}{{TOKEN3}}", _tokenMap,
                TemplateParameterFormat.DoubleBrace,
                TemplateParseValidationMode.EnsureAllTokensFound);
            act.Should().Throw<NesiValidationException>();
        }

        [Fact]
        public void Parse_WithUnReplacedTokens_ShouldThrow()
        {
            Action act = () => Parse("{{TOKEN1}}", _tokenMap, TemplateParameterFormat.DoubleBrace,
                TemplateParseValidationMode.EnsureNoUnreplacedTokensRemaning);
            act.Should().Throw<NesiValidationException>();
        }
        [Fact]
        public void Parse_WithUnReplacedTokens_ShouldThrow1()
        {
            Action act = () => Parse("{{TOKEN1}}", _tokenMap, TemplateParameterFormat.SingleSquareBrace,
                TemplateParseValidationMode.All);
            act.Should().Throw<NesiValidationException>();
        }
    }
}
