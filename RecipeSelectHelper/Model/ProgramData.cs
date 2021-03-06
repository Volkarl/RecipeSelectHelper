﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        [DataMember]
        public SubstituteRelationsDictionary ProductSubstitutes { get; set; }

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
            ProductSubstitutes = new SubstituteRelationsDictionary();
        }

        private ProgramData() { }

        public void ResetAllValues()
        {
            AllProductCategories.ForEach(x => x.Reset());
            AllBoughtProducts.ForEach(x => x.Reset());
            AllProducts.ForEach(x => x.Reset());
            AllRecipeCategories.ForEach(x => x.Reset());
            AllRecipes.ForEach(x => x.Reset());
        }

        public int GetValueHashCode()
        {
            int hash = 0;
            hash += AllBoughtProducts.Count.GetHashCode();
            hash += AllProductCategories.Count.GetHashCode();
            hash += AllProducts.Count.GetHashCode();
            hash += AllRecipeCategories.Count.GetHashCode();
            hash += AllRecipes.Count.GetHashCode();
            hash += AllSortingMethods.Count.GetHashCode();

            foreach (BoughtProduct bp in AllBoughtProducts)
            {
                hash += bp.Amount.GetHashCode();
            }

            foreach (ProductCategory pc in AllProductCategories)
            {
                hash += pc.Name.GetHashCode();
            }

            foreach (Product p in AllProducts)
            {
                hash += p.Name.GetHashCode();
                foreach (ProductCategory s in p.Categories)
                {
                    hash += s.Name.GetHashCode();
                }
            }

            foreach (RecipeCategory rc in AllRecipeCategories)
            {
                hash += rc.Name.GetHashCode();
            }

            foreach (Recipe r in AllRecipes)
            {
                hash += r.CategoriesAsString.GetHashCode();
                hash += r.Description.GetHashCode();
                foreach (Ingredient i in r.Ingredients)
                {
                    hash += i.AmountNeeded.GetHashCode();
                }
            }

            foreach (SortingMethod sm in AllSortingMethods)
            {
                hash += sm.Name.GetHashCode();
                foreach (Preference p in sm.Preferences) hash += p.Description.GetHashCode();
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
            ProductSubstitutes.RemoveProduct(p);
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
            // ProductSubstitutes
            throw new NotImplementedException();
        }
    }
}
