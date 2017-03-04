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
        [DataMember]
        public List<GroupedProductCategory> GroupedCategories { get; set; }

        public int OwnValue { get; set; } = 0; 

        public Product(string name, List<ProductCategory> categories = null, List<Product> substituteProducts = null, List<GroupedProductCategory> groupedCategories = null)
        {
            Name = name;
            Categories = categories ?? new List<ProductCategory>();
            SubstituteProducts = substituteProducts ?? new List<Product>();
            GroupedCategories = groupedCategories ?? new List<GroupedProductCategory>();
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

        public string CategoriesAsString
        {
            get
            {
                if (Categories == null)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Join(", ", Categories.ConvertAll(x => x.Name));
                }
            }
        }
    }
}
