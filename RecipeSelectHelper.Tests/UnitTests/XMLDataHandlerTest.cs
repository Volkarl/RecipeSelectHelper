using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Tests.UnitTests
{
    [TestFixture]
    public class XmlDataHandlerTest
    {
        string _testFilePath;
        private ProgramData[] _data;

        [OneTimeSetUp]
        public void Initialize()
        {
            _data = new ProgramData[3];
            _testFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "testData.xml");

            List<ProductCategory> pc = AddPC();
            List<RecipeCategory> rc = AddRC();
            List<Product> p = AddP(pc);
            List<BoughtProduct> bp = AddBP(p);
            List<Recipe> r = AddR(rc, p);
            List<SortingMethod> sm = AddSM(pc);

            ProgramData dataLowComplexity =    addProgramData(0, pc, rc, p, bp, r, sm);
            ProgramData dataMediumComplexity = addProgramData(1, pc, rc, p, bp, r, sm);
            ProgramData dataHighComplexity =   addProgramData(2, pc, rc, p, bp, r, sm);
            _data[0] = dataLowComplexity;
            _data[1] = dataMediumComplexity;
            _data[2] = dataHighComplexity;
        }

        private ProgramData addProgramData(int complexity, List<ProductCategory> pc, List<RecipeCategory> rc, List<Product> p, List<BoughtProduct> bp, List<Recipe> r, List<SortingMethod> sm)
        {
            var dataL = new ProgramData
            {
                AllProductCategories = pc,
                AllRecipeCategories = rc,
                AllProducts = p,
                AllBoughtProducts = new List<BoughtProduct>(),
                AllRecipes = new List<Recipe>()
            };

            for (int i = complexity; i >= 0; i--)
            {
                dataL.AllBoughtProducts.Add(bp[i]);
                dataL.AllRecipes.Add(r[i]);
            }

            dataL.AllSortingMethods = sm;
            return dataL;
        }

        private List<SortingMethod> AddSM(List<ProductCategory> productCategories)
        {
            var sm = new List<SortingMethod>();
            for (int i = 0; i < 2; i++)
            {
                sm.Add(new SortingMethod("SM" + i, new List<Preference> {new ProductCategoryPreference(i, productCategories[i])}));
            }
            return sm;
        }

        private List<Recipe> AddR(List<RecipeCategory> recipeCategories, List<Product> products)
        {
            var recipes = new List<Recipe>();
            var ingredients = new List<Ingredient>();

            for (int index = 0; index < products.Count; index++)
            {
                ingredients.Add(new Ingredient(Convert.ToUInt32(index), products[index]));
            }

            int i = 0;
            int servings = 1;
            recipes.Add(new Recipe("RLowComplexity", servings, "D" + i, "I" + i++));
            recipes.Add(new Recipe("RMediumComplexity", servings, "D" + i, "I" + i++, categories:recipeCategories));
            recipes.Add(new Recipe("RHighComplexity", servings, "D" + i, "I" + i, ingredients, recipeCategories));

            return recipes;
        }

        private List<BoughtProduct> AddBP(List<Product> products)
        {
            var bp = new List<BoughtProduct>();
            var ex = new ExpirationInfo();
            for (int index = 0; index < products.Count; index++)
            {
                var product = products[index];
                bp.Add(new BoughtProduct(product, Convert.ToUInt32(index), ex));
            }
            return bp;
        }

        private List<Product> AddP(List<ProductCategory> productCategories)
        {
            var p = new List<Product>();
            var p1 = new Product("PLowComplexity");
            var p2 = new Product("PMediumComplexity", productCategories);
            var p3 = new Product("PHighComplexity", productCategories);
            p.Add(p1);
            p.Add(p2);
            p.Add(p3);
            return p;
        }

        private List<RecipeCategory> AddRC()
        {
            var pc = new List<RecipeCategory>();
            for (int i = 0; i < 3; i++)
            {
                pc.Add(new RecipeCategory("RC" + i.ToString()));
            }
            return pc;
        }

        private List<ProductCategory> AddPC()
        {
            var pc = new List<ProductCategory>();
            for (int i = 0; i < 3; i++)
            {
                pc.Add(new ProductCategory("PC" + i.ToString()));
            }
            return pc;
        }

        [TearDown]
        public void CleanUp()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ToXMLAndFromXML_ValidProgramData_Works(int i)
        {
            XmlDataHandler.SaveToXml(_testFilePath, _data[i]);
            ProgramData deserializedData = XmlDataHandler.FromXml(_testFilePath);
            Assert.AreEqual(_data[i].GetValueHashCode(), deserializedData.GetValueHashCode());
        }
    }
}
