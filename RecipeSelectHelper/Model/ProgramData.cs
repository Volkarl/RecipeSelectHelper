using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    public class ProgramData
    {
        public IProduct AllStoreProducts { get; private set; }

        public void Load()
        {
            AllStoreProducts = LoadStoreProducts();
            AllBoughtProducts = LoadBoughtProducts();
            AllRecipes = LoadRecipes();
            AllCategories = LoadCategories();
            AllSortingMethods = LoadSortingMethods();
        }
    }
}
