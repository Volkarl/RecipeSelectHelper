using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "ProductCategoryPreference")]
    public class ProductCategoryPreference : Preference
    {
        [DataMember]
        public int Val { get; set; }
        [DataMember]
        public ProductCategory ProductCategory { get; set; }

        public ProductCategoryPreference(int val, ProductCategory productCategory)
        {
            Val = val;
            ProductCategory = productCategory;
            Description += nameof(ProductCategoryPreference) + " | Add " + val + " to all products of category: " + ProductCategory.Name;
        }

        public override void Calculate(ProgramData pd)
        {
            if (ProductCategory == null) return;
            pd.AllProductCategories.Find(y => y.Equals(ProductCategory)).Value += Val;   //this definitely works
        }
    }
}
