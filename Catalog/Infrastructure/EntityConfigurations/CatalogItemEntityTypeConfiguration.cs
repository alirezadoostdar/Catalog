﻿namespace Catalog.Infrastructure.EntityConfigurations;

public sealed class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable(CatalogItem.TableName);

        builder.HasKey(x => x.Slug);
        builder.Property(x => x.Slug)
               .HasMaxLength(150);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Description)
               .IsRequired()
               .HasMaxLength(5000);
         
        builder.Property(x => x.Price)
               .HasColumnType("decimal(15,2)");
 
        builder.OwnsMany(x => x.Medias, builder =>
        {
            builder.ToJson();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Url)
                   .IsRequired()
                   .HasMaxLength(1098);
        });
    }
}
