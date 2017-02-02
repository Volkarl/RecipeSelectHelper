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

        ProgramData data1;
        ProgramData data2;
        ProgramData data3;

        [OneTimeSetUp]
        public void Initialize()
        {
            _testFilePath = "testData.xml";
            _xmlConverter = new XMLDataHandler(_testFilePath);

            List<ProductCategory> productCategories = AddPC();
            List<RecipeCategory> recipeCategories = AddRC();
            List<Product> products = AddP(productCategories);
            List<BoughtProduct> boughtProducts = AddBP();
            List<Recipe> recipes = AddR(recipeCategories, products);
            List<SortingMethod> sortingMethods = AddSM();



        }

        private List<SortingMethod> AddSM()
        {
            return new List<SortingMethod>();
        }

        private List<Recipe> AddR(List<RecipeCategory> recipeCategories, List<Product> products)
        {
            int i = 1;
            var recipes = new List<Recipe>();
            recipes.Add(new Recipe(i.ToString(), i.ToString(), i++.ToString()));
            recipes.Add(new Recipe(i.ToString(), i.ToString(), i++.ToString(), ));
            recipes.Add(new Recipe(i.ToString(), i.ToString(), i++.ToString()));

            return recipes;
        }

        private List<BoughtProduct> AddBP()
        {
            throw new NotImplementedException();
        }

        private List<Product> AddP(List<ProductCategory> productCategories)
        {
            throw new NotImplementedException();
        }

        private List<RecipeCategory> AddRC()
        {
            throw new NotImplementedException();
        }

        private List<ProductCategory> AddPC()
        {
            throw new NotImplementedException();
        }

        [OneTimeTearDown]
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

        [Test]
        public void ToXMLAndFromXML_Recipe_Works()
        {
            _xmlConverter.SaveToXML()
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }
    }
}
