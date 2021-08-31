# stacks-dotnet

The full documentation on Amido Stacks can be found [here](https://amido.github.io/stacks/).

Amido Stacks targets different cloud providers.

[Azure](https://amido.github.io/stacks/docs/workloads/azure/backend/netcore/introduction_netcore)

### Templates

This repository contains a template that you can get as a [NuGet package](https://www.nuget.org/packages/Amido.Stacks.Templates/). The template inside the package is called `stacks-app-web-api` and contains a full Web API + build infrastructure.


### Template usage

#### Template installation

To install the template locally you'll need to download the [Amido.Stacks.Templates](https://www.nuget.org/packages/Amido.Stacks.Templates/) NuGet package.

You can install it locally to your machine via the command line (the version provided is just an example, please consult the NuGet page for an up-to date version)

```shell
dotnet new --install Amido.Stacks.Templates::0.0.191
```

The output you'll see will list all installed templates. In that list you'll see the just installed Amido Stacks template `stacks-app-web-api`

```shell
Template Name                                    Short Name                       Language    Tags
-----------------------------------------------  -------------------------------  ----------  ------------------------------------------
Console Application                              console                          [C#],F#,VB  Common/Console
Class library                                    classlib                         [C#],F#,VB  Common/Library
WPF Application                                  wpf                              [C#]        Common/WPF
WPF Class library                                wpflib                           [C#]        Common/WPF
WPF Custom Control Library                       wpfcustomcontrollib              [C#]        Common/WPF
WPF User Control Library                         wpfusercontrollib                [C#]        Common/WPF
Windows Forms (WinForms) Application             winforms                         [C#]        Common/WinForms
Windows Forms (WinForms) Class library           winformslib                      [C#]        Common/WinForms
Worker Service                                   worker                           [C#],F#     Common/Worker/Web
Amido Stacks Web Api - Full solution             stacks-app-web-api               [C#]        Stacks/WebApi/api
MSTest Test Project                              mstest                           [C#],F#,VB  Test/MSTest
NUnit 3 Test Project                             nunit                            [C#],F#,VB  Test/NUnit
NUnit 3 Test Item                                nunit-test                       [C#],F#,VB  Test/NUnit
xUnit Test Project                               xunit                            [C#],F#,VB  Test/xUnit
Razor Component                                  razorcomponent                   [C#]        Web/ASP.NET
Razor Page                                       page                             [C#]        Web/ASP.NET
MVC ViewImports                                  viewimports                      [C#]        Web/ASP.NET
MVC ViewStart                                    viewstart                        [C#]        Web/ASP.NET
Blazor Server App                                blazorserver                     [C#]        Web/Blazor
Blazor WebAssembly App                           blazorwasm                       [C#]        Web/Blazor/WebAssembly
ASP.NET Core Empty                               web                              [C#],F#     Web/Empty
ASP.NET Core Web App (Model-View-Controller)     mvc                              [C#],F#     Web/MVC
ASP.NET Core Web App                             webapp                           [C#]        Web/MVC/Razor Pages
ASP.NET Core with Angular                        angular                          [C#]        Web/MVC/SPA
ASP.NET Core with React.js                       react                            [C#]        Web/MVC/SPA
ASP.NET Core with React.js and Redux             reactredux                       [C#]        Web/MVC/SPA
Razor Class Library                              razorclasslib                    [C#]        Web/Razor/Library
ASP.NET Core Web API                             webapi                           [C#],F#     Web/WebAPI
ASP.NET Core gRPC Service                        grpc                             [C#]        Web/gRPC
dotnet gitignore file                            gitignore                                    Config
global.json file                                 globaljson                                   Config
NuGet Config                                     nugetconfig                                  Config
Dotnet local tool manifest file                  tool-manifest                                Config
Web Config                                       webconfig                                    Config
Solution File                                    sln                                          Solution
Protocol Buffer File                             proto                                        Web/gRPC

Examples:
    dotnet new mvc --auth Individual
    dotnet new react --auth Individual
    dotnet new --help
    dotnet new stacks-function-asb-listener --help
```

#### Uninstalling a template

To uninstall the template pack you have to execute the following command

```shell
dotnet new -u Amido.Stacks.Templates
```

#### Creating a new WebAPI project with the template

Let's say you want to create a brand new WebAPI for your project.

It's entirely up to you where you want to generate the WebAPI. For example your company has the name structure `Foo.Bar` as a prefix to all your namespaces where `Foo` is the company name and `Bar` is the name of the project. If you want the WebAPI to be generated inside a folder called `Foo.Bar` you'll do the following:

```shell
% cd your-repo-folder

% dotnet new stacks-app-web-api -n Foo.Bar
The template "Amido Stacks Web Api - Full solution" was created successfully.

% ls -la
total 0
drwxr-xr-x  3 amido  staff   96 23 Aug 15:51 .
drwxr-xr-x  9 amido  staff  288 16 Aug 14:06 ..
drwxr-xr-x  6 amido  staff  192 23 Aug 15:51 Foo.Bar

% ls -la Foo.Bar
total 16
drwxr-xr-x  6 amido  staff   192 27 Aug 15:51 .
drwxr-xr-x  3 amido  staff    96 27 Aug 15:51 ..
-rw-r--r--  1 amido  staff  1062 27 Aug 14:59 LICENSE
-rw-r--r--  1 amido  staff   258 27 Aug 14:59 README.md
drwxr-xr-x  3 amido  staff    96 27 Aug 14:59 build
drwxr-xr-x  4 amido  staff   128 27 Aug 14:59 contracts
drwxr-xr-x  5 amido  staff   160 27 Aug 14:59 deploy
drwxr-xr-x  4 amido  staff   128 27 Aug 14:59 src
-rw-r--r--  1 amido  staff   292 27 Aug 14:59 yamllint.confn
```

The `Foo.Bar` namespace prefix will be added to the class names and is reflected not only in folder/file names, but inside the codebase as well.

To generate the template with your own namespace, but in a different folder you'll have to pass the `-o` flag with your desired path/folder name

```shell
% dotnet new stacks-app-web-api -n Foo.Bar -o web-api
The template "Amido Stacks Web Api - Full solution" was created successfully.

% ls -la
total 0
drwxr-xr-x  3 amido  staff   96 23 Aug 15:58 .
drwxr-xr-x  9 amido  staff  288 16 Aug 14:06 ..
drwxr-xr-x  6 amido  staff  192 23 Aug 15:58 web-api
```

Now you can build the solution located in the `web-api/src` folder and run/deploy it.