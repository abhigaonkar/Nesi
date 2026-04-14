using System;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using NESI.Common.Extensions;
using Xunit;

namespace Nesi.Common.Tests
{
    [Flags]
    internal enum TestFlagsEnum
    {
        [Description("Default")]
        DefaultValue = 0,
        [Description("First")]
        FirstFlag = 1,
        [Description("Second")]
        SecondFlag = 2,
        ThirdFlag = 4
    };

    internal enum TestSimpleEnum
    {
        Undefined = 0,
        FirstItem = 1
    };

    public class EnumTests
    {
        [Fact]
        public void Get_Description_ReturnsCorrectValue()
        {
            TestFlagsEnum.DefaultValue.GetEnumDescription().Should().Be("Default");
            TestFlagsEnum.ThirdFlag.GetEnumDescription().Should().Be("ThirdFlag");
            Action act = () => (TestFlagsEnum.FirstFlag | TestFlagsEnum.SecondFlag).GetEnumDescription();
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Get_SetEnumFlags_ReturnsCorrectValue()
        {
            TestFlagsEnum.DefaultValue.GetSetFlags().Should().BeEquivalentTo(TestFlagsEnum.DefaultValue);
            //Testing that default value is always set
            (TestFlagsEnum.FirstFlag | TestFlagsEnum.SecondFlag)
                .GetSetFlags().Should().BeEquivalentTo(TestFlagsEnum.DefaultValue, TestFlagsEnum.FirstFlag,
                    TestFlagsEnum.SecondFlag);

            //Check with exclusion
            (TestFlagsEnum.FirstFlag | TestFlagsEnum.SecondFlag)
                .GetSetFlags(TestFlagsEnum.DefaultValue).Should().BeEquivalentTo(TestFlagsEnum.FirstFlag,
                    TestFlagsEnum.SecondFlag);

        }

        [Fact]
        public void Get_SetEnumFlags_ThrowsOnNonFlagEnum()
        {
            Action act = () =>
            {
                var result = (TestSimpleEnum.FirstItem).GetSetFlags().ToList();
            };
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Get_SetEnumFlags_ChecksExcludedValues()
        {
            Action act = () => (TestFlagsEnum.FirstFlag | TestFlagsEnum.SecondFlag)
                .GetSetFlags(TestSimpleEnum.FirstItem)  //Wrong
                .Should()
                .BeEquivalentTo(TestFlagsEnum.FirstFlag,TestFlagsEnum.SecondFlag);
            act.Should().Throw<ArgumentException>();
        }
    }
}