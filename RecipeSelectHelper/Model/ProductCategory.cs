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

        public int OwnValue { get; set; }

        private ProductCategory() { }

        public ProductCategory(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
