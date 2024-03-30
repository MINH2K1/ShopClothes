using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Infastructure.Dto
{
   public class PagedResult<T> :PageResultBase where T : class
    {

        public PagedResult()
        {
            Results = new List<T>();
        }
        public IList<T> Results { get; set; }

        

    }
}
