using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class ValidityChecker
    {
        private ProgramData _programData;

        public ValidityChecker(ProgramData programData)
        {
            _programData = programData;
        }

        public bool RecipeNameIsValid(string name, out string error)
        {
            bool nameUnique = RecipeNameIsUnique(name);
            bool spellingValid = RecipeNameSpellingIsValid(name, out error);
            if (!nameUnique) error = "Recipe name is already taken";
            return nameUnique && spellingValid;
        }

        private bool RecipeNameIsUnique(string name)
        {
            return !_programData.AllRecipes.Any(x => x.Name.Equals(name));
        }

        private bool RecipeNameSpellingIsValid(string name, out string error)
        {
            error = String.Empty;
            return true;
            throw new NotImplementedException();
        }

        public bool DescriptionIsValid(string name, out string error)
        {
            error = String.Empty;
            return true;
            throw new NotImplementedException();
        }

        public bool InstructionIsValid(string name, out string error)
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
    }
}
