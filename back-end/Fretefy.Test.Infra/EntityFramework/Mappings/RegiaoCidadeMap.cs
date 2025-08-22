using Microsoft.EntityFrameworkCore;
using Fretefy.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fretefy.Test.Infra.EntityFramework.Mappings
{
    public class RegiaoCidadeMap : IEntityTypeConfiguration<RegiaoCidade>
    {
        public void Configure(EntityTypeBuilder<RegiaoCidade> builder)
        {
            builder.HasKey(rc => new { rc.RegiaoId, rc.CidadeId });
            builder.HasOne(rc => rc.Regiao)
                   .WithMany(r => r.RegiaoCidades)
                   .HasForeignKey(rc => rc.RegiaoId);

            builder.HasOne(rc => rc.Cidade)
                   .WithMany()
                   .HasForeignKey(rc => rc.CidadeId);
        }
    }
}
