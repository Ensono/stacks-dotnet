using System;
using System.Linq;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Queries;
using Amido.Stacks.Core.Operations;
using Amido.Stacks.DependencyInjection;
using AutoFixture;
using AutoFixture.Kernel;
using Shouldly;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Common.Operations;
using xxAMIDOxx.xxSTACKSxx.CQRS.Commands;
using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;

namespace xxAMIDOxx.xxSTACKSxx.CQRS.UnitTests
{
    /// <summary>
    /// Series of tests for Commands and Queries to ensure consistency and conventions
    /// </summary>
    [Trait("TestType", "UnitTests")]
    public class CommandsAndQueriesTests
    {
        [Fact]

        public void CommandsAndQueriesShouldBeUniquePerOperation()
        {
            var assembly = typeof(CreateMenu).Assembly;
            var definitions = assembly.GetImplementationsOf(typeof(ICommand), typeof(IQueryCriteria));

            var duplicateCodes = definitions.Select(d => new
            {
                OperationCode = GetOperationCode(d.implementation),
                d.implementation.Name
            })
                .GroupBy(i => i.OperationCode)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToArray();

            int duplicates = duplicateCodes.Length;
            duplicates.ShouldBe(0, $"Assembly {assembly.FullName} has duplicate operations for Operation Codes: " + string.Join(", ", duplicateCodes));
        }

        [Fact]

        public void CommandNameShouldMatchOperationName()
        {
            var definitions = typeof(CreateMenu).Assembly.GetImplementationsOf(typeof(ICommand));
            foreach (var definition in definitions)
            {
                var operationCode = GetOperationCode(definition.implementation);
                var operationName = GetOperationName(operationCode);

                definition.implementation.Name.ShouldBeOneOf(operationName, $"{operationName}Command");
            }
        }

        [Fact]

        public void QueriesNameShouldMatchOperationName()
        {
            var definitions = typeof(GetMenuByIdQueryCriteria).Assembly.GetImplementationsOf(typeof(IQueryCriteria));
            foreach (var definition in definitions)
            {
                var operationCode = GetOperationCode(definition.implementation);
                var operationName = GetOperationName(operationCode);

                definition.implementation.Name.ShouldBeOneOf(operationName, $"{operationName}QueryCriteria");
            }
        }

        private int GetOperationCode(Type commandType)
        {
            var fixture = new Fixture();
            var cmd = new SpecimenContext(fixture).Resolve(commandType);
            return ((IOperationContext)cmd).OperationCode;
        }

        private string GetOperationName(int operationCode)
        {
            return ((OperationCode)operationCode).ToString();
        }
    }
}
