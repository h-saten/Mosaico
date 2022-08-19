using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Network { get; set; }
        public StageDTO ActiveStage { get; set; }
        public string TokenName { get; set; }
        public Guid TokenId { get; set; }
        public string ShortDescription { get; set; }
        public string LogoUrl { get; set; }
        public string CoverLogoUrl { get; set; }
        public decimal HardCap { get; set; }
        public decimal SoftCap { get; set; }
        public decimal HardCapInUserCurrency { get; set; }
        public decimal SoftCapInUserCurrency { get; set; }
        public decimal RaisedCapital { get; set; }
        public decimal RaisedCapitalPercentage { get; set; }
        public decimal RaisedCapitalSoftCapPercentage { get; set; }
        public bool IsSoftCapAchieved { get; set; }
        public int NumberOfBuyers { get; set; }
        public string TokenSymbol { get; set; }
        public Guid PageId { get; set; }
        public string Slug { get; set; }
        public bool IsVisible { get; set; }
        public bool IsExchangeAvailable { get; set; }
        public string MarketplaceStatus { get; set; }
        public decimal RaisedCapitalInUSD { get; set; }
        public int LikeCount { get; set; }
        public bool LikedByUser { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsBlockedForEditing { get; set; }
        public bool IsUserSubscribeProject { get; set; }
        public bool IsPublic { get; set; }
    }
}