﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopClothes.Domain.Interface
{
    public interface IMutiLanguage<T>
    {
        T LanguageId { set; get; }
    }
}
