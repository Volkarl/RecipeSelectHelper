using NUnit.Framework;
using RecipeSelectHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Tests
{
    [TestFixture]
    public class XMLDataHandlerTest
    {
        XMLDataHandler _xmlConverter;
        string _testFilePath;
        List<ProgramData> data;

        [OneTimeSetUp]
        public void Initialize()
        {
            _testFilePath = "testData.xml";
            _xmlConverter = new XMLDataHandler(_testFilePath);

            List<ProductCategory> productCategories = AddPC();
            List<RecipeCategory> recipeCategories = AddRC();
            List<Product> products = AddP(productCategories);
            List<BoughtProduct> boughtProducts = AddBP(products);
            List<Recipe> recipes = AddR(recipeCategories, products);
            List<SortingMethod> sortingMethods = AddSM();

            ProgramData dataLowComplexity = addProgramData(0, productCategories, recipeCategories, products, boughtProducts, recipes, sortingMethods);
            ProgramData dataMediumComplexity = addProgramData(1, productCategories, recipeCategories, products, boughtProducts, recipes, sortingMethods);
            ProgramData dataHighComplexity = addProgramData(2, productCategories, recipeCategories, products, boughtProducts, recipes, sortingMethods);
            data.Add(dataLowComplexity);
            data.Add(dataMediumComplexity);
            data.Add(dataHighComplexity);
        }

        private ProgramData addProgramData(int complexity, List<ProductCategory> productCategories, List<RecipeCategory> recipeCategories, List<Product> products, List<BoughtProduct> boughtProducts, List<Recipe> recipes, List<SortingMethod> sortingMethods)
        {
            var data = new ProgramData();
            data.AllProductCategories = productCategories;
            data.AllRecipeCategories = recipeCategories;
            data.AllProducts = products;
            data.AllBoughtProducts = new List<BoughtProduct>();
            data.AllRecipes = new List<Recipe>();

            for (int i = complexity; i >= 0; i--)
            {
                data.AllBoughtProducts.Add(boughtProducts[i]);
                data.AllRecipes.Add(recipes[i]);
            }

            data.AllSortingMethods = sortingMethods;
            return data;
        }

        private List<SortingMethod> AddSM()
        {
            var sm = new List<SortingMethod>();
            for (int i = 0; i < 3; i++)
            {
                sm.Add(new SortingMethod("SM" + i.ToString()));
            }
            return sm;
        }

        private List<Recipe> AddR(List<RecipeCategory> recipeCategories, List<Product> products)
        {
            var recipes = new List<Recipe>();
            var ingredients = new List<Ingredient>();

            for (int index = 0; index < products.Count; index++)
            {
                ingredients.Add(new Ingredient(index, products[index]));
            }

            int i = 0;
            recipes.Add(new Recipe("RLowComplexity", "D" + i.ToString(), "I" + i++.ToString()));
            recipes.Add(new Recipe("RMediumComplexity", "D" + i.ToString(), "I" + i++.ToString(), categories:recipeCategories));
            recipes.Add(new Recipe("RHighComplexity", "D" + i.ToString(), "I" + i.ToString(), ingredients, recipeCategories));

            return recipes;
        }

        private List<BoughtProduct> AddBP(List<Product> products)
        {
            var bp = new List<BoughtProduct>();
            var ex = new ExpirationInfo();
            foreach (var product in products)
            {
                bp.Add(new BoughtProduct(product, ex));
            }
            return bp;
        }

        private List<Product> AddP(List<ProductCategory> productCategories)
        {
            var p = new List<Product>();
            var p1 = new Product("PLowComplexity");
            var p2 = new Product("PMediumComplexity", productCategories);
            var p3 = new Product("PHighComplexity", productCategories, new List<Product> { p1, p2 });
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
            else
            {
                throw new FileNotFoundException(_testFilePath + "does not exist and could not be deleted.");
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ToXMLAndFromXML_ValidProgramData_Works(int i)
        {
            _xmlConverter.SaveToXML(data[i]);
            ProgramData deserializedData = _xmlConverter.FromXML();
            Assert.AreEqual(data[i], deserializedData);
        }
    }
}
