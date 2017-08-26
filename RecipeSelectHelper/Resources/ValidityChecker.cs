using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;

namespace RecipeSelectHelper.Resources
{
    public class ValidityChecker
    {
        private ProgramData _programData;

        public ValidityChecker(ProgramData programData)
        {
            _programData = programData;
        }

        public bool RecipeIsValid(Recipe recipe, out List<string> errors)
        {
            var errorArray = new string[6];
            int i = 0;
            if (RecipeNameIsValid(recipe.Name) &&
                DescriptionIsValid(recipe.Description, out errorArray[i++]) &&
                InstructionIsValid(recipe.Instructions, out errorArray[i++]) &&
                GroupedRcAreValid(recipe.GroupedCategories, out errorArray[i++]) &&
                CategoriesAreValid(recipe.Categories, out errorArray[i++]) &&
                IngredientsAreValid(recipe.Ingredients, out errorArray[i]))
            {
                errors = null;
                return true;
            }
            errors = errorArray.SkipWhile(string.IsNullOrWhiteSpace).ToList();
            return false;
        }

        public bool RecipeNameIsValid(string r)
        {
            bool nameUnique = RecipeNameIsUnique(r);
            bool spellingValid = RecipeNameSpellingIsValid(r);
            return nameUnique && spellingValid;
        }

        private bool RecipeNameSpellingIsValid(string r)
        {
            return !string.IsNullOrWhiteSpace(r);
        }

        public bool DescriptionIsValid(string name, out string error)
        {
            error = String.Empty;
            return true;
            throw new NotImplementedException();
        }

        public bool InstructionIsValid(StringList name, out string error)
        {
            error = String.Empty;
            return true;
            throw new NotImplementedException();
        }

        public bool CategoriesAreValid(List<RecipeCategory> categories, out string error)
        {
            error = String.Empty;
            return true;
            throw new NotImplementedException();
        }

        public bool GroupedRcAreValid(List<GroupedRecipeCategory> groupedRc, out string error)
        {
            error = String.Empty;
            foreach (GroupedRecipeCategory grc in groupedRc)
            {
                if (!grc.SelectionIsValid(out error))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IngredientsAreValid(List<Ingredient> ingredients, out string error)
        {
            error = String.Empty;
            return true;
            throw new NotImplementedException();
        }

        public class PcComparer : IEqualityComparer<ProductCategory>
        {
            public bool Equals(ProductCategory x, ProductCategory y)
            {
                return x.Name.Equals(y.Name);
            }

            public int GetHashCode(ProductCategory obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public class RcComparer : IEqualityComparer<RecipeCategory>
        {
            public bool Equals(RecipeCategory x, RecipeCategory y)
            {
                return x.Name.Equals(y.Name);
            }

            public int GetHashCode(RecipeCategory obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public bool RecipeCategoryNameIsValid(string name) => !string.IsNullOrWhiteSpace(name) && RecipeCategoryNameIsUnique(name);
        public bool ProductCategoryNameIsValid(string name) => !string.IsNullOrWhiteSpace(name) && ProductCategoryNameIsUnique(name);
        public bool StoreProductNameIsValid(string name) => !string.IsNullOrWhiteSpace(name) && StoreProductNameIsUnique(name);

        private bool RecipeCategoryNameIsUnique(string rc) => !_programData.AllRecipeCategories.Any(x => x.Name.Equals(rc));
        private bool ProductCategoryNameIsUnique(string pc) => !_programData.AllProductCategories.Any(x => x.Name.Equals(pc));
        private bool RecipeNameIsUnique(string r) => !_programData.AllRecipes.Any(x => x.Name.Equals(r));
        private bool StoreProductNameIsUnique(string p) => !_programData.AllProducts.Any(x => x.Name.Equals(p));
    }
}