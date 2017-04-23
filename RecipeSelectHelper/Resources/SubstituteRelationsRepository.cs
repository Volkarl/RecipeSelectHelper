using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    [DataContract(Name = "SubstituteRelationsRepository")]
    public class SubstituteRelationsRepository
    {
        private string _productColumn = "Product";
        private string _substituteColumn = "Substitute";

        [DataMember]
        public DataTable SubstituteTable { get; }

        public SubstituteRelationsRepository()
        {
            SubstituteTable = new DataTable();
            SubstituteTable.Columns.Add(_productColumn, typeof(Product));
            SubstituteTable.Columns.Add(_substituteColumn, typeof(Product));
        }

        public void AddSubstitutes(Product product, List<Product> substitutes)
        {
            foreach (Product substitute in substitutes)
            {
                SubstituteTable.Rows.Add(product, substitute);
            }
        }

        public List<Product> FindSubstitutes(Product product)
        {
            List<Product> substituteList = new List<Product>();
            foreach (DataRow row in SubstituteTable.Rows)
            {
                if (row.Field<Product>(_productColumn) == product)
                {
                    substituteList.Add(row.Field<Product>(_substituteColumn));
                }
            }
            return substituteList;
        }

        public void RemoveProduct(Product product)
        {
            // Removes all references to a product from the table
            List<int> rowsToRemove = new List<int>();
            for (int i = 0; i < SubstituteTable.Rows.Count; i++)
            {
                DataRow row = SubstituteTable.Rows[i];
                if (row.Field<Product>(_productColumn) == product ||
                    row.Field<Product>(_substituteColumn) == product)
                {
                    rowsToRemove.Add(i);
                }
            }

            foreach (int i in rowsToRemove)
            {
                SubstituteTable.Rows.RemoveAt(i);
            }
        }

        public void RemoveSubstituteFrom(Product product, Product substituteToRemove)
        {
            DataRow dr = SubstituteTable.AsEnumerable().SingleOrDefault(
                x => x.Field<Product>(_productColumn) == product && x.Field<Product>(_substituteColumn) == substituteToRemove);
            if (dr != null) SubstituteTable.Rows.Remove(dr);
        }
    }
}
