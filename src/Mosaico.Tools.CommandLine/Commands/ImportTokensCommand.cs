using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Tools.CommandLine.Models;
using Newtonsoft.Json;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("import-tokens")]
    public class ImportTokensCommand : CommandBase
    {
        private readonly IWalletDbContext _context;

        public ImportTokensCommand(IWalletDbContext context)
        {
            _context = context;
        }

        public override async Task Execute()
        {
            var fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "tokens.json");

            
            if (!File.Exists(fileLocation))
            {
                throw new Exception($"File {fileLocation} does not exist");
            }
            
            var file = await File.ReadAllTextAsync(fileLocation, Encoding.UTF8);
            var tokenDTOs = JsonConvert.DeserializeObject<List<TokenImportModel>>(file);
            foreach (var tokenDTO in tokenDTOs)
            {
                var token = await _context.Tokens.FirstOrDefaultAsync(pc => pc.Symbol == tokenDTO.Symbol && pc.Network == tokenDTO.Network);
                if (token != null)
                {
                    await UpdateAsync(token, tokenDTO);
                }
                else
                {
                    await CreateAsync(tokenDTO);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task CreateAsync(TokenImportModel dto)
        {
            var tokenType = await _context.TokenTypes.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TokenType.Utility);
            var token = new Token
            {
                Name = dto.Name,
                Symbol = dto.Symbol,
                LogoUrl = dto.LogoUrl,
                Address = dto.Address,
                Network = dto.Network,
                Decimals = dto.Decimals,
                Type = tokenType,
                TypeId = tokenType.Id,
                Status = TokenStatus.Deployed,
                NameNormalized = dto.Name.ToUpperInvariant().Trim(),
                SymbolNormalized = dto.Symbol.ToUpperInvariant().Trim(),
                DisplayAlways = dto.DisplayAlways,
                ERCType = dto.Type
            };
            _context.Tokens.Add(token);
        }

        private async Task UpdateAsync(Token token, TokenImportModel dto)
        {
            token.Symbol = dto.Symbol;
            token.LogoUrl = dto.LogoUrl;
            token.Decimals = dto.Decimals;
            _context.Tokens.Update(token);
            await Task.CompletedTask;
        }
    }
}