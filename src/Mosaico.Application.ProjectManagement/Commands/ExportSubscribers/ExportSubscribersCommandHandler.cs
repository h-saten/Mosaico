using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.ExportSubscribers
{
    public class SubscriberCsv
    {
        [Name("Email")]
        public string Email { get; set; }
        
        [Name("SubscribedAt")]
        public string SubscribedAt { get; set; }
    }
    
    public class ExportSubscribersCommandHandler : IRequestHandler<ExportSubscribersCommand, ExportSubscribersCommandResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IDateTimeProvider _timeProvider;

        public ExportSubscribersCommandHandler(IProjectDbContext projectDbContext, IDateTimeProvider timeProvider)
        {
            _projectDbContext = projectDbContext;
            _timeProvider = timeProvider;
        }

        public async Task<ExportSubscribersCommandResponse> Handle(ExportSubscribersCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(t => t.Id == request.ProjectId, cancellationToken);
            if (project == null) throw new ProjectNotFoundException(request.ProjectId);
            var subscribers = await _projectDbContext.ProjectNewsletterSubscriptions
                .Where(s => s.ProjectId == request.ProjectId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
            var csvItems = subscribers.Select(s => new SubscriberCsv
            {
                Email = s.Email.ToLowerInvariant().Trim(),
                SubscribedAt = s.SubscribedAt.HasValue ? s.SubscribedAt.Value.ToString("s") : string.Empty
            }).ToList();
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true
            };
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csv = new CsvWriter(streamWriter, csvConfiguration))
                    {
                        csv.WriteHeader<SubscriberCsv>();
                        await csv.NextRecordAsync();
                        await csv.WriteRecordsAsync(csvItems, cancellationToken);
                    }
                }
                var file = memoryStream.ToArray();
                return new ExportSubscribersCommandResponse
                {
                    Count = csvItems.Count(),
                    File = file,
                    Filename = $"{project.Title}_{_timeProvider.Now():yyyy-MM-dd}.csv",
                    ContentType = "text/csv"
                };
            }
        }
    }
}