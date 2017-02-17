using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "RecipeCategory")]
    public class RecipeCategory : IRecipeCategory
    {
        [DataMember]
        public string Name { get; set; }

        public int Value { get; set; }

        public RecipeCategory(string name)
        {
            Name = name;
        }
    }
}
