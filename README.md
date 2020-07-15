# Cosmos Data Migrator

Simple console app that can be used to transfer data between Cosmos accounts.

To run the application, you'll first need to add a user secrets file, see the documentation available on this [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows#enable-secret-storage).

You'll also need to copy these secrets to any other machine that you want to run the application on. The user secrets file should include the following properties:

- SourceConnectionString
- DestinationConnectionString

These connection strings are available under the **Keys** section of your Cosmos account.
