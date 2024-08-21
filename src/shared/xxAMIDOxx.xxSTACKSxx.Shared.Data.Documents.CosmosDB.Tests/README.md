## Testing Guidance
Prerequisite:
When running on visual studio and configured to run against localhost, we use the CosmosDB emulator without need to set it in advance.
Documentation is available in https://stacks.amido.com/docs/workloads/azure/backend/java/setting_up_cosmos_db_locally_java/#set-up-cosmos-db-emulator-locally

CosmosDB tests retrieve the key to connect to CosmosDB from Environment Variables(COSMOSDB_KEY, and primary key which are configured in the appsettings.json).

If you change from the Local CosmosDB emulator to an azure instance, or you run the tests outside the pipeline, you have to set the new url in the appsetting.json and also set the environment variable COSMOSDB_KEY before running the tests.

The visual studio test console does not pass the environment variables declared withing the `properties/launchsettings.json` file, for that reason, you have to set the environment variable and restart vistual studio to reload the variables from the environment.

To set the environment variable on windows permanently, you can run the following command in the command prompt:

`setx COSMOSDB_KEY=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==`

if you are just running the test and want to set the environment variable for the session, replace `setx` by `set` and it will release the values when session finishes(prompt is closed).
 