# Introduction
Hangfire is an open-source package to run background jobs. It uses SQL Database to store data about executed jobs and allows to rerun failed ones.

# Adding Hangfire to the service

1. Add `services.AddHangfire(_hangfireConfiguration);` to ConfigureServices method in Startup.cs
2. Add `app.UseHangfire(_hangfireConfiguration);` to Configure method in Startup.cs
3. Add `builder.RegisterModule(new HangfireModule(_configuration));` to ServiceModule autofac module

# Configuration
In appsettings you have to configure Hangfire like this:
```jsonc
 "Hangfire": {
    "IsEnabled": true, // is hangfire enabled
    "ConnectionString": "", //MSSQL connection string
    "IsDashboardEnabled": false, //is dashboard enabled and publicly accessible
    "DatabaseSchema": "jobs", //database schema. should never change
    "JobSchedule": null //dictionary with configuration of job's schedule (job name: cron expression)
  }
```

# Creating a background job

1. Create new class and extend based class `HangfireBackgroundJobBase`
2. You can freely use same dependency injection container, as for normal HttpRequests. Dependencies resolved using Autofac
3. Keep in mind that current user context will be missing UserId. Subject to improve in the future by creating a service user.
4. Add `BackgroundJob` attribute and specify Name, Queue and CRON expression for the job. If you don't specify Queue, job will run on default one. If you don't specify Cron expression, job will run daily at midnight.
5. It is better to specify cron job in the configuration so it can be changed in a flexible way. Add this to Json configuration of Hangfire (job name: cron expression)
```json
"JobSchedule": {
  "test-job": "0 0 * * *"
}
```
6. Register your job in a DI module: `builder.RegisterType<TestJob>().As<IBackgroundJob>();`

Example job:
```c#
[BackgroundJob("test-job")]
public class TestJob : HangfireBackgroundJobBase
{
    private readonly IProjectDbContext _dbContext;

    public TestJob(IProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task ExecuteAsync(object parameters = null)
    {
        return Task.CompletedTask;
    }
}
```