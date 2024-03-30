using ShopClothes.Infastructure.DbContext;
using ShopClothes.Infastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Infastructure
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ShopClothesDbContext _context;
        public UnitOfWork(ShopClothesDbContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
