using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    public class ProductCategory : IProductCategory
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public ProductCategory(string name)
        {
            Name = name;
        }
    }
}
