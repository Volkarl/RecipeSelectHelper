using System.Collections.Generic;

namespace RecipeSelectHelper.Model.SortingMethods
{
    public interface ISortingMethod
    {
        string Name { get; set; }
        List<Preference> Preferences { get; set; }
        void Execute(ProgramData data, bool allowSubstitutes);
    }
}