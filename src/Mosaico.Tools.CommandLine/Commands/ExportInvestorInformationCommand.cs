using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Tools.CommandLine.Commands
{
    public class CsvUser
    {
        [Name("Name")] 
        public string Name { get; set; }

        [Name("Email")] 
        public string Email { get; set; }
        
        [Name("Phone")] 
        public string Phone { get; set; }
    }

    [Command("export-investor-info")]
    public class ExportInvestorInformationCommand : CommandBase
    {
        private Guid _projectId;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IIdentityContext _identityContext;
        
        public ExportInvestorInformationCommand(IWalletDbContext walletDbContext, IIdentityContext identityContext)
        {
            _walletDbContext = walletDbContext;
            _identityContext = identityContext;
            SetOption("--projectId", "project id", s => _projectId = Guid.Parse(s) );
        }
        public override async Task Execute()
        {
            var userIds = await _walletDbContext.Transactions.Where(t => t.ProjectId == _projectId && t.UserId != null)
                .Select(t => t.UserId).Distinct().ToListAsync();
            var users = await _identityContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            var csvUsers = users.Select(u => new CsvUser
            {
                Name = $"{u.FirstName} {u.LastName}",
                Email = u.Email,
                Phone = u.PhoneNumber
            });
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
                        csv.WriteHeader<CsvUser>();
                        await csv.NextRecordAsync();
                        await csv.WriteRecordsAsync(csvUsers);
                    }
                }
                var bytes = memoryStream.ToArray();
                File.WriteAllBytes("investors.csv", bytes);
            }
        }
    }
}