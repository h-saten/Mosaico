# Creating new Module

## Setup Domain

Domain package contains entity definitions and EntityFramework infrastructure.

- Create new Domain Project (.NET 5 Library) and call it `Mosaico.Domain.{MODULE_NAME}`
- Add reference to `Mosaico.Domain.Base` and `Mosaico.Core.EntityFramework`
- Create Entities folder and put there your entities, that extend `EntityBase` class
- Create Abstractions folder and add DbContext interface that inherits `IDbContext`. Add there `DbSet<{ENTITY}>` property
- Create EntityConfigurations folder and for each entity add class named `{ENTITY}EntityConfiguration` which should extend class `EntityConfigurationBase<{ENTITY}>`. You should override Configure method with extra definitions of the entity. This will help to ensure consistency of the table.
- Create Exceptions folder and put there any potential exceptions that can occur related to domain
- Create Extensions folder and `ModelBuilderExtensions` file there with following content. Add ApplyConfiguration call for each entity.
```c#
public static class ModelBuilderExtensions
{
    public static void Apply{MODULE_NAME}Configuration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new {ENTITY}EntityConfiguration());
        // ...
    }
}
```
- Create `Constants` file with describe database configuration like this:
```c#
public static class Constants
{
    public static class Tables
    {
        public const string {ENTITY} = "{TABLE_NAME}";
    }
    
    public const string Schema = "{SCHEMA}";
}
```
- If there are any dependencies to be registered in DI, create `{MODULE_NAME}Module` class that extends `Autofac.Module` class and overrides Load method
- Find project `Mosaico.Persistence.SqlServer` and reference `Mosaico.Domain.{MODULE_NAME}`.
- Create new folder in Contexts folder with the name of module
- This folder should contain two classes: Implementation of DomainContext and Context Factory.
- To implement domain context, create file `{MODULE_NAME}DbContext` and extend `DbContext` class and implement interface you created before. This file should have following structure:
```c#
public class {MODULE_Name}Context : DbContextBase<{MODULE_Name}Context>, IWalletDbContext
{
    public DbSet<{ENTITY}> {ENTITY} { get; set; }
    // ...

    public {MODULE_Name}Context(DbContextOptions<{MODULE_Name}Context> options, ICurrentUserContext currentUserContext) : base(options, currentUserContext)
    {
    }

    public {MODULE_Name}Context(DbContextOptions<{MODULE_Name}Context> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Apply{MODULE_NAME}Configuration();
        // ...
    }
}
```
- Now create a `{MODULE_NAME}ContextFactory` and extend `DbContextFactoryBase<{MODULE_NAME}Context>` and implement `IDbFactory<{MODULE_NAME}Context>`
- In `DomainModule` add registration `builder.RegisterDbContext<{MODULE_NAME}Context, {MODULE_NAME}ContextFactory>(sqlServerSettings.ConnectionString);`
- Run Migrations

## Create Application layer

Application layer contains business logic

- Create new .NET 5 library called `Mosaico.Application.{MODULE_NAME}`
- Add dependencies to at least Domain project, Events.Base, EventSourcing.Base, CQRS.Base
- Create Commands, Queries and EventHandlers accordingly
- Create Autofac.Module derivative and register it either in API or or Core projects.

## Create API layer

API layer services REST API definition for CRUD commands

- Create new .NET 5 library called `Mosaico.API.v1.{MODULE_NAME}`.
- Add reference to at least API.Base and Application projects
- Create controllers and APIv1Module derived from Autofac.Module and register it in Service module

Then you need to add a module registration of newly created module in ServiceModule of the hosting project (like `Mosaico.Core.Services`)

## Events

Events are better to be stored in a separate library for better interoperability. It avoids unnecessary dependencies for potential consumers of those events. Create a .NET 5 library called `Mosaico.Events.{MODULE_NAME}`

## Tests

Testing is very important aspect of delivering quality software. Create unit tests for all Command and Query Handlers in a separate project. Create a test library called `Mosaico.Application.{MODULE_NAME}.Tests` and make sure nuget versions for NUnit and NUnitTestAdapter are similar across the whole solution. Add reference to at least application and Tests.Base projects.

