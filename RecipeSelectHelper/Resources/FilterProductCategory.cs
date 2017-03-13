using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class FilterProductCategory : Boolable<ProductCategory>
    {
        //public Boolable<ProductCategory> Boolable { get; set; }
        //public FilterProductCategory(Boolable<ProductCategory> rcBoolable)
        //{
        //    if(rcBoolable == null) throw new ArgumentException();
        //    Boolable = rcBoolable;
        //}

        public FilterProductCategory(ProductCategory instance) : base(instance)
        {
        }
    }
}
