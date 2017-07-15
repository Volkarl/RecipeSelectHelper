using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "ProductCategory")]
    public class ProductCategory : IProductCategory
    {
        [DataMember]
        public string Name { get; set; }

        private ValueInformation _ownValue = new ValueInformation();
        public ValueInformation OwnValue => _ownValue ?? new ValueInformation(); //Needed for deserialization

        public void Reset() => OwnValue.Reset();

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
