using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public static class OrderBy
    {
        public static IEnumerable<Product> OrderByName(IEnumerable<Product> products)
        {
            return products.OrderBy(x => x.Name);
        }

        public static IEnumerable<BoughtProduct> OrderByName(IEnumerable<BoughtProduct> products)
        {
            return products.OrderBy(x => x.CorrespondingProduct.Name);
        }

        public static IEnumerable<Recipe> OrderByName(IEnumerable<Recipe> recipes)
        {
            return recipes.OrderBy(x => x.Name);
        }
    }
}
