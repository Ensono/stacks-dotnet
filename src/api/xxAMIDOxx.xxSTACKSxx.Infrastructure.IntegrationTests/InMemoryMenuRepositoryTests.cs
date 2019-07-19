using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Xunit;
using xxAMIDOxx.xxSTACKSxx.Domain;
using xxAMIDOxx.xxSTACKSxx.Infrastructure.Fakes;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.IntTests
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
        //GetByIdTest will be tested as part of Save+Get OR Get+Delete+Get
        //public void GetByIdTest() { }

        /// <summary>
        /// Ensure the implementation of MenuRepository.Save() submit 
        /// the menu information and is retrieved properly
        /// </summary>
        [Theory, AutoData]
        public async Task SaveAndGetTest(InMemoryMenuRepository repository, Menu menu)
        {
            await repository.SaveAsync(menu);
            var dbItem = await repository.GetByIdAsync(menu.Id);

            Assert.Equal(dbItem.Id, menu.Id);
            Assert.Equal(dbItem.Name, menu.Name);
            Assert.Equal(dbItem.RestaurantId, menu.RestaurantId);
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
            await repository.SaveAsync(menu);
            var dbItem = await repository.GetByIdAsync(menu.Id);
            Assert.NotNull(dbItem);

            await repository.DeleteAsync(menu.Id);
            dbItem = await repository.GetByIdAsync(menu.Id);
            Assert.Null(dbItem);
        }
    }
}
