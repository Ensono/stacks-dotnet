# Amido Stacks Automated Acceptance Testing
## Folder structure
```
├── Builders
├── Configuration
├── Models
└── Tests
	├── E2E
	├── Fixtures
	└── Functional
```

##### Builders  
This folder contains builder classes that are used to create POCO's for objects used in API requests.  
##### Configuration
This contains any classes used to manage configuration for the tests
##### Models
These are POCO representations of entities that are using in API requests. The builder classes are used to create instances of these models.
##### Tests
This is the parent folder for all test code
###### E2E
All tests covering End-To-End functionality should be put here. These tests should cover full user journeys by chaining API requests.
###### Fixtures
Fixtures contains xUnit class fixtures. These class fixtures are used to create test context for the tests that use the specific fixture. 
This is where the scenario set up and tear down are set
###### Functional
The functional folder contains all the functional acceptance tests. These tests should test a single API in isolation and create/read data directly from the database.
E.g.: If you are testing a GET request, test data should be created by inserting data directly into the database and then use GET to retrieve it.