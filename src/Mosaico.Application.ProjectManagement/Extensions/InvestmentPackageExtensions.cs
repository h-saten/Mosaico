using System.Collections.Generic;
using System.Linq;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Application.ProjectManagement.Extensions
{
    public static class InvestmentPackageExtensions
    {
        public static InvestmentPackageDTO ToFlatDTO(this InvestmentPackage package, string lang = Mosaico.Base.Constants.Languages.English)
        {
            if (package == null)
            {
                return null;
            }

            return new InvestmentPackageDTO
            {
                Id = package.Id,
                TokenAmount = package.TokenAmount,
                LogoUrl = package.LogoUrl,
                Name = package.GetTranslationInLanguage(lang)?.Name,
                Benefits = package.GetTranslationInLanguage(lang)?.Benefits
                    ?.Split(Domain.ProjectManagement.Constants.InvestmentPackageBenefitSeparator)
                    .ToList()
            };
        }

        public static Dictionary<string, InvestmentPackageDTO> ToDictionaryDTO(this InvestmentPackage package, string lang = null)
        {
            if (package?.Translations == null)
            {
                return null;
            }
            
            var dictionary = new Dictionary<string, InvestmentPackageDTO>();
            foreach (var translation in package.Translations.Where(t => lang == null || t.Language == lang))
            {
                if (!dictionary.ContainsKey(translation.Language))
                {
                    dictionary.Add(translation.Language, package.ToFlatDTO(translation.Language));
                }
            }

            return dictionary;
        }
    }
}