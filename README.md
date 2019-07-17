# stacks-dotnet
DotNet example application and scaffolding for Amido Stacks

# Definitions:

**Application**: 
Throughout this document, the term application will refer to a set of services that 
together form a component in a bigger solution. I.E: 
 - An e-commerce portal, will have multiple small applications like catalog, checkout,
composed of services
like front-end, APIs, cache, worker. 
*We should avoid the term component because everything is a component of a bigger 
solution and it makes harder to identify the context.*
                    
**Service**: 
Is the term we will use to describe a component that provides a specific set of 
features in the application scope. ie:                
- ***UI** is the front-end service that provides pages for users to navigate and access the application features.*
- ***API** is a service that provides endpoints to interact with the application resources either by UI or by other applications.*
  
  
# Repository Structure
```
├── build
│   ├── azDevOps: stores configuration, build steps and scripts used by azure dev ops only
│   ├── jenkins: stores configuration, build steps and scripts used by jenkins only
│   └── scripts: stores scripts used by build steps that can be resused by multiple 
|                tools without changes. ie: Docker script for Container Image creation
├── deploy
|   ├── k8s: stores yaml files for k8s deployments. i.e: deployments, services, 
|   |   |     configMap and related dependencies for each service in an application
|   |   ├── ui: stores yaml for ui service
|   |   └── api: stores yaml for the api service and configuration files
│   │       ├── base: store raw yaml used by k8s
│   │       ├── kustomization: store kustomize files(for kubectl apply -k)
│   │       │   ├── dev: configuration files for dev environment
│   │       │   └── test: configuration files for test environment
│   │       └── helm-chart: store helm chart files(if helm used)
│   │           └── templates
|   ├── serviceFabric: scripts used to deploy applications on serviceFabric
|   ├── terraform: terraform scripts used to provision dependencies only needed by this application
|   └── scripts: deployment scripts shared by multiple tools. ie: Variable substitution
├── contracts: stores swagger specs, ui mocks and other documents describing the overall solution
└── src
    ├── api: stores the solution for the api 
    ├── services (i.e: queue listerner, scheduled jobs) [TBD]
    ├── tests: stores tests not built in other services solutions(functional tests, performance, etc)  
    └── ui: stores the front end service and components
```
