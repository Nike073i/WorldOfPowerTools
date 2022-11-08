using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.ObjectValues;

namespace WorldOfPowerTools.DAL.Context.Configurations
{
    public class UserConfiguration : EntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.OwnsOne(typeof(Address), nameof(Address));
            builder.OwnsOne(typeof(ContactData), nameof(ContactData));
            builder.OwnsOne(typeof(PersonData), nameof(PersonData));
        }
    }
}