# Introduction
This is a monorepository of the Mosaico v2 solution.
- `.github/workflows` contains CI/CD pipeline definitions
- `docker_compose` contains definitions to run solution dependencies
- `frontend/mosaico-web-ui` - main web app portal (Angular 12)
- `infrastructure` - terraform scripts with terratest for infrastructure-as-a-code and yaml manifests to manage Azure resources and Kubernetes cluster.
- `smart_contracts` - truffle project with solidity with all evm-based smart contracts
- `src` - contains whole backend and microservices (.NET 5)
- `envoy` - contains docker image and configuration of the API gateway
- `scripts` - contains useful powershell scripts and commands to improve development experience

# Contribution rules

- Each feature should be developed in a separate branch. Main branch is restricted from direct pushes. Branch naming convention - {CONTRIBUTOR_INITIALS}/{JIRA_ISSUE_ID}-{SHORT_DESCRIPTION}

`eg dm/TOK-2675-adding-crypto-wallet-backend`

- After feature development is finished, you have to run unit tests locally and perform dev tests.

- Each commit should contain well defined explanation of what has been done. Include jira issue ID at the beginning of the commit title

`eg #TOK-2675 - Added EF Model for Crypto Wallet`

- Create Pull Request and add at least one reviewer for it. Wait until build job will succeed. Confirm all unit tests passed and sonar cloud has no high priority issues reported.
- Fix remarks, if any were left by the reviewer. While waiting for a review, you might start working on the next feature from backlog (in separate branch).
- After feature was merged to main and deployed to Dev environment, perform another dev tests and handover the issue to QA team.

# Dev tools
- [Visual Studio Code](https://code.visualstudio.com/download) with extensions: 
  - [Terraform](https://marketplace.visualstudio.com/items?itemName=4ops.terraform)
  - [TSLint](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-typescript-tslint-plugin)
  - [Docker](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-docker)
  - [Solidity](https://marketplace.visualstudio.com/items?itemName=JuanBlanco.solidity)
  - [YAML](https://marketplace.visualstudio.com/items?itemName=redhat.vscode-yaml)
  - [Angular Snippets](https://marketplace.visualstudio.com/items?itemName=johnpapa.Angular2)
  - [Markdown preview](https://marketplace.visualstudio.com/items?itemName=shd101wyy.markdown-preview-enhanced)
- [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)
- [Rider](https://www.jetbrains.com/rider/download/#section=windows)
- [Another Redis Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager/releases/tag/v1.5.0)
- [Metamask](https://metamask.io/download)
- [Figma](https://www.figma.com/downloads/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [GitHub Desktop](https://desktop.github.com/)
- [Postman](https://www.postman.com/downloads/)

# Running the solution

## Prerequisites

- [Node v16](https://nodejs.org/en/download/)
- `npm i -g @angular/cli`
- `npm i -g truffle`
- Latest version of Docker ([windows/mac](https://www.docker.com/products/docker-desktop)), [ubuntu](https://docs.docker.com/engine/install/ubuntu/)
- [.NET 5](https://dotnet.microsoft.com/en-us/download/dotnet/5.0)
- Install Envoy certificate in Trusted Root Authority folder. Certificate is located in `envoy/https.crt`.

On Windows create following folders:
- d:/mssql
- d:/azurite

Otherwise adjust docker-compose.yml

On Mac or Linux create following folders:
- /azurite

## Dependencies

You should have docker on your machine!

Before you can run API project, you need to ensure you have all dependencies running. Those dependencies on local environment are:
- Ganache CLI
- Redis
- MSSQL
- Azure Blob Storage Emulator
- Envoy
- RabbitMQ

Those dependencies can run within Docker and there is a docker-compose file in folder ~/docker_compose which you can use to bring those services up. From the root repository folder run: 
`docker compose -f ./docker_compose/docker-compose.yml up --build`.

If you work on MACOS, try to run
`docker compose -f ./docker_compose/docker-compose.mac.yml up --build`

**Login into SQL Server localhost:1433, login: sa, password: Mosaico2021! and ensure Mosaico and mosaicoid database were created on the server**

# Database

Run 'Install-Module sqlserver' in terminal
**Make sure there are 3 databases in MSSQL server: Mosaico, mosaicoid and Tokenizer**

## Database migrations
After Database is up, you have to ensure that sql migrations were executed. For that, confirm that in file `/src/Mosaico.Core.Service/appsettings.Development.json` json property Service--RunMigrations is set to true. In such case migrations will run automatically on the startup.
Otherwise, if you need to run migrations manually, reach out to the instructions in `/src/Mosaico.Persistence.SqlServer/README.md`.

## Running the solutions locally using Tye
For the instructions on how to run all components using Tye, go to the `./scripts/README.md`.

## Run API project
Run API project by executing from the repo root directory:
`dotnet run --project ./src/Mosaico.Core.Service/Mosaico.Core.Service.csproj`

On Mac and Linux you need to make sure the the generated TLS certificate is trusted.

Backend will be availale at https://localhost:5001 and swagger definition at https://localhost:5001/core/swagger

## Run Identity project
### Run Identity Frontend
Navigate to `frontend/mosaico-id-ui` and execute `npm i` and then `ng s`. 
### Run Identity Backend
Run API project by executing from the repo root directory:
`dotnet run --project ./src/Mosaico.Identity/Mosaico.Identity.csproj`
Identity will run on https://localhost:49153. Swagger is available at https://localhost:49153/id/swagger.
You can login with default credentials `dev@mosaico.ai` and password `Mosaico1`. You can also register and new user but you have to confirm the user via confirmation link, which is sent via email. On local environment emails are saved on local storage account in container `local-emails`.

## Run frontend

First you need to build smart contract, so navigate to `smart_contracts` folder and run `truffle compile`. Then navigate to the `frontend/mosaico-web-ui` folder and run:
`npm i` - this will install dependencies. Then run `ng s` to start serving the project in development mode.

Frontend will be available at http://localhost:4200/

# Run all services at once

To run all services at once, you can use Tye. Read more in `scripts` folder.

# Continuous Integration

Continuous integration runs on GitHub Actions and each of the solutions (frontend, API, etc) has 2 pipelines configured:

- Pull Request validation build
- Main branch build

Key difference between them is that PR build does not publishes artifacts to be ready for deploy.

Since repository was setup as a mono repository, each github workflow is triggered only when files in a certain folder has been changed. For example, if only API Controller was changed (accordingly to src/** path), only build-backend.yml will be triggered.

Basic CI steps of each pipeline:

- Fetch source codes
- Setup build dependencies (JDK, .NET, NPM, SonarCloud, etc)
- Initiate sonar cloud scanning
- Restore project dependencies (dotnet restore, npm install, etc)
- Build
- Run Tests
- Publish Sonar Cloud Scan results
- Publish Artifacts (push docker image or zip frontend files) (main branch only)

# Continuous Delivery
< TO DO >

# Frontend

# Backend

Backend is inspired by [Modular Monolith](https://www.youtube.com/watch?v=MkdutzVB3pY&t=6224s) extended with additional architectural patterns (CQRS, EDA, Event Sourcing). We separate backend into "Business Modules" and try to make them completely independent and isolated, which will allow us to scale smoother and faster in the future, if we were required to switch to microservices approach. Each library represents a module and therefore should contain Autofac Module which is registered in Service.
Each semi-independent and independent business logic parts should be treated as Modules and underlying project structure should be created for each of them:
- API library, where are [REST API](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design) controllers that emit commands and queries using MediatR. APIs are always versioned so we can guarantee smooth transition to our future consumers. If your main entity has a lot of nested objects for which you also want to provide API within same controller, consider using partial classes.
- Application library, which contains all business logic - command & queries, background jobs, event handlers, extensions and helpers. Also it contains mapping profiles for automapper.
  - Command declares input parameters for the command handler to execute the function
  - Command Handler gets command object through MediatR. It is automatically registered in DI and executed when MediatR receives command of specific type.
  - CommandValidators are classes based on FluentValidation that validate Command object before it goes to CommandHandler. It is executed automatically through MediatR's behavior pipeline in CQRS.Base library. **CommandValidators should be registered in Autofac module!**
  - Query declares input parameters for the data search or filtering
  - QueryHandlers retrieves data from the database or cache based on parameters from Query.
  - QueryValidator is same as CommandValidator. **They also should be registered in autofac module**
  - Query Response is an aggregate object which allows to incapsulate multiple DTOs and properties that are returned from the Query Handler. Useful in case we will extend controllers with extra metadata (like HATEOAS)
  - DTO is common representation of the database entities which is frequently reused within commands and responses
  - Exceptions are classes, custom exceptions, that extend ExceptinoBase and specify error message, extra data (object), ErrorCode which can be translated on frontend, and http status code which will be returned
  - Background jobs are either asynchronous pieces of code or recurring activities that run based on a cron expression. It works with Hangfire library and on local environment you can access Hangfire Dashboard at https://localhost:5001/hangfire
  - Event Handlers are listening to events in Service Bus and process them.
- Tests library, which covers all command handlers, event handlers, background jobs, and query handlers with sufficient level of unit testing
- Domain library contains entities stored in MSSQL. Unfortunately, they are not fully independent from EntityFramework.
  - Entities represent data in sql tables
  - EntityConfigurations use fluent API of EF to configure indexes, relationships etc. They should be added to extension method and applied in db context
  - In Abstractions there are usually interface for DB contexts
- Events is a library with set of records that represent system events. Can be shared across multiple modules so they can also react to events.
- SDK is a library with set of classes and interfaces that help to interact with other modules of Mosaico. Currently we run everything using same lifetime scope and those classes use DbContext to interact with data, sometimes causing redundancy with command handlers. however target is to rewrite those interfaces to use HttpClients to interact with other modules, when Microservices will be established.

Then there is an entry point project, called *.*.Service. This is a Web API project, that should contain only configuration of what modules to activate during startup. It should not contain any business logic. For example at the moment there are two entry projects:

`Mosaico.Core.Service`, which runs all modules related to business logic. In the future it might be required to scale business logic modules, therefore Mosaico.Core.Service should be cloned and adjusted to the needs.

Unlike Identity Server, which appeared to be complex enough due to vast number of possible configuration and security measures to be applied.

Base libraries are abstraction layers that decouples implementation and help with following SOLID principals.

Infrastructure libraries usually contain certain implementations and integrations. Related to other services, providers, SDKs etc.

Detailed instructions on how to create new module you can find in `src/README.MD`.

Because each module should has its own database context and defined schema, you must run several commands to apply all migrations. You can find instructions in `src/Mosaico.Persistence.SqlServer/README.md`.

## Modules

- `Core` - technical module, combines module registrations for all necessary dependencies from infrastructure
- `Business Management` - is responsible for collaborative features for **Issuers**.
- `User Management` - responsible for everything related to identity and user management
- `Project management` - everything related to ICO project execution
- `Wallet` - everything related to the crypto wallet functionality and integration
- `Document management` - semi generic module to work with files and attachments

## Redis

Redis is used as a cache and event storage for operations. At the end it should track all the operations and state changes in the application and provide flexible data search (with RedisSearch plugin?). Later this data could be offloaded from Redis every day to the Data Warehouse for further analytics.

## Tests

Main tests written in NUnit and across the whole solution you can find both Integration and Unit tests. Integration tests are especially relevant when writing blockchain-related logic which should run on the EVM-based environment. Otherwise it might be costly to test it on even Test Network.

## Domain Driven Design

There was no focus on making solution to follow DDD because of the following:

Solution demands high speed of development. Time is currently limited
Higher complexity of maintenance. Current number and experience of team members does not allow to scale DDD
However, with time, we should come back to this topic and evaluate again.

## Gotchas

- For queries that return many records, use PaginatedResult<T> as a response type
- Use static Constants classes to deal with magic strings

## Snippets

### API Controller

```csharp
[ApiVersion("1.0")]
[ApiController]
[Route("api/companies")]
[Route("api/v{version:apiVersion}/companies")]
public partial class CompanyController : ControllerBase {
  private readonly IMediator _mediator;

  public CompanyController(IMediator mediator)
  {
      _mediator = mediator;
  }

  [HttpGet]
  [ProducesResponseType(typeof(SuccessResult<GetCompaniesQueryResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> GetCompanies([FromQuery] GetCompaniesQuery query)
  {
      if (query == null) throw new InvalidParameterException(nameof(query));
      var response = await _mediator.Send(query);
      return new SuccessResult(response);
  }
}
```

### Event Handler

By default we use [CloudEvent](https://cloudevents.io/) notations. There is an implementation of IEventFactory called CloudEventFactory which should be used to wrap standard message payload into cloud event. You don't need to register event handlers on your own. All types with EventInfo attribute will be registered automatically on start.
We use RabbitMQ locally to maintain asynchronous communication between components. You can access RabbitMQ dashboard using `http://localhost:15672/` and credentials `guest:guest`.

- EventInfo specifies the topic/quee and subscription on Azure
- EventTypeFilter specifies with what types of events to proceed with this handler

```csharp
[EventInfo(nameof(CreateWalletOnUserCreated),  "users:api")]
[EventTypeFilter(typeof(UserCreatedEvent))]
public class CreateWalletOnUserCreated : EventHandlerBase {
  //TODO: DI dependencies

  public async Task HandleAsync(CloudEvent @event){
    var userEvent = @event.GetData<UserCreatedEvent>();
    if (userEvent != null)
    {
      //TODO: your logic
    }
  }

}
```


### Background job

Specify BackgroundJob attribute with following parameters:
- Name of the job
- If the job is recurring or not. If yes, you have to specify Cron parameter
- Cron represents a cron expression to run job at given frequency
- Queue name. Hangfire can run jobs on different queue with different settings

**Don't forget to register job in Autofac module**. Like `builder.RegisterHangfireJob<StageDeployerJob>();`

```csharp
[BackgroundJob(Constants.Jobs.StageDeployerJob, IsRecurring = false)]
public class StageDeployerJob : HangfireBackgroundJobBase{
    //TODO: DI dependencies

    public override async Task ExecuteAsync(object parameters = null){
        var now = DateTimeOffset.UtcNow;
        if (parameters is not string strParam || !Guid.TryParse(strParam, out var stageId))
        {
            //TODO: throw or not to throw?
        }

        //TODO: your logic
    }
}
```

To schedule or remove a job, use `IBackgroundJobProvider`.

### Command handler

Command without a response:

```csharp
public class AcceptInvitationCommand : IRequest
{
    public Guid InvitationId { get; set; }
}
```

Command with a response:
```csharp
public class AcceptInvitationCommand : IRequest<Guid>
{
    public Guid InvitationId { get; set; }
}
```


Command validator:
**Don't forget to register validator in autofac module** like `builder.RegisterType<AcceptInvitationCommandValidator>().As<IValidator<AcceptInvitationCommand>>();`
```csharp
public class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
{
    public AcceptInvitationCommandValidator()
    {
        RuleFor(c => c.InvitationId).NotNull().NotEmpty(); //TODO: we also should add error codes or messages to propagate to frontend exact reason otherwise it is too generic
    }
}
```

Command handler:

```csharp
public class ProjectTeamMemberProfileCommandHandler : IRequestHandler<ProjectTeamMemberProfileCommand, string>{
  //TODO: DI dependencies

  public async Task<string> Handle(ProjectTeamMemberProfileCommand request, CancellationToken cancellationToken){
      //TODO: your logic
  }
}
```

Cache: 
We use caching to improve the performance of certain queries on our system. As a distributed cache provider we use Redis which runs as a part of docker compose image. If you want to disable cache for certain debug scenarious, you can update `IsEnabled` of Cache configuration to true/false values in `appsettings.Development.json`.

If you want to cache some of your query data, add a Cache attribute to the Query object like this:
```csharp
[Cache("{{CompanyId}}_{{Network}}", ExpirationInMinutes = 3)]
public class CompanyWalletTokensQuery : IRequest<CompanyWalletTokensQueryResponse>
{
    public Guid CompanyId { get; set; }
    public string Network { get; set; }
}
```
In the example above the result of the CompanyWalletTokensQuery will be cached for 3 minutes with following key: `CompanyWalletTokensQuery_8BA2032A-5C17-4562-9495-B1125010109B_Rinkeby` where `8BA2032A-5C17-4562-9495-B1125010109B` and `Rinkeby` were the values supplied to the query. Pattern, the first paratemer of the Cache attribute, is using Handlebars to relpace content from the Query object in the string.
Cache for language-based queries is a little bit different. 
Afterwards, you want to force and clear the cache after some Command was successfully executed. For example, you cache the company details for many users and only owner can update the title. After he/she updated the title you want other users to immediately see it. To make it happen you should use CacheReset attribute on the command. CacheReset will immediately remove the key after command's successfull execution. In the example below the key `GetProjectQuery_8BA2032A-5C17-4562-9495-B1125010109B` will be immediately removed from Redis. Take into account that pattern, second parameter, works the same way as in Cache. 
```csharp
[CacheReset(nameof(GetProjectQuery), "{{ProjectId}}")]
public class UpdateProjectCommand : IRequest
{
    public Guid ProjectId { get; set; }
    public UpdateProjectDTO Project { get; set; }
}
```
You might have a language property in your query like on the example below. In case you want to clean all languages after some command, use wildcard suffix (works only for languages). 
```csharp
[Cache("{{CompanyId}}_{{Network}}_{{Language}}", ExpirationInMinutes = 3)]
public class CompanyWalletTokensQuery : IRequest<CompanyWalletTokensQueryResponse>
{
    public Guid CompanyId { get; set; }
    public string Network { get; set; }
    public string Language { get; set; }
}
```
It will look like this:
`[CacheReset(nameof(GetProjectQuery), "{{CompanyId}}_{{Network}}_*")]`

Restrictions:
Restrictions are the protection mehanism based on user permissions. User Permissions is the table in Identity service which stores correlation between UserId, Permission and EntityId. It means single user can have multiple permissions on multiple entities. However, entityId is not mandatory and in this case permission is considered as global. To restrict command or query from execution based on the permission, use Restricted attribute on those classes like here:
```csharp
[Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
public class UpdateProjectCommand : IRequest
{
    public Guid ProjectId { get; set; }
    public UpdateProjectDTO Project { get; set; }
}
```
In the example above the system will check if authorized user (by user id) has CanEditDetails on the entity with Id supplied in ProjectId property. There are some other predefined combinations for the restricted attribute like:
`[Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]` - Only ADMINS can run query / command
`[Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]` - can run only if authorized user id is equals with the command/query parameter value UserId (Or user is an admin). Protects from users to be able to update information about other users for example.

### Query handler

`same as command handlers but with different prefix`

# Blockchain

## Metamask & Ganache

When you run the solution, you must also run docker_compose with required prerequisites. One of the prerequisites is Ganache - local Ethereum environment. You can utilize and control this environment fully independently. To make testing complete, at some moment you will have to connect Metamask to this network. 
- open and login into your Metamask in your browser
- create new network with following values:
  - Network name: ethereum_ganache
  - RPC URL: http://localhost:8545
  - Chain ID: 1337
- Then you want to import accounts with private keys that are always the same for any environment as they are pre-configured with 100ETH:
  - 0x7b9b4f50d30f8eba180081741226ce1deee15c8b894af500106bd36e9123a22d
  - 0x14bd56ee025501e56851571dbb4d91c886ec67d5e6bc56021e8fdd87583ac9ef
  - 0x78677671fe428e84866e0e80300df81ba515815a1d8aeee9f55d9c1bca5fcb7d

## Smart Contracts

Folder with smart contracts contains a truffle project setup. 

1. When first time, make sure you run following commands:
  - `npm i`
  - `npm i -g truffle`
  - `npm i -g ganache-cli`
2. When creating new contract, add new js file in migrations folder to deploy the contract
3. Run `ganache-cli` to startup test network in interactive mode.
4. Run `truffle compile` to compile all contracts
5. Run `truffle test` to execute tests

Result of smart contract compilation is a set of JSON files that contain ABI. This ABI is required in both frontend and backend projects.

### Generate ABI for frontend
- Run `truffle compile` in the `smart_contracts` directory. 
- Then navigate to `/scripts/` folder and execute `build-smart-contracts.ps1` which will build and copy ABI files to the appropriate folder in the frontend.
### Generate ABI for backend
- Open `smart_contracts` directory in Visual Studio Code.
- Install `solidity` extension developed by Juan Blanco
- Open the smart contract you want to compile and generate C# classes from
- Click Ctrl+Shift+P and search for Solidity: Compile Contract. Execute
- New C# classes will appear in `Mosaico.Integration.Blockchain.Ethereum` project.
