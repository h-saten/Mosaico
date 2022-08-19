using System;
using System.IO;
using System.Reflection;

namespace Mosaico.Integration.Blockchain.Ethereum.Extensions
{
    public static class JsonAbiFile
    {
        public static string Read(string fileName)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var basePath = Path.GetDirectoryName(path);
            using StreamReader r = new StreamReader(basePath + "/" + fileName);
            string json = r.ReadToEnd();
            return json;
        }
    }
}