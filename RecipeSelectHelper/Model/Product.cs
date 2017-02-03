using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "Product")]
    public class Product : IProduct
    {
        [DataMember]
        public List<ProductCategory> Categories { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<Product> SubstituteProducts { get; set; }
        [DataMember]
        public int Value { get; set; } = 0;

        private static int _productCreatedNumber = 0;
        // Counter starts at 0 at every new program execution, it needs to get this from settings.settings? 

        public Product(string name, List<ProductCategory> categories = null, List<Product> substituteProducts = null)
        {
            Name = name;
            Categories = categories ?? new List<ProductCategory>();
            SubstituteProducts = substituteProducts ?? new List<Product>();
            ID = _productCreatedNumber++;
        }
    }
}
