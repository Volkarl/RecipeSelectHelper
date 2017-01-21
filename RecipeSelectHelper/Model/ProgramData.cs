using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    public class ProgramData
    {
        public List<IProduct> AllProducts { get; private set; }
        public List<IBoughtProduct> AllBoughtProducts { get; private set; }
        public List<IRecipe> AllRecipes { get; private set; }
        public List<IProductCategory> AllProductCategories { get; private set; }
        public List<IRecipeCategory> AllRecipeCategories { get; private set; }

        public List<ISortingMethod> AllSortingMethods { get; private set; }
        // ?? How?

        public void Load()
        {
            AllProducts = LoadStoreProducts();
            AllBoughtProducts = LoadBoughtProducts();
            AllRecipes = LoadRecipes();
            AllProductCategories = LoadProductCategories();
            AllRecipeCategories = LoadRecipeCategories();

            AllSortingMethods = LoadSortingMethods();
        }

        private List<IRecipeCategory> LoadRecipeCategories()
        {
            return null;
            throw new NotImplementedException();
        }

        private List<ISortingMethod> LoadSortingMethods()
        {
            return null;
            throw new NotImplementedException();
        }

        private List<IProductCategory> LoadProductCategories()
        {
            return null;
            throw new NotImplementedException();
        }

        private List<IRecipe> LoadRecipes()
        {
            return null;
            throw new NotImplementedException();
        }

        private List<IBoughtProduct> LoadBoughtProducts()
        {
            return null;
            throw new NotImplementedException();
        }

        private List<IProduct> LoadStoreProducts()
        {
            return null;
            throw new NotImplementedException();
        }
    }
}
