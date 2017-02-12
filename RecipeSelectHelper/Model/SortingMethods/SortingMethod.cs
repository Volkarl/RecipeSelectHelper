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
        public List<Preference> Preferences { get; set; }

        public SortingMethod(string name, List<Preference> preferences)
        {
            Name = name;
            Preferences = preferences;
        }

        public void Execute(ProgramData data)
        {
            if (Preferences == null || data == null) return;
            foreach (Preference preference in Preferences)
            {
                preference.Calculate(data);
            }
        }
    }
}