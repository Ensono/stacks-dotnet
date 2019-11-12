using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Xunit;
using Xunit.Abstractions;
using xxAMIDOxx.xxSTACKSxx.Domain;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.IntegrationTests
{
    /// <summary>
    /// The purpose of this integration test is to validate the implementation
    /// of MenuRepository againt the data store at development\integration
    /// It is not intended to test if the configuration is valid for a release
    /// Configuration issues will be surfaced on e2e or acceptance tests
    /// </summary>
    [Trait("TestType", "IntegrationTests")]
    public class InMemoryMenuRepositoryTests
    {
        private readonly ITestOutputHelper output;

        public InMemoryMenuRepositoryTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        //GetByIdTest will be tested as part of Save+Get OR Get+Delete+Get
        //public void GetByIdTest() { }

        /// <summary>
        /// Ensure the implementation of MenuRepository.Save() submit 
        /// the menu information and is retrieved properly
        /// </summary>
        [Theory, AutoData]
        public async Task SaveAndGetTest(InMemoryMenuRepository repository, Menu menu)
        {
            output.WriteLine($"Creating the menu '{menu.Id}' in the repository");
            await repository.SaveAsync(menu);
            output.WriteLine($"Retrieving the menu '{menu.Id}' from the repository");
            var dbItem = await repository.GetByIdAsync(menu.Id);

            Assert.NotNull(dbItem);
            Assert.Equal(dbItem.Id, menu.Id);
            Assert.Equal(dbItem.Name, menu.Name);
            Assert.Equal(dbItem.TenantId, menu.TenantId);
            Assert.Equal(dbItem.Description, menu.Description);
            Assert.Equal(dbItem.Enabled, menu.Enabled);
            Assert.Equal(dbItem.Categories, menu.Categories);
        }

        /// <summary>
        /// Ensure the implementation of MenuRepository.Delete() 
        /// removes an existing menu and is not retrieved when requested
        /// </summary>
        [Theory, AutoData]
        public async Task DeleteTest(InMemoryMenuRepository repository, Menu menu)
        {
            output.WriteLine($"Creating the menu '{menu.Id}' in the repository");
            await repository.SaveAsync(menu);
            output.WriteLine($"Retrieving the menu '{menu.Id}' from the repository");
            var dbItem = await repository.GetByIdAsync(menu.Id);
            Assert.NotNull(dbItem);

            output.WriteLine($"Removing the menu '{menu.Id}' from the repository");
            await repository.DeleteAsync(menu.Id);
            output.WriteLine($"Retrieving the menu '{menu.Id}' from the repository");
            dbItem = await repository.GetByIdAsync(menu.Id);
            Assert.Null(dbItem);
        }

        /// <summary>
        /// This test will run 100 operations concurrently to test concurrency issues
        /// </summary>
        [Theory, AutoData]
        public async Task ParallelRunTest(InMemoryMenuRepository repository)
        {
            Task[] tasks = new Task[100];

            Fixture fixture = new Fixture();
            for (int i = 0; i < tasks.Length; i++)
            {
                if (i % 2 == 0)
                    tasks[i] = Task.Run(async () => await SaveAndGetTest(repository, fixture.Create<Menu>()));
                else
                    tasks[i] = Task.Run(async () => await DeleteTest(repository, fixture.Create<Menu>()));
            }

            await Task.WhenAll(tasks);

            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.False(tasks[i].IsFaulted, tasks[i].Exception?.Message);
            }
        }
    }
}
