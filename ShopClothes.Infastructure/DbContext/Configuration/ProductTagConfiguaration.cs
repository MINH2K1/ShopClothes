﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Infastructure.DbContext.Configuration
{
    public class ProductTagConfiguaration
    {
        public class ProductTagConfiguration : DbEntityConfiguration<ProductTag>
        {
            public override void Configure(EntityTypeBuilder<ProductTag> entity)
            {
                entity.Property(c => c.TagId).HasMaxLength(50).IsRequired()
                .HasMaxLength(50).IsUnicode(false);
                // etc.
            }
        }
    }
}