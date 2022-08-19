using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mosaico.API.Base.Filters;
using Mosaico.BackgroundJobs.Hangfire.Configurations;
using Mosaico.BackgroundJobs.Hangfire.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add(typeof(APIExceptionFilterAttribute))).AddNewtonsoftJson();
var hangfireConfiguration = new HangfireConfig();
builder.Configuration.GetSection(HangfireConfig.SectionName).Bind(hangfireConfiguration);

builder.Services.AddHangfire(hangfireConfiguration, false);

var app = builder.Build();
app.UseHangfire(hangfireConfiguration);

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();