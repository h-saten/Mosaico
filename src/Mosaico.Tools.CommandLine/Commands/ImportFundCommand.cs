using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Fund;
using Mosaico.Tools.CommandLine.Models;
using Newtonsoft.Json;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("import-fund")]
    public class ImportFundCommand : CommandBase
    {
        private readonly IVentureFundDbContext _context;

        public ImportFundCommand(IVentureFundDbContext context)
        {
            _context = context;
        }

        public override async Task Execute()
        {
            var fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "funds.json");

            
            if (!File.Exists(fileLocation))
            {
                throw new Exception($"File {fileLocation} does not exist");
            }
            
            var file = await File.ReadAllTextAsync(fileLocation, Encoding.UTF8);
            var funds = JsonConvert.DeserializeObject<List<VentureFundImportDTO>>(file);

            foreach (var fund in funds)
            {
                var fundInDb = await _context.VentureFunds.FirstOrDefaultAsync(f => f.Name == fund.Name);
                if (fundInDb == null)
                {
                    fundInDb = new VentureFund
                    {
                        Name = fund.Name,
                        LastUpdatedAt = fund.LastUpdatedAt
                    };
                    await _context.VentureFunds.AddAsync(fundInDb);
                }
                
                fundInDb.IsEnabled = fund.IsEnabled;
                
                foreach (var fundToken in fund.Tokens)
                {
                    var ventureFundTokenInDb = fundInDb.Tokens.FirstOrDefault(t => t.Symbol == fundToken.Symbol);
                    if (ventureFundTokenInDb == null)
                    {
                        ventureFundTokenInDb = new VentureFundToken
                        {
                            Symbol = fundToken.Symbol,
                            LatestPrice = fundToken.LatestPrice,
                            VentureFund = fundInDb,
                            VentureFundId = fundInDb.Id
                        };
                        fundInDb.Tokens.Add(ventureFundTokenInDb);
                    }

                    ventureFundTokenInDb.Amount = fundToken.Amount;
                    ventureFundTokenInDb.Logo = fundToken.Logo;
                    ventureFundTokenInDb.Name = fundToken.Name;
                    ventureFundTokenInDb.IsStakingEnabled = fundToken.IsStakingEnabled;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}