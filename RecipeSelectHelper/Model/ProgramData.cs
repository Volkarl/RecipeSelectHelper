using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "ProgramData")]
    public class ProgramData
    {
        [DataMember]
        public List<Product> AllProducts { get; set; }
        [DataMember]
        public List<BoughtProduct> AllBoughtProducts { get; set; }
        [DataMember]
        public List<Recipe> AllRecipes { get; set; }
        [DataMember]
        public List<ProductCategory> AllProductCategories { get; set; }
        [DataMember]
        public List<RecipeCategory> AllRecipeCategories { get; set; }

        [DataMember]
        public List<SortingMethod> AllSortingMethods { get; set; }
        // ?? How?

        public ProgramData()
        {
            AllProducts = new List<Product>();
            AllBoughtProducts = new List<BoughtProduct>();
            AllRecipes = new List<Recipe>();
            AllProductCategories = new List<ProductCategory>();
            AllRecipeCategories = new List<RecipeCategory>();

            AllSortingMethods = new List<SortingMethod>();
        }
    }
}
