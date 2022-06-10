# stacks-dotnet

The full documentation on Amido Stacks can be found [here](https://amido.github.io/stacks/).

Amido Stacks targets different cloud providers.

[Azure](https://amido.github.io/stacks/docs/workloads/azure/backend/netcore/introduction_netcore)

### Templates

This repository contains a template that you can get as a [NuGet package](https://www.nuget.org/packages/Amido.Stacks.Templates/). The template inside the package is called `stacks-webapi` and contains a complete Web API.

### Template usage

#### Template installation

For the latest template version, please consult the Nuget page [Amido.Stacks.Templates](https://www.nuget.org/packages/Amido.Stacks.Templates/). To install the latest version of the templates to your machine via the command line:

```shell
dotnet new --install Amido.Stacks.Templates
```

The output you'll see will list all installed templates (not listed for brevity). In that list you'll see the just installed Amido Stacks template `stacks-webapi`

```shell
Template Name                                    Short Name                       Language    Tags
-----------------------------------------------  -------------------------------  ----------  ------------------------------------------
...
Amido Stacks Web API                             stacks-webapi                    [C#]        Stacks/Infrastructure/WebAPI
...

Examples:
    dotnet new mvc --auth Individual
    dotnet new react --auth Individual
    dotnet new --help
    dotnet new stacks-webapi --help
```

#### Uninstalling a template

To uninstall the template pack you have to execute the following command

```shell
dotnet new --uninstall Amido.Stacks.Templates
```

#### Important parameters

- **-n|--name**
  - Sets the project name
    - Omitting it will result in the project name being the same as the folder where the command has been ran from
- **-do|--domain**
  - Sets the name of the aggregate root object. It is also the name of the collection within CosmosDB instance.
- **-o|--output**
  - Sets the path to where the project is added
  - Omitting the parameter will result in the creation of a new folder
- **-cp|--cloudProvider**
  - Configures which cloud provider to be used
- **-cicd|--cicdProvider**
  - Configures which cicd provider templates to be used

#### Creating a new WebAPI project with the template

Let's say you want to create a brand new WebAPI for your project.

It's entirely up to you where you want to generate the WebAPI. For example your company has the name structure `Foo.Bar` as a prefix to all your namespaces where `Foo` is the company name and `Bar` is the name of the project. If you want the WebAPI to be generated inside a folder called `Foo.Bar` you'll do the following:

```shell
% cd your-repo-folder

% dotnet new stacks-webapi -n Foo.Bar -do Menu
The template "Amido Stacks Web Api" was created successfully.
```

Inspecting the folder will yield the following

```shell
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
% dotnet new stacks-webapi -n Foo.Bar -d Car -o web-api-dir
The template "Amido Stacks Web Api" was created successfully.

% ls -la
total 0
drwxr-xr-x  3 amido  staff   96 23 Aug 15:58 .
drwxr-xr-x  9 amido  staff  288 16 Aug 14:06 ..
drwxr-xr-x  6 amido  staff  192 23 Aug 15:58 web-api-dir
```

Now you can build the solution located in the `web-api/src` folder and run/deploy it.
