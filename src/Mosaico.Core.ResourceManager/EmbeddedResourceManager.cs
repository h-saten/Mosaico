using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Mosaico.Base.Abstractions;
using Mosaico.Core.ResourceManager.Exceptions;

namespace Mosaico.Core.ResourceManager
{
    public class EmbeddedResourceManager : IResourceManager
    {
        public Task<string> GetResourceNameAsync(string category, string name, string culture = Base.Constants.Languages.English, Assembly assembly = null)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = Base.Constants.Languages.English;
            }

            if (assembly == null)
            {
                assembly = typeof(EmbeddedResourceManager).Assembly;
            }
            if (culture == null) culture = Base.Constants.Languages.English;
            var resources = assembly.GetManifestResourceNames();
            var resourceMapName = resources.FirstOrDefault(r => r.EndsWith($"{category}.resources"));
            if (string.IsNullOrWhiteSpace(resourceMapName))
            {
                throw new ResourceCategoryNotFoundException(category);
            }

            var rm = new System.Resources.ResourceManager(resourceMapName.Replace(".resources", ""), assembly);
            var resourceName = rm.GetString(name, CultureInfo.CreateSpecificCulture(culture))?.Replace('/', '.');
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ResourceNotFoundException(resourceName);
            }

            return Task.FromResult(resourceName);
        }

        public async Task<byte[]> GetResourceAsync(string category, string name, string culture = Base.Constants.Languages.English, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = typeof(EmbeddedResourceManager).Assembly;
            }
            
            var fileResourceName = GetResourceFullName(category, name, assembly, culture);
            await using var stream = assembly.GetManifestResourceStream(fileResourceName);
            if (stream == null)
            {
                throw new ResourceNotFoundException(fileResourceName);
            }
            var content = new byte[stream.Length];
            await stream.ReadAsync(content, 0, content.Length);
            return content;
        }

        public async Task<string> GetTextResourceAsync(string category, string name, string culture = Base.Constants.Languages.English, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = typeof(EmbeddedResourceManager).Assembly;
            }
            
            var fileResourceName = GetResourceFullName(category, name, assembly, culture);
            await using var stream = assembly.GetManifestResourceStream(fileResourceName);
            if (stream == null)
            {
                throw new ResourceNotFoundException(fileResourceName);
            }

            using (var streamReader = new StreamReader(stream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        protected virtual string GetResourceFullName(string category, string name, Assembly assembly, string culture = Base.Constants.Languages.English)
        { 
            if (culture == null) culture = Base.Constants.Languages.English;
            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = Base.Constants.Languages.English;
            }
            var resources = assembly.GetManifestResourceNames();
            var resourceMapName = resources.FirstOrDefault(r => r.EndsWith($"{category}.resources"));
            if (string.IsNullOrWhiteSpace(resourceMapName))
            {
                throw new ResourceCategoryNotFoundException(category);
            }

            var rm = new System.Resources.ResourceManager(resourceMapName.Replace(".resources", ""), assembly);
            var resourceName = rm.GetString(name, CultureInfo.CreateSpecificCulture(culture))?.Replace('/', '.');
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                resourceName = rm.GetString(name, CultureInfo.InvariantCulture)?.Replace('/', '.');
                if (string.IsNullOrWhiteSpace(resourceName))
                {
                    throw new ResourceNotFoundException(resourceName);
                }
            }

            var fileResourceName = resources.FirstOrDefault(r => r.EndsWith(resourceName));
            if (string.IsNullOrWhiteSpace(fileResourceName))
            {
                throw new ResourceNotFoundException(resourceName);
            }

            return fileResourceName;
        }
    }
}