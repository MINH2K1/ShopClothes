using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopClothes.Domain.Entity;
using ShopClothes.Infastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Infastructure.DbContext.Configuration
{
  
        public class FunctionConfiguration : DbEntityConfiguration<Function>
        {
            public override void Configure(EntityTypeBuilder<Function> entity)
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).IsRequired()
                    .HasMaxLength(128).IsUnicode(false);
                // etc.
            }
        }
    
}
