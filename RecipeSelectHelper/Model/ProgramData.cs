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

        public int GetValueHashCode()
        {
            int hash = 0;
            hash += this.AllBoughtProducts.Count.GetHashCode();
            hash += this.AllProductCategories.Count.GetHashCode();
            hash += this.AllProducts.Count.GetHashCode();
            hash += this.AllRecipeCategories.Count.GetHashCode();
            hash += this.AllRecipes.Count.GetHashCode();
            hash += this.AllSortingMethods.Count.GetHashCode();

            foreach (BoughtProduct bp in this.AllBoughtProducts)
            {
                hash += bp.ID.GetHashCode();
                hash += bp.Value.GetHashCode();
                hash += bp.CorrespondingProduct.ID.GetHashCode();
            }

            foreach (ProductCategory pc in this.AllProductCategories)
            {
                hash += pc.Name.GetHashCode();
                hash += pc.Value.GetHashCode();
            }

            foreach (Product p in this.AllProducts)
            {
                hash += p.ID.GetHashCode();
                hash += p.Name.GetHashCode();
                hash += p.Value.GetHashCode();
                foreach (ProductCategory s in p.Categories)
                {
                    hash += s.Name.GetHashCode();
                    hash += s.Value.GetHashCode();
                }
                foreach (Product b in p.SubstituteProducts)
                {
                    hash += b.ID.GetHashCode();
                }
            }

            foreach (RecipeCategory rc in this.AllRecipeCategories)
            {
                hash += rc.Name.GetHashCode();
                hash += rc.Value.GetHashCode();
            }

            foreach (Recipe r in this.AllRecipes)
            {
                hash += r.CategoriesAsString.GetHashCode();
                hash += r.Description.GetHashCode();
                hash += r.ID.GetHashCode();
                hash += r.Value.GetHashCode();
                foreach (Ingredient i in r.Ingredients)
                {
                    hash += i.AmountNeeded.GetHashCode();
                    hash += i.Value.GetHashCode();
                    hash += i.CorrespondingProduct.ID.GetHashCode();
                }
            }

            foreach (SortingMethod sm in this.AllSortingMethods)
            {
                //hash += sm.GetHashCode();
            }

            return hash;
        }
    }
}
