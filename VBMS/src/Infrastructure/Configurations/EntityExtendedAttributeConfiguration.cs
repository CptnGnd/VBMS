using VBMS.Application.Serialization.Options;
using VBMS.Application.Serialization.Serializers;
using VBMS.Domain.Contracts;
using VBMS.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace VBMS.Infrastructure.Configurations
{
    public class EntityExtendedAttributeConfiguration : IEntityTypeConfiguration<IEntityExtendedAttribute>
    {
        public void Configure(EntityTypeBuilder<IEntityExtendedAttribute> builder)
        {
            // This Converter will perform the conversion to and from Json to the desired type
            builder
                .Property(e => e.Json)
                .HasJsonConversion(
                    new SystemTextJsonSerializer(
                        new OptionsWrapper<SystemTextJsonOptions>(new SystemTextJsonOptions())));
        }
    }
}