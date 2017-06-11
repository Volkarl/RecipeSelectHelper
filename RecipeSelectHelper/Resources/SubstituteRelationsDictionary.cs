using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    [DataContract(Name = "SubstituteRelationsDictionary")]
    public class SubstituteRelationsDictionary
    {
        [DataMember]
        public Dictionary<Product, List<Product>> SubstituteDictionary { get; set; }

        public List<Product> FindSubstitutes(Product product)
        {
            List<Product> substitutes = null;
            if (product != null)
                SubstituteDictionary?.TryGetValue(product, out substitutes);
            return substitutes ?? new List<Product>();
        }

        public void AddSubstitutes(Product product, List<Product> substitutes)
        {
            if (SubstituteDictionary == null) SubstituteDictionary = new Dictionary<Product, List<Product>>();

            if(product == null) throw new ArgumentException();
            substitutes = substitutes?.Distinct().ToList() ?? new List<Product>();

            if(!SubstituteDictionary.ContainsKey(product))
                SubstituteDictionary.Add(product, substitutes);
            else
                SubstituteDictionary[product].AddRange(substitutes);
        }

        public void RemoveProduct(Product product)
        {
            if (product != null) SubstituteDictionary.Remove(product);
            foreach (List<Product> substitutes in SubstituteDictionary.Values)
            {
                substitutes.RemoveAll(x => x == product);
            }
        }

        public void RemoveSubstituteFrom(Product product, Product substituteToRemove)
        {
            List<Product> substitutes;
            if (SubstituteDictionary.TryGetValue(product, out substitutes))
                substitutes.Remove(substituteToRemove);
        }
    }
}
