using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.ObjectValues;

namespace WorldOfPowerTools.DAL.Context.Configurations
{
    public class OrderConfiguration : EntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            builder.OwnsOne(typeof(Address), nameof(Address));
            builder.OwnsOne(typeof(ContactData), nameof(ContactData));
        }
    }
}