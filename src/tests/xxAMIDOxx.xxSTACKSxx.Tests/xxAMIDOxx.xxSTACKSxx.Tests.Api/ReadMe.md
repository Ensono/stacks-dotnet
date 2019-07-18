# Amido Stacks Automated Acceptance Testing
## Folder structure
```
├── Builders
├── Configuration
├── Models
└── Tests
	├── Fixtures
	└── Functional
```

##### Builders  
This folder contains builder classes that are used to create POCO's for objects used in API requests. Ideally there should be a model for every
request and response that is used within the tests.
The aim of these classes is to make it as easy as possible for developers to generate the data required by API's.

All Builder classes should inherit from `IBuilder.cs`
##### Configuration
This contains classes used to manage the configuration for the tests. 

- `ConfigModel.cs` is a POCO (Plain Old CLR Object) representation of the json in `appsettings.json`
- `ConfigAccessor.cs` contains the logic required to obtain the JSON from `appsettings.json` and bind it to the `ConfigModel.cs` object. This allows the configuration to be used as a simple object.
##### Models
These are POCO (Plain Old CLR Object) representations of entities that are used in API requests (E.g. Request body, response body). The builder classes are used to create instances of these models.
##### Tests
This is the parent folder for all test code
###### Fixtures
Fixtures contains xUnit class fixtures. These class fixtures are used to create test context for the tests.

- `ApiFixture.cs` handles the creation of the HttpClient. All fixtures using the API should inherit from this class 
- `ClientFixture.cs` is where the API clients are created. This inherits from `ApiFixture.cs` as it uses the API. All endpoints should have a client and all tests/fixtures using the API should inherit from this class.
- `MenuFixture.cs` This is an example fixture for tests around the Menu api. This fixture contains all the steps used in the story testing the Menu API. Tests using the Menu API should inherit from this class in order to access the step definitions

###### Functional
The functional folder contains all the functional acceptance tests. These tests should test a single API in isolation and should orchestrate the tests, rather than contain the deeper test logic (I.e. this is where the BDD syntax lives)
