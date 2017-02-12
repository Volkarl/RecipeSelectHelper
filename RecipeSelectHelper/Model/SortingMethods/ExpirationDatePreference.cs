using System;
using System.Runtime.Serialization;
using System.Windows;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "ExpirationDatePreference")]
    public class ExpirationDatePreference : Preference
    {
        public ExpirationDatePreference(string description = null) : base(description)
        {
            Description += nameof(ExpirationDatePreference) + " | No one knows what goes on here.";
        }

        public override void Calculate(ProgramData pd)
        {
            MessageBox.Show("No one knows");
        }
    }
}