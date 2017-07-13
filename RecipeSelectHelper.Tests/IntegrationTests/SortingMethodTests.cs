using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Tests.IntegrationTests
{
    [TestFixture]
    public class SortingMethodTests
    {
        private static class SortingMethods
        {
            public static SortingMethod Empty(ProgramData pd)
            {
                return new SortingMethod(SortingMethodType.Empty.ToString(), CreatePreferenceList(SortingMethodType.Empty, pd));
            }

            public static SortingMethod ProductCategory(ProgramData pd)
            {
                return new SortingMethod(SortingMethodType.ProductCategory.ToString(), CreatePreferenceList(SortingMethodType.ProductCategory, pd));
            }

            public static SortingMethod RecipeCategory(ProgramData pd)
            {
                return new SortingMethod(SortingMethodType.RecipeCategory.ToString(), CreatePreferenceList(SortingMethodType.RecipeCategory, pd));
            }

            public static SortingMethod IngredientsOwned(ProgramData pd)
            {
                return new SortingMethod(SortingMethodType.IngredientsOwned.ToString(), CreatePreferenceList(SortingMethodType.IngredientsOwned, pd));
            }

            public static SortingMethod SingleIngredient(ProgramData pd)
            {
                return new SortingMethod(SortingMethodType.SingleIngredient.ToString(), CreatePreferenceList(SortingMethodType.SingleIngredient, pd));
            }

            public static SortingMethod ExpirationDate(ProgramData pd)
            {
                return new SortingMethod(SortingMethodType.ExpirationDate.ToString(), CreatePreferenceList(SortingMethodType.ExpirationDate, pd));
            }
        }

        private enum SortingMethodType
        {
            Empty, ProductCategory, RecipeCategory, IngredientsOwned, SingleIngredient, ExpirationDate, All
        }

        private static class ProgramDatas
        {
            public static ProgramData Empty = CreateProgramData(ProgramDataType.Empty);
            public static ProgramData NoCoupling = CreateProgramData(ProgramDataType.NoCoupling);
            public static ProgramData LowCoupling = CreateProgramData(ProgramDataType.LowCoupling);
            public static ProgramData MedCoupling = CreateProgramData(ProgramDataType.MedCoupling);
            public static ProgramData HighCoupling = CreateProgramData(ProgramDataType.HighCoupling);
            public static ProgramData NoCouplingWithSubs = CreateProgramData(ProgramDataType.NoCoupling, true);
            public static ProgramData LowCouplingWithSubs = CreateProgramData(ProgramDataType.LowCoupling, true);
            public static ProgramData MedCouplingWithSubs = CreateProgramData(ProgramDataType.MedCoupling, true);
            public static ProgramData HighCouplingWithSubs = CreateProgramData(ProgramDataType.HighCoupling, true);
        }

        private enum ProgramDataType
        {
            Empty, NoCoupling, LowCoupling, MedCoupling, HighCoupling
        }

        private static ProgramData CreateProgramData(ProgramDataType i, bool genWithSubstitutes = false)
        {
            var pd = new ProgramData();
            List<ProductCategory> pc = GenPc();
            List<RecipeCategory> rc = GenRc();
            List<Product> p;
            List<Recipe> r;
            List<BoughtProduct> bp;
            switch (i)
            {
                case ProgramDataType.Empty:
                    return pd;
                case ProgramDataType.NoCoupling:
                    p = GenP();
                    r = GenR();
                    bp = GenBp();
                    break;
                case ProgramDataType.LowCoupling:
                    p = GenP(pc);
                    r = GenR(rc);
                    bp = GenBp();
                    break;
                case ProgramDataType.MedCoupling:
                    p = GenP(pc);
                    r = GenR(rc);
                    bp = GenBp(p);
                    break;
                case ProgramDataType.HighCoupling:
                    p = GenP(pc);
                    r = GenR(rc, p);
                    bp = GenBp(p);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, null);
            }
            Dictionary<Product, List<Product>> subs = genWithSubstitutes ? GenSub(p) : GenSub();
            AddToData(pd, pc, rc, p, r, bp, subs);
            return pd;
        }

        private static Dictionary<Product, List<Product>> GenSub(List<Product> products = null)
        {
            Dictionary<Product, List<Product>> subs = new Dictionary<Product, List<Product>>();
            if (products != null)
            {
                List<Product> previous = new List<Product>();
                foreach (Product product in products)
                {
                    if(previous.Any()) subs.Add(product, previous);
                    previous.Add(product);
                }
            }
            return subs;
            // Each product will have all the previous products as substitutes
        }

        private static List<BoughtProduct> GenBp(List<Product> products = null)
        {
            if (products == null)
            {
                Product noCoupling = new Product("noCouplingProduct");
                return new List<BoughtProduct>
                { new BoughtProduct(noCoupling, 1), new BoughtProduct(noCoupling, 2), new BoughtProduct(noCoupling, 3)};
            }
            uint amount = 1;
            return new List<BoughtProduct>(products.ConvertAll(x => new BoughtProduct(x, amount++)));
        }

        private static List<Recipe> GenR(List<RecipeCategory> rc = null, List<Product> products = null)
        {
            List<Recipe> recipes;
            
            if(rc != null && products != null) recipes = new List<Recipe>
            {
                new Recipe("rec0", categories:rc[0].ToSingleItemList(), ingredients:new Ingredient(0, products[0]).ToSingleItemList()),
                new Recipe("rec1", categories:rc[1].ToSingleItemList(), ingredients:new Ingredient(1, products[1]).ToSingleItemList()),
                new Recipe("rec2", categories:rc[2].ToSingleItemList(), ingredients:new Ingredient(2, products[2]).ToSingleItemList())
            };
            else if(rc != null) recipes = new List<Recipe>
            {
                new Recipe("rec0", categories:rc[0].ToSingleItemList()),
                new Recipe("rec1", categories:rc[1].ToSingleItemList()),
                new Recipe("rec2", categories:rc[2].ToSingleItemList())
            };
            else if (products != null)recipes = new List<Recipe>
            {
                new Recipe("rec0", ingredients:new Ingredient(0, products[0]).ToSingleItemList()),
                new Recipe("rec1", ingredients:new Ingredient(1, products[1]).ToSingleItemList()),
                new Recipe("rec2", ingredients:new Ingredient(2, products[2]).ToSingleItemList())
            };
            else recipes = new List<Recipe> { new Recipe("rec0"), new Recipe("rec1"), new Recipe("rec2") };

            return recipes;
        }

        private static List<Product> GenP(List<ProductCategory> pc = null)
        {
            if(pc == null) return new List<Product> {new Product("0"), new Product("1"), new Product("2")};
            return new List<Product>
            {
                new Product("0", pc[0].ToSingleItemList()),
                new Product("1", pc[1].ToSingleItemList()),
                new Product("2", pc[2].ToSingleItemList())
            };
        }

        private static List<RecipeCategory> GenRc()
        {
            return new List<RecipeCategory> {new RecipeCategory("0"), new RecipeCategory("1"), new RecipeCategory("2")};
        }

        private static List<ProductCategory> GenPc()
        {
            return new List<ProductCategory> { new ProductCategory("0"), new ProductCategory("1"), new ProductCategory("2") };
        }

        private static void AddToData(ProgramData pd, List<ProductCategory> pc = null, List<RecipeCategory> rc = null, List<Product> p = null, List<Recipe> r = null, List<BoughtProduct> bp = null, Dictionary<Product, List<Product>> subs = null)
        {
            if (pc != null) pd.AllProductCategories.AddRange(pc);
            if (rc != null) pd.AllRecipeCategories.AddRange(rc);
            if (p != null) pd.AllProducts.AddRange(p);
            if (r != null) pd.AllRecipes.AddRange(r);
            if (bp != null) pd.AllBoughtProducts.AddRange(bp);
            if (subs != null)
                foreach (KeyValuePair<Product, List<Product>> pair in subs)
                    pd.ProductSubstitutes.AddSubstitutes(pair.Key, pair.Value);
        }

        private static List<Preference> CreatePreferenceList(SortingMethodType i, ProgramData pd)
        {
            var pref = new List<Preference>();
            const int val = 1;
            switch (i)
            {
                case SortingMethodType.Empty: 
                    break;
                case SortingMethodType.ProductCategory:
                    pref.Add(new ProductCategoryPreference(val, pd.AllProductCategories.FirstOrDefault()));
                    break;
                case SortingMethodType.RecipeCategory:
                    pref.Add(new RecipeCategoryPreference(val, pd.AllRecipeCategories.FirstOrDefault()));
                    break;
                case SortingMethodType.IngredientsOwned:
                    pref.Add(new IngredientsOwnedPreference(val));
                    break;
                case SortingMethodType.SingleIngredient:
                    pref.Add(new SingleIngredientPreference(val, pd.AllProducts.FirstOrDefault()));
                    break;
                case SortingMethodType.ExpirationDate:
                    pref.Add(new ExpirationDatePreference(val));
                    break;
                case SortingMethodType.All: 
                    pref.AddRange(CreatePreferenceList(SortingMethodType.Empty, pd));
                    pref.AddRange(CreatePreferenceList(SortingMethodType.ProductCategory, pd));
                    pref.AddRange(CreatePreferenceList(SortingMethodType.RecipeCategory, pd));
                    pref.AddRange(CreatePreferenceList(SortingMethodType.IngredientsOwned, pd));
                    pref.AddRange(CreatePreferenceList(SortingMethodType.SingleIngredient, pd));
                    pref.AddRange(CreatePreferenceList(SortingMethodType.ExpirationDate, pd));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, null);
            }
            return pref;
        }

        [TestCase] 
        public void ProductCategoryPreference_AllRecipesContainProductCategory_AllRecipesRecieve1Point()
        {
            ProgramData pd = new ProgramData();
            pd.AllProductCategories = new List<ProductCategory> { new ProductCategory("mypc")};
            pd.AllProducts = new List<Product>
            {
                new Product("0", pd.AllProductCategories),
                new Product("1", pd.AllProductCategories),
                new Product("2", pd.AllProductCategories)
            };
            pd.AllRecipes = GenR(products: pd.AllProducts);

            ExecuteAndVerifyResult(pd, false, SortingMethodType.ProductCategory, new []{1, 1, 1});
        }
        
        private void ExecuteAndVerifyResult(ProgramData pd, bool allowSubs, SortingMethodType smt,
            int[] expectedRecipeValues)
        {
            if(pd.AllRecipes.Count != expectedRecipeValues.Length) throw new ArgumentException("Mismatched array size");

            CreateAndExecutePreference(pd, allowSubs, smt);
            int i = 0;
            foreach (int val in pd.AllRecipes.ConvertAll(r => r.Value))
                Assert.AreEqual(val, expectedRecipeValues[i++]);
        }

        private void CreateAndExecutePreference(ProgramData pd, bool allowSubs, SortingMethodType smt)
        {
            List<Preference> pref = CreatePreferenceList(smt, pd);
            var sm = new SortingMethod("sm", pref);
            sm.Execute(pd, allowSubs);
        }

        private void ExecuteAndVerifyResult(ProgramDataType pdt, bool allowSubs, SortingMethodType smt, ProgramData expectedResult)
        {
            ProgramData pd = CreateProgramData(pdt);
            CreateAndExecutePreference(pd, allowSubs, smt);
            ReportNonEquivalence(expectedResult, pd);
        }
        
        private void ReportNonEquivalence(ProgramData expectedResult, ProgramData data)
        {
            // Todo: perhaps I should not check whether every item of each sequence is equal, but only whether the sorting is correct? 

            if (expectedResult == null || data == null) throw new ArgumentNullException(data == null ? nameof(data) : nameof(expectedResult));

            if (!expectedResult.AllProductCategories.ConvertAll(x => x.OwnValue).SequenceEqual(data.AllProductCategories.ConvertAll(x => x.OwnValue)))
                throw new ArgumentException(nameof(expectedResult.AllProductCategories));
            if (!expectedResult.AllRecipeCategories.ConvertAll(x => x.OwnValue).SequenceEqual(data.AllRecipeCategories.ConvertAll(x => x.OwnValue)))
                throw new ArgumentException(nameof(expectedResult.AllRecipeCategories));
            if (!expectedResult.AllProducts.ConvertAll(x => x.OwnValue).SequenceEqual(data.AllProducts.ConvertAll(x => x.OwnValue)))
                throw new ArgumentException(nameof(expectedResult.AllProducts));
            if (!expectedResult.AllBoughtProducts.ConvertAll(x => x.OwnValue).SequenceEqual(data.AllBoughtProducts.ConvertAll(x => x.OwnValue)))
                throw new ArgumentException(nameof(expectedResult.AllBoughtProducts));
            if (!expectedResult.AllRecipes.ConvertAll(x => x.OwnValue).SequenceEqual(data.AllRecipes.ConvertAll(x => x.OwnValue)))
                throw new ArgumentException(nameof(expectedResult.AllRecipes));
        }
    }
}
