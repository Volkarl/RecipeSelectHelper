using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    public interface IProduct
    {
        string Name { get; set; }
        ValueInformation OwnValue { get; }
        int Value { get; }
        //int ID { get; }
        List<ProductCategory> Categories { get; set; }
    }
}
