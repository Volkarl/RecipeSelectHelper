using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "SortingMethod")]
    public class SortingMethod : ISortingMethod
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<IPreference> Preferences { get; }

        public SortingMethod(string name, List<IPreference> preferences)
        {
            Name = name;
            Preferences = preferences;
        }

        public void Execute(ProgramData data)
        {
            if (Preferences == null || data == null) return;
            foreach (IPreference preference in Preferences)
            {
                preference.Calculate(data);
            }
        }
    }
}