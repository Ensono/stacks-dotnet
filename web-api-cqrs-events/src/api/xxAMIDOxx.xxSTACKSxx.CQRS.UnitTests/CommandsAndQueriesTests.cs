using System;
using System.Linq;
using Amido.Stacks.Application.CQRS.Commands;
using Amido.Stacks.Application.CQRS.Queries;
using Amido.Stacks.Core.Operations;
using Amido.Stacks.DependencyInjection;
using AutoFixture;
using AutoFixture.Kernel;
using NSubstitute;
using Shouldly;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Application.CommandHandlers;
using xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers;
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

        public void OperationCodeShouldHaveOneImplementation()
        {
            var definitions = typeof(CreateMenu).Assembly.GetImplementationsOf(typeof(IOperationContext));
            foreach (OperationCode code in Enum.GetValues(typeof(OperationCode)))
            {
                var implementation = definitions.Select(d => d.implementation).SingleOrDefault(o => GetOperationCode(o) == (int)code);
                implementation.ShouldNotBeNull($"The operation '{(int)code}-{code.ToString()}' does not have an implementation");
            }
        }

        [Fact]

        public void QueriesNameShouldMatchOperationName()
        {
            var definitions = typeof(GetMenuById).Assembly.GetImplementationsOf(typeof(IQueryCriteria));
            foreach (var definition in definitions)
            {
                var operationCode = GetOperationCode(definition.implementation);
                var operationName = GetOperationName(operationCode);

                definition.implementation.Name.ShouldBeOneOf(operationName, $"{operationName}QueryCriteria");
            }
        }

        [Fact(DisplayName = "Commands should have a handler")]

        public void CommandsShouldHaveAHandler()
        {
            var commands = typeof(CreateMenu)
                .Assembly
                .GetImplementationsOf(typeof(ICommand))
                .Select(c => c.Item2);

            var handlers = typeof(CreateCategoryCommandHandler)
                .Assembly
                .GetImplementationsOf(typeof(ICommandHandler<,>))
                .Select(d => d).ToList();

            var join = (from c in commands
                    join handler in handlers on c equals handler.Item1.GenericTypeArguments[0] into dj
                    from h in dj.DefaultIfEmpty()
                    select new
                    {
                        CommandType = c,
                        GenericTypeArg = h.interfaceVariation?.GenericTypeArguments[0],
                        Name = h.implementation?.Name
                    })
                .ToList();

            foreach (var j in join)
            {
                j.GenericTypeArg.ShouldBe(j.CommandType);
                j.Name.ShouldBe($"{j.CommandType.Name}CommandHandler");
            }
        }

        [Fact(DisplayName = "Queries should have a handler")]
        public void QueriesShouldHaveAHandler()
        {
            var queries = typeof(GetMenuById)
                .Assembly
                .GetImplementationsOf(typeof(IQueryCriteria))
                .Select(c => c.Item2);

            var handlers = typeof(GetMenuByIdQueryHandler)
                .Assembly
                .GetImplementationsOf(typeof(IQueryHandler<,>))
                .Select(d => d).ToList();

            var join = (from q in queries
                    join handler in handlers on q equals handler.Item1.GenericTypeArguments[0] into dj
                    from h in dj.DefaultIfEmpty()
                    select new
                    {
                        QueryType = q, 
                        GenericTypeArg = h.interfaceVariation?.GenericTypeArguments[0],
                        h.implementation?.Name
                    })
                .ToList();

            foreach (var j in join)
            {
                j.GenericTypeArg.ShouldBe(j.QueryType);
                j.Name.ShouldBe($"{j.QueryType.Name}QueryHandler");
            }
        }

        private int GetOperationCode(Type commandType)
        {
            var fixture = new Fixture();
            fixture.Register<IOperationContext>(() => Substitute.For<IOperationContext>());
            var cmd = new SpecimenContext(fixture).Resolve(commandType);
            return ((IOperationContext)cmd).OperationCode;
        }

        private string GetOperationName(int operationCode)
        {
            return ((OperationCode)operationCode).ToString();
        }
    }
}
