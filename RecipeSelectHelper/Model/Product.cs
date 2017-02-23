using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "Product")]
    public class Product : IProduct
    {
        [DataMember]
        public List<ProductCategory> Categories { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<Product> SubstituteProducts { get; set; }

        public int OwnValue { get; set; } = 0; //Should this be set-only? //Aggregated value get only then.

        public Product(string name, List<ProductCategory> categories = null, List<Product> substituteProducts = null)
        {
            Name = name;
            Categories = categories ?? new List<ProductCategory>();
            SubstituteProducts = substituteProducts ?? new List<Product>();
            //ID = _productCreatedNumber++;
        }

        public int AggregatedValue => CalculateValue();
        private int CalculateValue()                             // I COULD ADD SOMETHING ABOUT SUBSTITUTES HERE?
        {
            int val = OwnValue;
            foreach (ProductCategory productCategory in Categories)
            {
                val += productCategory.Value;
            }
            return val;
        }

        public override string ToString()
        {
            string str = "|Product: " + Name + "\n";
            str += "|Categories: \n";
            foreach (ProductCategory pc in Categories)
            {
                str += "  " + pc.Name + "\n";
            }
            str += "|Substitutes: \n";
            foreach (Product substitute in SubstituteProducts)
            {
                str += "  " + substitute.Name + "\n";
            }
            return str;
        }
    }
}
