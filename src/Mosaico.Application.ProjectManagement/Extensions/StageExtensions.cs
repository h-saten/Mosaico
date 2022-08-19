using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Extensions
{
    public static class StageExtensions
    {
        public static void Update(this Stage stage, StageCreationDTO dto)
        {
            stage.Name = dto.Name;
            stage.EndDate = dto.EndDate;
            stage.Type = dto.Type;
            stage.MinimumPurchase = dto.MinimumPurchase;
            stage.MaximumPurchase = dto.MaximumPurchase;
            stage.StartDate = dto.StartDate;
            stage.TokenPrice = dto.TokenPrice;
            stage.TokensSupply = dto.TokensSupply;
        }
    }
}