using System;
using System.Linq;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;

namespace xxAMIDOxx.xxSTACKSxx.Common.UnitTests
{
    public class OperationIdTests
    {
        [Fact]
        public void EnsureUniqueOperationIds()
        {
            var enumType = typeof(OperationId);
            var enums = (OperationId[])Enum.GetValues(enumType);

            var duplicateValues = enums.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
            Assert.True(duplicateValues.Length == 0, $"{enumType.Name} has duplicate values for: " + string.Join(", ", duplicateValues));
        }
    }
}
