using System;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Configuration;
using Amido.Stacks.Data.Documents;
using Amido.Stacks.Data.Documents.CosmosDB;
using Amido.Stacks.Tests.Settings;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Domain;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Repositories;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.IntTests
{

    /// <summary>
    /// The purpose of this integration test is to validate the implementation
    /// of MenuRepository againt the data store at development\integration
    /// It is not intended to test if the configuration is valid for a release
    /// Configuration issues will be surfaced on e2e or acceptance tests
    /// </summary>
    [Trait("TestType", "IntegrationTests")]
    public class MenuRepositoryTests
    {

        //GetByIdTest will be tested as part of Save+Get OR Get+Delete+Get
        //public void GetByIdTest() { }

        /// <summary>
        /// Ensure the implementation of MenuRepository.Save() submit 
        /// the menu information and is retrieved properly
        /// </summary>
        [Theory, MenuRepositoryAutoData]
        public async Task SaveAndGetTest(MenuRepository repository, Menu menu)
        {
            await repository.SaveAsync(menu);
            var dbItem = await repository.GetByIdAsync(menu.Id);

            //Assert the values returned from DB matches the values sent
            Assert.Equal(dbItem.Id, menu.Id);
            Assert.Equal(dbItem.Name, menu.Name);
            Assert.Equal(dbItem.RestaurantId, menu.RestaurantId);
            Assert.Equal(dbItem.Description, menu.Description);
            Assert.Equal(dbItem.Enabled, menu.Enabled);
            Assert.All(menu.Categories, c =>
                dbItem.Categories.Any(d =>
                    c.Id == d.Id &&
                    c.Name == d.Name &&
                    c.Description == d.Description &&
                    c.Items == d.Items
                )
            );
        }

        /// <summary>
        /// Ensure the implementation of MenuRepository.Delete() 
        /// removes an existing menu and is not retrieved when requested
        /// </summary>
        [Theory, MenuRepositoryAutoData]
        public async Task DeleteTest(MenuRepository repository, Menu menu)
        {
            await repository.SaveAsync(menu);
            var dbItem = await repository.GetByIdAsync(menu.Id);
            Assert.NotNull(dbItem);

            await repository.DeleteAsync(menu.Id);
            dbItem = await repository.GetByIdAsync(menu.Id);
            Assert.Null(dbItem);
        }
    }

    public class MenuRepositoryAutoData : AutoDataAttribute
    {
        public MenuRepositoryAutoData() : base(Customizations) { }

        public static IFixture Customizations()
        {
            //Use local emulator on dev when no ENV is defined
            if (Environment.GetEnvironmentVariable("COSMOSDBKEY") == null)
                Environment.SetEnvironmentVariable("COSMOSDBKEY", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            IFixture fixture = new Fixture();

            var loggerFactory = Substitute.For<ILoggerFactory>();
            loggerFactory.CreateLogger(Arg.Any<string>()).Returns(new Logger<CosmosDbDocumentStorage<Menu>>(loggerFactory));
            fixture.Register<ILogger<CosmosDbDocumentStorage<Menu>>>(() => new Logger<CosmosDbDocumentStorage<Menu>>(loggerFactory));
            fixture.Register<ISecretResolver<string>>(() => new SecretResolver());
            fixture.Register<IOptions<CosmosDbConfiguration>>(() => Configuration.For<CosmosDbConfiguration>("CosmosDB").AsOption());
            fixture.Register<IDocumentStorage<Menu>>(() => fixture.Create<CosmosDbDocumentStorage<Menu>>());

            return fixture;
        }
    }
}
