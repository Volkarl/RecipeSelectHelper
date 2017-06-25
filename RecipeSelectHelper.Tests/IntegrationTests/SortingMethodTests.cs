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
        private struct SortingMethods
        {
            public static SortingMethod Empty = new SortingMethod(SortingMethodType.Empty.ToString(), CreatePreferenceList(SortingMethodType.Empty));
            public static SortingMethod ProductCategory = new SortingMethod(SortingMethodType.ProductCategory.ToString(), CreatePreferenceList(SortingMethodType.ProductCategory));
            public static SortingMethod RecipeCategory = new SortingMethod(SortingMethodType.RecipeCategory.ToString(), CreatePreferenceList(SortingMethodType.RecipeCategory));
            public static SortingMethod IngredientsOwned = new SortingMethod(SortingMethodType.IngredientsOwned.ToString(), CreatePreferenceList(SortingMethodType.IngredientsOwned));
            public static SortingMethod SingleIngredient = new SortingMethod(SortingMethodType.SingleIngredient.ToString(), CreatePreferenceList(SortingMethodType.SingleIngredient));
            public static SortingMethod ExpirationDate = new SortingMethod(SortingMethodType.ExpirationDate.ToString(), CreatePreferenceList(SortingMethodType.ExpirationDate));
        }

        private enum SortingMethodType
        {
            Empty, ProductCategory, RecipeCategory, IngredientsOwned, SingleIngredient, ExpirationDate, All
        }

        private struct ProgramDatas
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
            if(rc == null && products == null) return new List<Recipe> {new Recipe("rec0"), new Recipe("rec1"), new Recipe("rec2")};

            throw new NotImplementedException();
        }

        private static List<Product> GenP(List<ProductCategory> pc = null)
        {
            if(pc == null) return new List<Product> {new Product("0"), new Product("1"), new Product("2")};
            return new List<Product> {new Product("0", new List<ProductCategory> {pc[0]}), new Product("1", new List<ProductCategory> {pc[1]}), new Product("2", new List<ProductCategory> {pc[2]})};
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
            if(bp != null) pd.AllBoughtProducts.AddRange(bp);
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


        [OneTimeSetUp]
        public void Initialzie()
        {
            SortingMethod sm = CreateSortingMethod();
            sm.Execute(CreateProgramData(), true);
            var s = new ProgramData();
            s.
        }
    }
}
