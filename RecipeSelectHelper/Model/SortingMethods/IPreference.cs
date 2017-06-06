using System.Collections.Generic;

namespace RecipeSelectHelper.Model.SortingMethods
{
    public interface IPreference
    {
        string Description { get; set; }
        void Calculate(ProgramData data, Dictionary<BoughtProduct, uint> amountsInFridge);
    }
}