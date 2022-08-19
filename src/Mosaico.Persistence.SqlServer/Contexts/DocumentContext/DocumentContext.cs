using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Domain.DocumentManagement.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mosaico.Core.EntityFramework.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.DocumentContext
{
    public class DocumentContext : DbContextBase<DocumentContext>, IDocumentDbContext
    {
        public DocumentContext(DbContextOptions<DocumentContext> options, IEnumerable<ISaveChangesCommandInterceptor> saveChangesCommandInterceptor = null) :
            base(options, saveChangesCommandInterceptor)
        {
        }

        public DbSet<DocumentBase> Documents { get; set; }
        public DbSet<ProjectDocument> ProjectDocuments { get; set; }
        public DbSet<CompanyDocument> CompanyDocuments { get; set; }

        public DbSet<DocumentContent> DocumentContents { get; set; }
        public DbSet<ProjectLogo> ProjectLogos { get; set; }
        public DbSet<TokenLogo> TokenLogos { get; set; }
        public DbSet<CompanyLogo> CompanyLogos { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<PageCover> PageCovers { get; set; }
        public DbSet<ArticleCover> ArticleCovers { get; set; }
        public DbSet<ArticlePhoto> ArticlePhotos { get; set; }
        public DbSet<InvestmentPackageLogo> InvestmentPackageLogos { get; set; }
        public DbSet<StakingTermsDocument> StakingTermsDocuments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyProjectManagementConfiguration();
            base.OnModelCreating(modelBuilder);
        }
        
        public string ContextName => "core";
    }
}
