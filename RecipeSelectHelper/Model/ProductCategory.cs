using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "ProductCategory")]
    public class ProductCategory : IProductCategory
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Value { get; set; }

        public ProductCategory(string name)
        {
            Name = name;
        }
    }
}
