using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Domain.ProjectManagement.EntityConfigurations
{
    public class CrowdsaleEntityConfiguration : EntityConfigurationBase<Crowdsale>
    {
        protected override string TableName => Constants.Tables.Crowdsales;
        protected override string Schema => Constants.Schema;

        public override void Configure(EntityTypeBuilder<Crowdsale> builder)
        {
            base.Configure(builder);
            builder.HasOne(c => c.Project)
                .WithOne(p => p.Crowdsale)
                .HasForeignKey<Crowdsale>(c => c.ProjectId).IsRequired(true);
            var splitStringConverter = new ValueConverter<List<string>, string>(
                v => v.Count > 0 ? string.Join(";", v) : null, 
                v => v.Split(';', StringSplitOptions.None).ToList());
            builder.Property(p => p.SupportedStableCoins)
                .HasConversion(splitStringConverter);
        }
    }
}