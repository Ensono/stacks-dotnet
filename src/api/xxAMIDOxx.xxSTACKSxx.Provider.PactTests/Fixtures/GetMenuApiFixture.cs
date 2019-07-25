//using System;
//using System.Threading.Tasks;
//using Amido.Stacks.Application.CQRS.Queries;
//using Microsoft.Extensions.DependencyInjection;
//using NSubstitute;
//using NSubstitute.Core;
//using xxAMIDOxx.xxSTACKSxx.API;
//using xxAMIDOxx.xxSTACKSxx.Application.Integration;
//using xxAMIDOxx.xxSTACKSxx.Application.QueryHandlers;
//using xxAMIDOxx.xxSTACKSxx.CQRS.Queries.GetMenuById;
//using xxAMIDOxx.xxSTACKSxx.Infrastructure;

//namespace xxAMIDOxx.xxSTACKSxx.Provider.PactTests.Fixtures
//{
//    public class GetMenuApiFixture : ApiFixture<Startup>
//    {
//        //CreateOrUpdateMenu newMenu;
//        //IMenuRepository repository;
//        //IApplicationEventPublisher applicationEventPublisher;

//        //public GetMenuApiFixture(Menu newMenu)
//        //{
//        //    this.newMenu = newMenu;
//        //}

//        protected override void RegisterDependencies(IServiceCollection collection)
//        {
//            DependencyRegistration.ConfigureStaticServices(collection);

//            // Mocked external dependencies, the setup should 
//            // come later according to the scenarios
//            //repository = Substitute.For<IMenuRepository>();
//            //applicationEventPublisher = Substitute.For<IApplicationEventPublisher>();

//            //collection.AddTransient(IoC => repository);
//            //collection.AddTransient(IoC => applicationEventPublisher);

//            var getMenuIdQueryCriteria = Substitute.For<IQueryHandler<GetMenuByIdQueryCriteria, Menu>>();
//            var repository = Substitute.For<IMenuRepository>();
//            //var getMenuIdQueryCriteria = Substitute.For<GetMenuByIdQueryHandler>();

//            getMenuIdQueryCriteria.ExecuteAsync(Arg.Any<GetMenuByIdQueryCriteria>()).Returns(ReturnDummyResult);

//            collection.AddSingleton(x => repository);
//            collection.AddSingleton<IQueryHandler<GetMenuByIdQueryCriteria, Menu>>(x => new GetMenuByIdQueryHandler(repository));
//        }

//        private Task<Menu> ReturnDummyResult(CallInfo arg)
//        {
//            var menu = new Menu
//            {
//                Id = Guid.Parse("9dbffe96-daa5-4adc-a888-34e41dc205d4"),
//                Name = "menu tuga",
//                Description = "pastel de nata",
//                Enabled = true,
//                Categories = null
//            };

//            throw new Exception("DUMMY EXCEPTION");

//            return Task.FromResult(menu);
//        }
//    }
//}
