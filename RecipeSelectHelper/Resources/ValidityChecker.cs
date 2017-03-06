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

        public bool NameIsValid(string name, out string error)
        {
            return NameSpellingIsValid(name, out error) && NameIsUnique(name);
        }

        private bool NameIsUnique(string name)
        {
            throw new NotImplementedException();
        }

        private bool NameSpellingIsValid(string name, out string error)
        {
            throw new NotImplementedException();
        }

        public bool DescriptionIsValid(string name, out string error)
        {
            throw new NotImplementedException();
        }

        public bool InstructionIsValid(string name, out string error)
        {
            throw new NotImplementedException();
        }

        public bool CategoriesAreValid(List<RecipeCategory> categories, out string error)
        {
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
            throw new NotImplementedException();
        }
    }
}
