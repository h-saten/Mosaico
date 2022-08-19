using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mosaico.Application.ProjectManagement.Services.Models;

namespace Mosaico.Application.ProjectManagement.Services
{
    public interface ICertificateGeneratorService
    {
        Task<FileContentResult> AsPdfAsync(Action<CertificateGeneratorConfiguration> configuration, CancellationToken cancellationToken = new());
        // Task<FileContentResult> AsImageAsync(Action<CertificateGeneratorConfiguration> config,
        //     CancellationToken cancellationToken = new());
        Task<string> AsImageUrlAsync(Action<CertificateGeneratorConfiguration> config,
            CancellationToken cancellationToken = new());
    }
}