using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;
using Snyk.Fixes.Domain;
using Snyk.Fixes.Infrastructure.Fakes;

namespace Snyk.Fixes.Infrastructure.IntegrationTests
{
    /// <summary>
    /// The purpose of this integration test is to validate the implementation
    /// of localhostRepository againt the data store at development\integration
    /// It is not intended to test if the configuration is valid for a release
    /// Configuration issues will be surfaced on e2e or acceptance tests
    /// </summary>
    [Trait("TestType", "IntegrationTests")]
    public class InMemorylocalhostRepositoryTests
    {
        private readonly ITestOutputHelper output;

        public InMemorylocalhostRepositoryTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        //GetByIdTest will be tested as part of Save+Get OR Get+Delete+Get
        //public void GetByIdTest() { }

        /// <summary>
        /// Ensure the implementation of localhostRepository.Save() submit 
        /// the localhost information and is retrieved properly
        /// </summary>
        [Theory, AutoData]
        public async Task SaveAndGetTest(InMemorylocalhostRepository repository, localhost localhost)
        {
            output.WriteLine($"Creating the localhost '{localhost.Id}' in the repository");
            await repository.SaveAsync(localhost);
            output.WriteLine($"Retrieving the localhost '{localhost.Id}' from the repository");
            var dbItem = await repository.GetByIdAsync(localhost.Id);

            Assert.NotNull(dbItem);
            Assert.Equal(dbItem.Id, localhost.Id);
            Assert.Equal(dbItem.Name, localhost.Name);
            Assert.Equal(dbItem.TenantId, localhost.TenantId);
            Assert.Equal(dbItem.Description, localhost.Description);
            Assert.Equal(dbItem.Enabled, localhost.Enabled);
            Assert.Equal(dbItem.Categories, localhost.Categories);
        }

        /// <summary>
        /// Ensure the implementation of localhostRepository.Delete() 
        /// removes an existing localhost and is not retrieved when requested
        /// </summary>
        [Theory, AutoData]
        public async Task DeleteTest(InMemorylocalhostRepository repository, localhost localhost)
        {
            output.WriteLine($"Creating the localhost '{localhost.Id}' in the repository");
            await repository.SaveAsync(localhost);
            output.WriteLine($"Retrieving the localhost '{localhost.Id}' from the repository");
            var dbItem = await repository.GetByIdAsync(localhost.Id);
            Assert.NotNull(dbItem);

            output.WriteLine($"Removing the localhost '{localhost.Id}' from the repository");
            await repository.DeleteAsync(localhost.Id);
            output.WriteLine($"Retrieving the localhost '{localhost.Id}' from the repository");
            dbItem = await repository.GetByIdAsync(localhost.Id);
            Assert.Null(dbItem);
        }

        /// <summary>
        /// This test will run 100 operations concurrently to test concurrency issues
        /// </summary>
        [Theory, AutoData]
        public async Task ParallelRunTest(InMemorylocalhostRepository repository)
        {
            Task[] tasks = new Task[100];

            Fixture fixture = new Fixture();
            for (int i = 0; i < tasks.Length; i++)
            {
                if (i % 2 == 0)
                    tasks[i] = Task.Run(async () => await SaveAndGetTest(repository, fixture.Create<localhost>()));
                else
                    tasks[i] = Task.Run(async () => await DeleteTest(repository, fixture.Create<localhost>()));
            }

            await Task.WhenAll(tasks);

            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.False(tasks[i].IsFaulted, tasks[i].Exception?.Message);
            }
        }
    }
}
