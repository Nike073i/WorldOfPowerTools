using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldOfPowerTools.Domain.Entities;

namespace WorldOfPowerTools.DAL.Context.Configurations
{
    public class ProductConfiguration : EntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
        }
    }
}