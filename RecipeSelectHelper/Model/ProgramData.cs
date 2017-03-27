using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "ProgramData")]
    public class ProgramData
    {
        [DataMember]
        public string CompatibilityVersion { get; set; }
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
        [DataMember]
        public List<GroupedSelection<ProductCategory>> AllGroupedProductCategories { get; set; }
        [DataMember]
        public List<GroupedSelection<RecipeCategory>> AllGroupedRecipeCategories { get; set; }

        public static string ProgramVersion = "Version 1.0";
        public ProgramData(string compatibilityVersion = "Version 1.0")
        {
            CompatibilityVersion = compatibilityVersion;
            AllProducts = new List<Product>();
            AllBoughtProducts = new List<BoughtProduct>();
            AllRecipes = new List<Recipe>();
            AllProductCategories = new List<ProductCategory>();
            AllRecipeCategories = new List<RecipeCategory>();
            AllSortingMethods = new List<SortingMethod>();
            AllGroupedProductCategories = new List<GroupedSelection<ProductCategory>>();
            AllGroupedRecipeCategories = new List<GroupedSelection<RecipeCategory>>();
        }

        public void ResetAllValues()
        {
            AllProductCategories.ForEach(x => x.Value = 0);
            AllBoughtProducts.ForEach(x => x.Value = 0);
            AllProducts.ForEach(x => x.OwnValue = 0);
            AllRecipeCategories.ForEach(x => x.Value = 0);
            AllRecipes.ForEach(x => x.OwnValue = 0);
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
                //hash += bp.ID.GetHashCode();
                hash += bp.Value.GetHashCode();
                //hash += bp.CorrespondingProduct.ID.GetHashCode();
            }

            foreach (ProductCategory pc in this.AllProductCategories)
            {
                hash += pc.Name.GetHashCode();
                hash += pc.Value.GetHashCode();
            }

            foreach (Product p in this.AllProducts)
            {
                //hash += p.ID.GetHashCode();
                hash += p.Name.GetHashCode();
                hash += p.OwnValue.GetHashCode();
                foreach (ProductCategory s in p.Categories)
                {
                    hash += s.Name.GetHashCode();
                    hash += s.Value.GetHashCode();
                }
                foreach (Product b in p.SubstituteProducts)
                {
                    //hash += b.ID.GetHashCode();
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
                //hash += r.ID.GetHashCode();
                hash += r.Value.GetHashCode();
                foreach (Ingredient i in r.Ingredients)
                {
                    hash += i.AmountNeeded.GetHashCode();
                    hash += i.Value.GetHashCode();
                    //hash += i.CorrespondingProduct.ID.GetHashCode();
                }
            }

            foreach (SortingMethod sm in this.AllSortingMethods)
            {
                //hash += sm.GetHashCode();
            }

            return hash;
        }

        #region SafeRemoveItems

        public void RemoveElement(ProductCategory pc)
        {
            AllProductCategories.Remove(pc);
            foreach (Product product in AllProducts)
            {
                product.Categories.Remove(pc);
            }
        }

        public void RemoveElement(RecipeCategory rc)
        {
            AllRecipeCategories.Remove(rc);
            foreach (Recipe recipe in AllRecipes)
            {
                recipe.Categories.Remove(rc);
            }
        }

        public void RemoveElement(GroupedSelection<ProductCategory> gpc)
        {
            AllGroupedProductCategories.Remove(gpc);
            foreach (Product product in AllProducts)
            {
                product.GroupedCategories.RemoveAll(x => x.CorrespondingGroupedSelection.Equals(gpc));
            }
        }

        public void RemoveElement(GroupedSelection<RecipeCategory> grc)
        {
            AllGroupedRecipeCategories.Remove(grc);
            foreach (Recipe recipe in AllRecipes)
            {
                recipe.GroupedCategories.RemoveAll(x => x.CorrespondingGroupedSelection.Equals(grc));
            }
        }

        public void RemoveElement(Product p)
        {
            AllProducts.Remove(p);
            AllBoughtProducts.RemoveAll(x => x.CorrespondingProduct.Equals(p));
            foreach (Product allP in AllProducts)
            {
                allP.SubstituteProducts.Remove(p);
            }
            foreach (Recipe r in AllRecipes)
            {
                r.Ingredients.RemoveAll(x => x.CorrespondingProduct.Equals(p));
            }
        }

        public void RemoveElement(BoughtProduct bp)
        {
            AllBoughtProducts.Remove(bp);
        }

        public void RemoveElement(Recipe r)
        {
            AllRecipes.Remove(r);
        }

        public void RemoveElement(SortingMethod sm)
        {
            AllSortingMethods.Remove(sm);
        }

        #endregion

        //public void Import(List<RecipeCategory> importedData, out List<RecipeCategory> conflicts)
        //{
        //    conflicts = new ProgramData();
        //    var s = new ValidityChecker(importedData);

        //    ImportRc(im);

        //    if(s.RecipeIsValid())
        //    return;
        //}

        public void Merge(ProgramData dataToAdd)
        {
            AllProductCategories = AllProductCategories.Union(dataToAdd.AllProductCategories).ToList();
            AllGroupedProductCategories = AllGroupedProductCategories.Union(dataToAdd.AllGroupedProductCategories).ToList();
            AllRecipeCategories = AllRecipeCategories.Union(dataToAdd.AllRecipeCategories).ToList();
            AllGroupedRecipeCategories = AllGroupedRecipeCategories.Union(dataToAdd.AllGroupedRecipeCategories).ToList();
            AllProducts = AllProducts.Union(dataToAdd.AllProducts).ToList();
            AllBoughtProducts = AllBoughtProducts.Union(dataToAdd.AllBoughtProducts).ToList();
            AllRecipes = AllRecipes.Union(dataToAdd.AllRecipes).ToList();
            AllSortingMethods = AllSortingMethods.Union(dataToAdd.AllSortingMethods).ToList();
        }
    }
}
