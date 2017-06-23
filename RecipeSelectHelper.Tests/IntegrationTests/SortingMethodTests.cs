using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Model.SortingMethods;

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
            public static ProgramData NoCouplingWithSubs = CreateProgramData(ProgramDataType.NoCouplingWithSubs);
            public static ProgramData LowCouplingWithSubs = CreateProgramData(ProgramDataType.LowCouplingWithSubs);
            public static ProgramData MedCouplingWithSubs = CreateProgramData(ProgramDataType.MedCouplingWithSubs);
            public static ProgramData HighCouplingWithSubs = CreateProgramData(ProgramDataType.HighCouplingWithSubs);
        }

        private enum ProgramDataType
        {
            Empty, NoCoupling, LowCoupling, MedCoupling, HighCoupling, NoCouplingWithSubs, LowCouplingWithSubs, MedCouplingWithSubs, HighCouplingWithSubs
        }

        private static ProgramData CreateProgramData(ProgramDataType i)
        {
            var pd = new ProgramData();
            List<ProductCategory> pc;
            List<RecipeCategory> rc;
            List<Product> p;
            List<Recipe> r;
            switch (i)
            {
                case ProgramDataType.Empty:
                    break;
                case ProgramDataType.NoCoupling:
                    AddToData(pd, GenPc(), GenRc(), GenP(), GenR(), GenBp());
                    break;
                case ProgramDataType.LowCoupling:
                    pc = GenPc();
                    rc = GenRc();
                    p = GenP(pc);
                    AddToData(pd, pc, rc, p, GenR(rc), GenBp(p));
                    break;
                case ProgramDataType.MedCoupling:
                    break;
                case ProgramDataType.HighCoupling:
                    break;
                case ProgramDataType.NoCouplingWithSubs:
                    break;
                case ProgramDataType.LowCouplingWithSubs:
                    pc = GenPc();
                    rc = GenRc();
                    p = GenP(pc);
                    AddToData(pd, pc, rc, p, GenR(rc), GenBp(p), GenSub(p));
                    break;
                case ProgramDataType.MedCouplingWithSubs:
                    break;
                case ProgramDataType.HighCouplingWithSubs:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), i, null);
            }
            return pd;
        }

        private static Dictionary<Product, List<Product>> GenSub(List<Product> products = null)
        {
            throw new NotImplementedException();
        }

        private static List<BoughtProduct> GenBp(List<Product> products = null)
        {
            throw new NotImplementedException();
        }

        private static List<Recipe> GenR(List<RecipeCategory> rc = null)
        {
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
