using PMApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMApi.PriceEngine
{
    public interface IPriceEngine
    {
        List<PriceValue> GetPrices(string[] symbols);
    }
}