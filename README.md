# SqlServerWebDemo

[ASP.NET Core](https://github.com/aspnet/AspNetCore) sample application illustrating how to use [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore) together with the [Steeltoe SQL Server Connector](https://github.com/SteeltoeOSS/Connectors) to connect to a bound Microsoft SQL Server service on [Cloud Foundry](https://github.com/cloudfoundry).

## Prerequisites

1. Install [.NET Core SDK](https://dotnet.microsoft.com/download) (v2.2+)
1. Install [Cloud Foundry CLI](https://github.com/cloudfoundry/cli) (v6.43+)

## Create SQL Server Instance on Cloud Foundry

1. `cf target -o MY_ORG -s MY_SPACE`
1. Create the SQL Server backing service:
   * `cf create-service hsdp-rds-sqlserver sqlserver-express mssql-db`
1. Wait for the service instance to finish provisioning:
   * Use `cf service mssql-db` to check operation status.
   * Ensure the status shows as `create succeeded` before attempting to push the app.

## Push App to Cloud Foundry

From the project root:

1. `cf target -o MY_ORG -s MY_SPACE`
1. Publish the app to a local directory, specifying the .NET framework and runtime:  
   * If target stack is `cflinuxfs3`: `dotnet publish -f netcoreapp2.2 -r ubuntu.14.04-x64`
   * If target stack is `windows2016`: `dotnet publish -f netcoreapp2.2 -r win10-x64`
1. Push the app using the appropriate manifest/artifacts for the target stack:  
   * If target stack is `cflinuxfs3`: `cf push -f manifest.yml -p bin/Debug/netcoreapp2.2/ubuntu.14.04-x64/publish`
   * If target stack is `windows2016`: `cf push -f manifest-windows.yml -p bin/Debug/netcoreapp2.2/win10-x64/publish`

> Note: The provided manifests will create an app named `mssql-web-demo` and attempt to bind the app to a SQL Server service instance named `mssql-db`.

## What to Expect

To see the logs as you startup and use the app: `cf logs mssql-web-demo`

You should see something like this during startup (with slight differences depending on the Cloud Foundry stack in use):

```text
2019-03-09T09:52:24.92-0800 [CELL/0] OUT Cell 74e3209c-7886-43ca-856e-a9265da30ddd successfully created container for instance 23ffa498-fa7f-4021-7b1f-1e55
2019-03-09T09:52:27.30-0800 [CELL/0] OUT Starting health monitoring of container
2019-03-09T09:52:30.30-0800 [APP/PROC/WEB/0] OUT Hosting environment: Development
2019-03-09T09:52:30.30-0800 [APP/PROC/WEB/0] OUT Content root path: /home/vcap/app
2019-03-09T09:52:30.30-0800 [APP/PROC/WEB/0] OUT Now listening on: http://0.0.0.0:8080
2019-03-09T09:52:30.30-0800 [APP/PROC/WEB/0] OUT Application started. Press Ctrl+C to shut down.
2019-03-09T09:52:32.08-0800 [CELL/0] OUT Container became healthy
```

Once the application has started, it will be available at <https://mssql-web-demo-[random-route].[your-cf-apps-domain]/>.

You can use the app to update a simple list of authors and their books.  The data will be persisted to the bound SQL Server instance.

Click the *Cloud Foundry Config* menu option at the top of the app to see the `VCAP_APPLICATION` and `VCAP_SERVICES` configuration data.

## Local Development

If you want to play around with the code locally, create a file called `appsettings.Development.json` in the root of the source directory and add the following settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "SqlServer": {
    "credentials": {
      "ConnectionString": "Data Source=your-sqlserver-hostname,1433;User Id=your-sqlserver-userid;Password=your-sqlserver-password;Initial Catalog=your-db-name;"
    }
  }
}
```

You will need to update the `ConnectionString` to point to a Microsoft SQL Server instance accessible from your local machine, e.g. running in Docker or made available via ssh port forwarding, etc.