using System;
using System.Linq;
using System.Threading.Tasks;
using Amido.Stacks.Configuration;
using Amido.Stacks.Data.Documents.Abstractions;
using Amido.Stacks.Data.Documents.CosmosDB;
using Amido.Stacks.Testing.Settings;
using AutoFixture;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using Snyk.Fixes.Domain;
using Snyk.Fixes.Infrastructure.Repositories;

namespace Snyk.Fixes.Infrastructure.IntegrationTests
{

    /// <summary>
    /// The purpose of this integration test is to validate the implementation
    /// of localhostRepository againt the data store at development\integration
    /// It is not intended to test if the configuration is valid for a release
    /// Configuration issues will be surfaced on e2e or acceptance tests
    /// </summary>
    [Trait("TestType", "IntegrationTests")]
    public class localhostRepositoryTests
    {
        public localhostRepositoryTests()
        {
            var settings = Configuration.For<CosmosDbConfiguration>("CosmosDB");
            //Notes:
            // if using an azure instance to run the tests, make sure you set the environment variable before you start visual studio
            // Ex: CMD C:\> setx COSMOSDB_KEY=ABCDEFGASD==
            // On CosmosDB, make sure you create the collection 'localhost' in the same database defined in the config.
            // To overrride the appsettings values, set the environment variable using SectionName__PropertyName. i.e: CosmosDB__DatabaseAccountUri 
            // Note the use of a double _ between the section and the property name
            if (Environment.GetEnvironmentVariable(settings.SecurityKeySecret.Identifier) == null)
            {
                //if locahost and running in visual studio use the default emulator key
                if (settings.DatabaseAccountUri.Contains("localhost", StringComparison.InvariantCultureIgnoreCase) && Environment.GetEnvironmentVariable("VisualStudioEdition") != null)
                    Environment.SetEnvironmentVariable(settings.SecurityKeySecret.Identifier, "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
                else
                    throw new ArgumentNullException($"The environment variable '{settings.SecurityKeySecret.Identifier}' has not been set. Esure all environment variables required are set before wunning integration tests.");
            }
        }

        //GetByIdTest will be tested as part of Save+Get OR Get+Delete+Get
        //public void GetByIdTest() { }

        /// <summary>
        /// Ensure the implementation of localhostRepository.Save() submit 
        /// the localhost information and is retrieved properly
        /// </summary>
        [Theory, localhostRepositoryAutoData]
        public async Task SaveAndGetTest(localhostRepository repository, localhost localhost)
        {
            await repository.SaveAsync(localhost);
            var dbItem = await repository.GetByIdAsync(localhost.Id);

            //Assert the values returned from DB matches the values sent
            Assert.Equal(dbItem.Id, localhost.Id);
            Assert.Equal(dbItem.Name, localhost.Name);
            Assert.Equal(dbItem.TenantId, localhost.TenantId);
            Assert.Equal(dbItem.Description, localhost.Description);
            Assert.Equal(dbItem.Enabled, localhost.Enabled);
            Assert.All(localhost.Categories, c =>
                dbItem.Categories.Any(d =>
                    c.Id == d.Id &&
                    c.Name == d.Name &&
                    c.Description == d.Description &&
                    c.Items == d.Items
                )
            );
        }

        /// <summary>
        /// Ensure the implementation of localhostRepository.Delete() 
        /// removes an existing localhost and is not retrieved when requested
        /// </summary>
        [Theory, localhostRepositoryAutoData]
        public async Task DeleteTest(localhostRepository repository, localhost localhost)
        {
            await repository.SaveAsync(localhost);
            var dbItem = await repository.GetByIdAsync(localhost.Id);
            Assert.NotNull(dbItem);

            await repository.DeleteAsync(localhost.Id);
            dbItem = await repository.GetByIdAsync(localhost.Id);
            Assert.Null(dbItem);
        }
    }

    public class localhostRepositoryAutoData : AutoDataAttribute
    {
        public localhostRepositoryAutoData() : base(Customizations) { }

        public static IFixture Customizations()
        {
            var settings = Configuration.For<CosmosDbConfiguration>("CosmosDB");

            IFixture fixture = new Fixture();

            var loggerFactory = Substitute.For<ILoggerFactory>();
            loggerFactory.CreateLogger(Arg.Any<string>()).Returns(new Logger<CosmosDbDocumentStorage<localhost>>(loggerFactory));
            fixture.Register<ILogger<CosmosDbDocumentStorage<localhost>>>(() => new Logger<CosmosDbDocumentStorage<localhost>>(loggerFactory));
            fixture.Register<ISecretResolver<string>>(() => new SecretResolver());
            fixture.Register<IOptions<CosmosDbConfiguration>>(() => settings.AsOption());
            fixture.Register<IDocumentStorage<localhost>>(() => fixture.Create<CosmosDbDocumentStorage<localhost>>());

            return fixture;
        }
    }
}
