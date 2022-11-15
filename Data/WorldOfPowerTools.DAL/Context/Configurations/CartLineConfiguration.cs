using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.DAL.Context.Configurations
{
    public class CartLineConfiguration : EntityConfiguration<CartLine>
    {
        public override void Configure(EntityTypeBuilder<CartLine> builder)
        {
            base.Configure(builder);
        }
    }
}
