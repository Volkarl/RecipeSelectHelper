using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    public interface IProduct
    {
        string Name { get; set; }
        int Value { get; set; }
        int ID { get; }
        List<ProductCategory> Categories { get; set; }
        List<Product> SubstituteProducts { get; set; }
    }
}
