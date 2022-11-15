using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.DAL.Context.Configurations
{
    public class OrderProductConfiguration : EntityConfiguration<OrderProduct>
    {
        public override void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            base.Configure(builder);
        }
    }
}
