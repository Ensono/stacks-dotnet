# Amido Stacks Automated Acceptance Testing

## Folder Structure

```
├── Builders
|   ├── Http
├── Configuration
├── Models
└── Tests
    ├── Fixtures
    ├── Functional
	└── Steps
```

### Builders

This folder contains builder classes that are used to create POCO's for objects used in API requests. Ideally there should be a model for every
request and response that is used within the tests.
The aim of these classes is to make it as easy as possible for developers to generate the data required by API's.

All Builder classes should inherit from `IBuilder.cs`

#### Http

This folder contains a builder and factory for managing HttpClients within the tests.

- `HttpRequestBuilder.cs` manages creating the HttpRequest and also the HttpClient. This is only used within `HttpRequestFactory.cs`.
- `HttpRequestFactory.cs` orchestrates the creation of the HttpRequest for each REST method.

### Configuration

This contains classes used to manage the configuration for the tests. 

- `ConfigModel.cs` is a POCO (Plain Old CLR Object) representation of the json in `appsettings.json`
- `ConfigAccessor.cs` contains the logic required to obtain the JSON from `appsettings.json` and bind it to the `ConfigModel.cs` object. This allows the configuration to be used as a simple object.

The ConfigAccessor will automatically replace any configuration setting values with the values set in the Environment Variables on the machine running the tests. 

E.g. in `appsettings.json` we are using the configuration setting (key-value pair) `"BaseUrl":"http://dev.azure.amidostacks.com/api/menu/"`. If there is an Environment Variable set on the current machine/build agent using `BaseUrl` key, the value in `appsettings.json` will be replaced.

### Models

These are POCO (Plain Old CLR Object) representations of entities that are used in API requests (E.g. Request body, response body). The builder classes are used to create instances of these models.

### Tests

This is the parent folder for all test code

#### Fixtures

Fixtures contains xUnit class fixtures. These class fixtures are used to create test context for the tests. The fixture is where you can put fixture setup (via constructor) and teardown (Via `Dispose()`)

See xUnit documentation for information on different fixtures and how to use them: https://xunit.net/docs/shared-context

- `AuthFixture.cs` contains methods for getting authentication tokens required in the test cases.

#### Functional

The functional folder contains all the functional acceptance tests. These tests should test a single API in isolation and should orchestrate the tests, rather than contain the deeper test logic (I.e. this is where the BDD syntax lives)
