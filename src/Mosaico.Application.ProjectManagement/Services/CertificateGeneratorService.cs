using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Services.Models;
using Mosaico.Base;
using Mosaico.Base.Abstractions;
using Mosaico.DocumentExport.PDF;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Models.CertificateGenerator;
using Mosaico.Storage.Base;

namespace Mosaico.Application.ProjectManagement.Services
{
    public class CertificateGeneratorService : ICertificateGeneratorService
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IResourceManager _resourceManager;
        private readonly IPdfClient _pdfClient;
        private readonly IStorageClient _storageClient;

        public CertificateGeneratorService(
            IProjectDbContext projectDbContext,
            IResourceManager resourceManager, 
            IPdfClient pdfClient, 
            IStorageClient storageClient)
        {
            _projectDbContext = projectDbContext;
            _resourceManager = resourceManager;
            _pdfClient = pdfClient;
            _storageClient = storageClient;
        }

        public async Task<FileContentResult> AsPdfAsync(Action<CertificateGeneratorConfiguration> config, CancellationToken cancellationToken = new())
        {
            var compiledPdf = await PdfBytesAsync(config, cancellationToken);

            return new FileContentResult(compiledPdf, "application/pdf");
        }

        public async Task<string> AsImageUrlAsync(Action<CertificateGeneratorConfiguration> config, CancellationToken cancellationToken = new())
        {
            var compiledPdf = await PdfBytesAsync(config, cancellationToken);
            var certificateImgResult = await _pdfClient.PdfToImageAsync(compiledPdf, cancellationToken);
            
            // save to storage
            var storageContainer = "ci-investor-certificates-img";
            var storageObject = new StorageObject
            {
                Container = storageContainer,
                Content = certificateImgResult.FileContent,
                Size = certificateImgResult.FileContent.Length,
                FileName = $"{Guid.NewGuid().ToString()}.{MimeTypes.GetMimeTypeExtensions(certificateImgResult.ContentType).First()}",
                MimeType = certificateImgResult.ContentType
            };
            var fileId = await _storageClient.CreateAsync(storageObject);
            var url = await _storageClient.GetFileURLAsync(fileId, storageContainer);
            return url;
        }

        private async Task<byte[]> PdfBytesAsync(Action<CertificateGeneratorConfiguration> config,
            CancellationToken cancellationToken = new())
        {
            var parameters = new CertificateGeneratorConfiguration();
            config.Invoke(parameters);
            var project = await _projectDbContext
                    .Projects
                    .Include(t => t.InvestorCertificate.Backgrounds)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == parameters.ProjectId, cancellationToken);
            
            if (project == null || project.TokenId == null)
            {
                throw new ProjectNotFoundException(parameters.ProjectId);
            }
            
            CertificateConfiguration configuration = new DefaultCertificateConfiguration();
            
            var backgroundUrl = DefaultCertificateConfiguration.GetDefaultCertificateBackgroundUrl(parameters.Language);
            
            if (project.InvestorCertificate is not null)
            {
                configuration = project.InvestorCertificate.GetConfiguration();
                var customBackgroundForLanguage = project.InvestorCertificate.GetBackgroundPath(parameters.Language);

                if (customBackgroundForLanguage is not null)
                {
                    backgroundUrl = customBackgroundForLanguage;
                }
            }

            var template = await _resourceManager.GetTextResourceAsync(Core.ResourceManager.Constants.Categories.CertificateTemplates, "Common", parameters.Language);
            var userTokensAmount = Math.Round(parameters.TokensAmount, 2);
            var payload = new CertificateConfigurationMapper()
                .Map(parameters.TokenSymbol,
                    parameters.UserName,
                    userTokensAmount.ToString(CultureInfo.InvariantCulture), 
                    parameters.Date.ToString("dd.MM.yyyy"), 
                    project.LogoUrl, 
                    new CertificateCode(parameters.FinalizedTransactionsAmount, parameters.SequenceNumber), 
                    backgroundUrl, 
                    configuration);
            
            var compiledCertificateTemplate = await _pdfClient.CompileHtmlAsync(template, payload, cancellationToken);
            var compiledPdf = await _pdfClient.HtmlToPdfAsync(compiledCertificateTemplate, true, cancellationToken);
            return compiledPdf;
        }
    }
}