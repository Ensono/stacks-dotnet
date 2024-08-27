using System;
using System.Linq;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Common.Exceptions;

namespace xxENSONOxx.xxSTACKSxx.Common.UnitTests;

[Trait("TestType", "UnitTests")]
public class ExceptionCodeTests
{
    [Fact]
    public void EnsureUniqueExceptionIds()
    {
        var enumType = typeof(ExceptionCode);
        var enums = (ExceptionCode[])Enum.GetValues(enumType);

        var duplicateValues = enums.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
        Assert.True(duplicateValues.Length == 0, $"{enumType.Name} has duplicate values for: " + string.Join(", ", duplicateValues));
    }
}
