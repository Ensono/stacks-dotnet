using System;
using System.Linq;
using Xunit;
using Snyk.Fixes.Common.Operations;

namespace Snyk.Fixes.Common.UnitTests
{
    [Trait("TestType", "UnitTests")]
    public class OperationCodeTests
    {
        [Fact]
        public void EnsureUniqueOperationIds()
        {
            var enumType = typeof(OperationCode);
            var enums = (OperationCode[])Enum.GetValues(enumType);

            var duplicateValues = enums.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            Assert.True(duplicateValues.Length == 0, $"{enumType.Name} has duplicate operations for codes: " + string.Join(", ", duplicateValues));
        }
    }
}
