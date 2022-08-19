using System;
using System.Text;
using System.Threading.Tasks;
using Jose;
using Mosaico.DocumentExport.Base;
using Mosaico.DocumentExport.CKEditor.Configurations;
using Mosaico.DocumentExport.CKEditor.Exceptions;
using Mosaico.DocumentExport.CKEditor.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Mosaico.DocumentExport.CKEditor
{
    public class CKEditorClient : IDocumentExportClient
    {
        private readonly CKEditorConfiguration _editorConfig;

        public CKEditorClient(CKEditorConfiguration editorConfig)
        {
            _editorConfig = editorConfig;
        }

        public async Task<byte[]> ExportAsync(string html)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var tokenPayload = new {aud = _editorConfig.EnvironmentId, iat = now};
            var accessKeyByteArray = Encoding.UTF8.GetBytes(_editorConfig.AccessKey);
            var token = JWT.Encode(tokenPayload, accessKeyByteArray, JwsAlgorithm.HS256);
            var restClient = new RestClient(new Uri(_editorConfig.EnvironmentUrl));
            //restClient.AddDefaultHeader("Authorization", token);
            var requestApi = new RestRequest("https://pdf-converter.cke-cs.com/v1/convert", DataFormat.Json)
            {
                OnBeforeDeserialization = resp =>
                {
                    resp.ContentType = "application/json";
                },
                Method = Method.POST
            };

            requestApi.AddJsonBody(new
            {
                html = html,
                css = "",
                options = new { }
            });
            var response = await restClient.ExecutePostAsync(requestApi);
            if (!response.IsSuccessful)
            {
                var error = JsonConvert.DeserializeObject<ExportErrorModel>(response.Content);
                throw new CKEditorExportException(error?.Message ?? $"Unhandled CKEditor exception - {response.StatusCode}");
            }
            
            return response.RawBytes;
        }
    }
}