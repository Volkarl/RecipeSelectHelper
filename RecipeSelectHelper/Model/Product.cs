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

        public int OwnValue { get; set; } = 0;

        public event EventHandler<Tuple<int,uint>> IncreaseIngredientValue;

        public Product(string name, List<ProductCategory> categories = null, List<GroupedProductCategory> groupedCategories = null)
        {
            Name = name;
            Categories = categories ?? new List<ProductCategory>();
            GroupedCategories = groupedCategories ?? new List<GroupedProductCategory>();
        }

        public int Value => AggregateValue();
        private int AggregateValue()
        {
            int val = OwnValue;
            foreach (ProductCategory productCategory in Categories)
            {
                val += productCategory.OwnValue;
            }
            return val;
        }

        public void AddValueToCorrespondingIngredients(int valueForEntireAmount, uint amountOfProduct)
        {
            IncreaseIngredientValue?.Invoke(this, new Tuple<int, uint>(valueForEntireAmount, amountOfProduct)); 
        }

        public string ToString(SubstituteRelationsRepository subRepo)
        {
            string subsToString = "|Substitutes: \n";
            foreach (Product substitute in subRepo.FindSubstitutes(this))
            {
                subsToString += "  " + substitute.Name + "\n";
            }
            return ToString() + subsToString;
        }

        public override string ToString()
        {
            string str = "|Product: " + Name + "\n";
            str += "|Categories: \n";
            foreach (ProductCategory pc in Categories)
            {
                str += "  " + pc.Name + "\n";
            }
            return str;
        }

        public string CategoriesAsString
        {
            get
            {
                if (Categories == null || Categories.IsEmpty()) return String.Empty;
                return string.Join(", ", Categories.ConvertAll(x => x.Name));
            }
        }

        public string GroupedCategoriesToString
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
