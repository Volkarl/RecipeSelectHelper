using System;
using System.Runtime.Serialization;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "SortingMethod")]
    public class SortingMethod : ISortingMethod
    {
        [DataMember]
        public string Name { get; set; }

        public SortingMethod(string name)
        {
            Name = name;
        }
    }
}