using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Domain.DocumentManagement.Abstractions
{
    public interface IDocumentDbContext : IDbContext
    {
        DbSet<DocumentBase> Documents { get; set; }
        DbSet<ProjectDocument> ProjectDocuments { get; set; }
        DbSet<CompanyDocument> CompanyDocuments { get; set; }

        DbSet<DocumentContent> DocumentContents { get; set; }
        DbSet<ProjectLogo> ProjectLogos { get; set; }
        DbSet<CompanyLogo> CompanyLogos { get; set; }
        DbSet<TokenLogo> TokenLogos { get; set; }
        DbSet<UserPhoto> UserPhotos { get; set; }
        DbSet<PageCover> PageCovers { get; set; }
        DbSet<ArticleCover> ArticleCovers { get; set; }
        DbSet<ArticlePhoto> ArticlePhotos { get; set; }
        DbSet<InvestmentPackageLogo> InvestmentPackageLogos { get; set; }
        DbSet<StakingTermsDocument> StakingTermsDocuments { get; set; }

    }
}
