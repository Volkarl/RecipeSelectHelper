using System;

namespace RecipeSelectHelper.Model.SortingMethods
{
    public class Preference : IPreference
    {
        private Action<ProgramData> _calculationMethod;

        public Preference(Action<ProgramData> calculationMethod)
        {
            this._calculationMethod = calculationMethod;
        }

        public void Calculate(ProgramData pd)
        {
            _calculationMethod(pd);
        }
        // FX. RecipeCategoryPreference der arver herfra, der fx tager imod en værdi der tilføjes alle values 
        // (eller et delegate der udregner værdien)
    }
}