using System;

namespace RecipeSelectHelper.Model.SortingMethods
{
    public class Preference : IPreference
    {
        private Action<ProgramData> _calculationMethod;
        public string Description { get; private set; }

        public Preference(Action<ProgramData> calculationMethod, string description = null)
        {
            this._calculationMethod = calculationMethod;
            Description = description;
        }

        public void Calculate(ProgramData pd)
        {
            _calculationMethod(pd);
        }
    }
}