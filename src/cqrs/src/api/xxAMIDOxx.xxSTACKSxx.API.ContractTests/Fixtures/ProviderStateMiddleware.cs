using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NSubstitute;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;
using xxAMIDOxx.xxSTACKSxx.Domain;

namespace xxAMIDOxx.xxSTACKSxx.API.ContractTests.Fixtures;

public class ProviderStateMiddleware : IMiddleware
{
    private readonly IDictionary<string, Action> providerStates;
    readonly IMenuRepository repository;

    public ProviderStateMiddleware(IMenuRepository repository)
    {
        this.repository = repository;

        //Provider states are from the Given clause in the consumer tests
        providerStates = new Dictionary<string, Action>
        {
            {
                //These are case sensitive. Consumer and Provider should share list of states when states are required
                "An existing menu",
                ExistingMenu
            },
            {
                "A menu does not exist",
                MenuDoesNotExist
            }
        };
    }

    //These functions set up the mocked provider API state by mocking the response the repository gives
    private void ExistingMenu()
    {
        var menu = new Fixture().Create<Menu>();

        repository.GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult(menu));
        repository.SaveAsync(Arg.Any<Menu>()).Returns(true);
    }

    private void MenuDoesNotExist()
    {
        repository.GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult((Menu)null));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.Value == "/provider-states")
        {
            HandleProviderStatesRequest(context);
            await context.Response.WriteAsync(string.Empty);
        }
        else
        {
            await next(context);
        }
    }

    private void HandleProviderStatesRequest(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
            context.Request.Body != null)
        {
            var jsonRequestBody = string.Empty;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                jsonRequestBody = reader.ReadToEnd();
            }

            var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

            //A null or empty provider state key must be handled
            if (providerState != null && !string.IsNullOrEmpty(providerState.State))
            {
                providerStates[providerState.State].Invoke();
            }
        }
    }
}
