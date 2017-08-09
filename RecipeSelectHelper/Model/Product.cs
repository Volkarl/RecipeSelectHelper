using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RecipeSelectHelper.Resources;

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
        public List<GroupedProductCategory> GroupedCategories { get; set; }

        private ValueInformation _ownValue = new ValueInformation();
        public ValueInformation OwnValue => _ownValue ?? (_ownValue = new ValueInformation()); //Needed for deserialization
        public void Reset() => OwnValue.Reset();

        public event EventHandler<AmountNeededValueCalculator> TransferValueToIngredients;

        private Product() { } // Needed for deserialization

        public Product(string name, List<ProductCategory> categories = null, List<GroupedProductCategory> groupedCategories = null)
        {
            Name = name;
            Categories = categories ?? new List<ProductCategory>();
            GroupedCategories = groupedCategories ?? new List<GroupedProductCategory>();
        }

        public int Value => AggregateValue();
        private int AggregateValue()
        {
            int val = OwnValue.GetValue;
            foreach (ProductCategory productCategory in Categories)
            {
                val += productCategory.OwnValue.GetValue;
            }
            return val;
        }

        public void TransferValueToCorrespondingIngredients(AmountNeededValueCalculator valueCalculator)
        {
            TransferValueToIngredients?.Invoke(this, valueCalculator); 
        }

        public override string ToString()
        {
            return $"-- Product --\n" +
                   $"| Product: {Name}\n" +
                   $"| Categories:\n" +
                   $"{string.Join(", ", Categories.ConvertAll(x => x.Name)).Indent()}";
        }

        public string CategoriesAsString => Categories.IsNullOrEmpty() ? string.Empty : string.Join(", ", Categories.ConvertAll(x => x.Name));

        public string GroupedCategoriesAsString
        {
            get
            {
                if (GroupedCategories == null || GroupedCategories.IsEmpty()) return String.Empty;
                List<List<Boolable<ProductCategory>>> gpcs = GroupedCategories.ConvertAll(x => x.GroupedPc);
                var gpcNames = new List<String>();
                foreach (List<Boolable<ProductCategory>> groupedPc in gpcs)
                {
                    foreach (Boolable<ProductCategory> boolablePc in groupedPc)
                    {
                        if(boolablePc.Bool) gpcNames.Add(boolablePc.Instance.Name);
                    }
                }
                return string.Join(", ", gpcNames);
            }
        }

        //public string SubstitutesToString
        //{
        //    get
        //    {
        //        if(SubstituteProducts == null || SubstituteProducts.IsEmpty()) return String.Empty;
        //        return string.Join(", ", SubstituteProducts.ConvertAll(x => x.Name));
        //    }
        //}
    }
}
