using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Mosaico.Tests.Base
{
    public abstract class TestBase
    {
        protected virtual string SettingsFileName { get; } = "test_settings.json";
        protected virtual bool LoadSettings { get; } = true;
        private IConfigurationRoot Configuration { get; set; }
        
        [OneTimeSetUp]
        public virtual void InitializeTestsAsync()
        {
            if (LoadSettings)
            {
                LoadUnitTestConfigurationAsync();
            }
        }

        protected virtual void LoadUnitTestConfigurationAsync()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(SettingsFileName, false, false)
                .AddEnvironmentVariables();
            Configuration = configBuilder.Build();
        }

        protected T GetSettings<T>(string sectionName) where T : new()
        {
            var settings = new T();
            Configuration.GetSection(sectionName).Bind(settings);
            return settings;
        }
    }
}