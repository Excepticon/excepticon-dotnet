

# excepticon-dotnet

Excepticon SDK for .NET

[![Build Status](https://dev.azure.com/Excepticon/excepticon-dotnet/_apis/build/status/excepticon-dotnet-ASP.NET-CI?branchName=master)](https://dev.azure.com/Excepticon/excepticon-dotnet/_build/latest?definitionId=1&branchName=master)

| Package | Target | Downloads | Nuget Pre-release | Nuget Stable |
| ---     | ---       | ---               | ---          | ---     |
| [Excepticon](https://www.nuget.org/packages/Excepticon) | .NET Standard 2.0 | ![Nuget](https://img.shields.io/nuget/dt/Excepticon) | ![Nuget](https://img.shields.io/nuget/vpre/Excepticon) | ![Nuget](https://img.shields.io/nuget/v/Excepticon) |
| [Excepticon.AspNetCore](https://www.nuget.org/packages/Excepticon.AspNetCore) | .NET Standard 2.0 | ![Nuget](https://img.shields.io/nuget/dt/Excepticon.AspNetCore) | ![Nuget](https://img.shields.io/nuget/v/Excepticon.AspNetCore) | ![Nuget](https://img.shields.io/nuget/v/Excepticon.AspNetCore) |



## About

[Excepticon](https://excepticon.io) is an exception monitoring service for .NET applications and services.  This repository contains the client-side libraries for integrating your .NET applications and services with Excepticon.



## Documentation

Here you'll find a basic introduction to the Excepticon SDK and its API, but for more detailed information, please take a look at the Excepticon [documentation](https://docs.excepticon.io).

You can also check out the examples [here](https://github.com/Excepticon/excepticon-dotnet/tree/master/examples) to see how Excepticon can be integrated into different project types.



## Usage

### Getting Started
1. **Create Excepticon Account** - You'll need create an Excepticon account before you can use the Excepticon SDK.  All of our [plans](https://excepticon.io/plans) include a free 15-day trial (no credit card required), and we offer a Developer plan that is free for solo developers working on non-commercial projects.

2.  **Create Project** - After creating an account, create a project in the Excepticon web app.

3.  **API Key** - An API key will be generated when your project is created and can be retrieved from the Project Settings page in Excepticon.  This key uniquely identifies your project and will be used when configuring the SDK.

### Basic Usage

For many project types, adding Excepticon error monitoring can be accomplished in just a couple of simple steps.

##### 1. Install the Excepticon Nuget Package

Install the latest version of the **Excepticon** package via the Nuget Package Manager, the Package Manager Console, or the dotnet CLI:

```powershell
#Package Manager Console:
Install-Package Excepticon
            
#dotnet CLI:
dotnet add package Excepticon
```

##### 2. Initialize the ExcepticonSdk

Initialize the `ExcepticonSdk` with you project's API Key:

```csharp
static void Main(string[] args)
{
    using (ExcepticonSdk.Init("{Your ApiKey Here}"))
    {
        // Application logic
        
        // Test that exceptions are being sent to Excepticon
        throw new ApplicationException("My test exception.");
    }
}
```

Unhandled exceptions within the using block will automatically be sent to Excepticon.

### ASP.NET Core

##### 1. Install the Excepticon.AspNetCore Nuget Package

Install the latest version of the **Excepticon.AspNetCore** package via the Nuget Package Manager, the Package Manager Console, or the dotnet CLI:

```powershell
#Package Manager Console:
Install-Package Excepticon.AspNetCore
            
#dotnet CLI:
dotnet add package Excepticon.AspNetCore
```

##### 2. Add Excepticon When Creating the IWebHostBuilder

In Program.cs, add a call to `.UseExcepticon()` when building your `IWebHostBuilder`:

```csharp
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseExcepticon("{Your ApiKey Here}");
```

Unhandled exceptions will automatically be sent to Excepticon.

Refer to the [documentation](https://docs.excepticon.io/articles/getting-started/asp-net-core.html) for recommendations for storing your API Key in your application's configuration.

### Other Scenarios

You might require a different configuration depending on your project type.

Please see the [Getting Started documentation](https://docs.excepticon.io/articles/getting-started/index.html) for guides on configuring Excepticon for many different .NET project types.



## Stay In Touch

- E-mail: [jon@excepticon.io](mailto:jon@excepticon.io)
- Twitter: [@excepticonapp](https://twitter.com/excepticonapp)
