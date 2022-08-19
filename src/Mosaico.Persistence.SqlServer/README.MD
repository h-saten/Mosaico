# Add new migration

Navigate to the project root folder (src\Mosaico.Persistence.SqlServer) using CLI and run following command:

```
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context ProjectContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context WalletContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context DocumentContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context BusinessContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context FeaturesContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context ApplicationDbContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context IdentityPersistedGrantDbContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context DataProtectionDbContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context TokenizerDbContext
dotnet ef migrations add Initial --project Mosaico.Persistence.SqlServer.csproj --context StatisticsContext
```

# Running migrations

To run migrations using CLI make sure appsettings.json file is updated with proper connection string. Then execute:

```
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context ProjectContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context WalletContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context DocumentContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context BusinessContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context ApplicationDbContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context IdentityPersistedGrantDbContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context DataProtectionDbContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context FeaturesContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context TokenizerDbContext
dotnet ef database update --project Mosaico.Persistence.SqlServer.csproj --context StatisticsContext
```

# Remove migrations

To remove last added migration, run

```
dotnet ef migrations remove --project Mosaico.Persistence.SqlServer.csproj --context ProjectContext
dotnet ef migrations remove --project Mosaico.Persistence.SqlServer.csproj --context WalletContext
dotnet ef migrations remove --project Mosaico.Persistence.SqlServer.csproj --context DocumentContext
dotnet ef migrations remove --project Mosaico.Persistence.SqlServer.csproj --context BusinessContext
dotnet ef migrations remove --project Mosaico.Persistence.SqlServer.csproj --context ApplicationDbContext
dotnet ef migrations remove --project Mosaico.Persistence.SqlServer.csproj --context DataProtectionDbContext
dotnet ef migrations remove --project Mosaico.Persistence.SqlServer.csproj --context StatisticsContext
```

# Adding Seed Data

If you want to add seed data, which is more or less constant in the application (like statuses, types etc), you can use HasData attribute in EntityConfiguration, but remember to hardcode GUID for each entity, because otherwise data will be recreated with each migration.
E.g
` builder.HasData(new ProjectStatus(Constants.ProjectStatuses.New, "New"){Id = new Guid("74246a47-a93d-4713-b8c7-4f51263947ce")});`
