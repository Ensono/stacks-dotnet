# Testing Guidance

> **Note:** The **local setup** works only on **Windows** due to an issue with the CosmosDB Docker container on **Linux**. However, if you're using **Azure CosmosDB**, this works on both **Windows** and **Linux**.

---

## Prerequisite

If you're using **Visual Studio** and running the app on your local machine, you don't need to manually set up CosmosDB if you're using the **CosmosDB emulator**. It's automatically configured.

For more details, see the documentation:  
[Setting up CosmosDB locally](https://stacks.ensono.com/docs/workloads/azure/backend/java/setting_up_cosmos_db_locally_java/#set-up-cosmos-db-emulator-locally)

## CosmosDB Tests

- **Connection to CosmosDB**: The tests use **environment variables** to get the connection key for CosmosDB. These variables include:
  - `COSMOSDB_KEY`
  - The primary key from `appsettings.json`
  
### If You Switch from Local Emulator to an Azure Instance

- Update the **URL** in `appsettings.json`.
- Set a new `COSMOSDB_KEY` environment variable before running the tests.

#### Important for Visual Studio

- Visual Studio doesn't automatically pass environment variables from `properties/launchsettings.json`.
- To fix this, manually set the environment variable and **restart Visual Studio**.

---

## How to Set Environment Variables

### On Windows

1. **Permanent (remains after restart)**:  
   Open **Command Prompt** and run this command:

   ```bash
   setx COSMOSDB_KEY=YourNewCosmosDBKeyHere
   ```

2. **Temporary (only for the current session)**:  
   Replace `setx` with `set`:

   ```bash
   set COSMOSDB_KEY=YourNewCosmosDBKeyHere
   ```

    This will work until you close the Command Prompt.

### On Linux

1. **Temporary (for the current terminal session only)**:

   Open a **terminal** and run the following command:

   ```bash
   export COSMOSDB_KEY=YourNewCosmosDBKeyHere
   ```

    This will work only for the terminal session. When you close the terminal, the variable will be lost.

2. **Permanent (across sessions)**:

   You can add the environment variable to your shell configuration file. This could be ~/.bashrc, ~/.zshrc, or another file, depending on your shell.

    Open the file in a text editor (for example, for bash users):

   ```bash
   nano ~/.bashrc
   ```

    Add the following line at the end of the file:

   ```bash
   export COSMOSDB_KEY=YourNewCosmosDBKeyHere
   ```

   Save and close the file, then run:

   ```bash
    source ~/.bashrc
   ```

    This will make the environment variable permanent across terminal sessions.
