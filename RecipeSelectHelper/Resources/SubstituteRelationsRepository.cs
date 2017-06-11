using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    // Deprecated because if we serialize the datatable (even if using the datacontractserializer, that the rest uses),
    // then all the entries are copied, and references are lost. This means that we can never use FindSubstitutes(product)
    // and actually find anything after deserialization. :( We now use SubstituteRelationsDictionary in stead, 
    // sadly that uses O(n^2) removal methods at the worst case, but I see no good alternative solution. 

    #region deprecatedSubstituteRelationsDataTable

    //[DataContract(Name = "SubstituteRelationsRepository")]
    //public class SubstituteRelationsRepository
    //{
    //    private static readonly string _productColumn = "Product";
    //    private static readonly string _substituteColumn = "Substitute";

    //    [DataMember]
    //    public DataTable SubstituteTable { get; set; }

    //    public SubstituteRelationsRepository()
    //    {
    //        SetupTable();
    //    }

    //    public void AddSubstitutes(Product product, List<Product> substitutes)
    //    {
    //        if (SubstituteTable == null) SetupTable();

    //        foreach (Product sub in substitutes)
    //        {
    //            SubstituteTable.Rows.Add(product, sub);
    //        }
    //    }

    //    private void SetupTable()
    //    {
    //        SubstituteTable = new DataTable("ProductSubstituteRelationsTable") { CaseSensitive = true };
    //        SubstituteTable.Columns.Add(_productColumn, typeof(Product));
    //        SubstituteTable.Columns.Add(_substituteColumn, typeof(Product));
    //    }

    //    public List<Product> FindSubstitutes(Product product)
    //    {
    //        List<Product> substituteList = new List<Product>();
    //        foreach (DataRow row in SubstituteTable.Rows)
    //        {

    //            // MAKE INTO LOOKUP IN STEAD OF THIS SHIT. CANT SERIALIZE FAK

    //            // All right, problem here is that whenever I serialize my DataTable, all the references are lost, which fucks up everything.

    //            //MessageBox.Show($"Product " + row.Field<Product>(_productColumn).Name + " contains sub" + row.Field<Product>(_substituteColumn).Name + (row.Field<Product>(_productColumn) == product).ToString());
    //            MessageBox.Show(row.Field<Product>(_productColumn).Equals(product).ToString());



    //            if (row.Field<Product>(_productColumn).Equals(product))
    //            {
    //                substituteList.Add(row.Field<Product>(_substituteColumn));
    //            }
    //        }
    //        return substituteList;
    //    }

    //    public void RemoveProduct(Product product)
    //    {
    //        // Removes all references to a product from the table
    //        List<int> rowsToRemove = new List<int>();
    //        for (int i = 0; i < SubstituteTable.Rows.Count; i++)
    //        {
    //            DataRow row = SubstituteTable.Rows[i];
    //            if (row.Field<Product>(_productColumn) == product ||
    //                row.Field<Product>(_substituteColumn) == product)
    //            {
    //                rowsToRemove.Add(i);
    //            }
    //        }

    //        foreach (int i in rowsToRemove)
    //        {
    //            SubstituteTable.Rows.RemoveAt(i);
    //        }
    //    }

    //    public void RemoveSubstituteFrom(Product product, Product substituteToRemove)
    //    {
    //        DataRow dr = SubstituteTable.AsEnumerable().SingleOrDefault(
    //            x => x.Field<Product>(_productColumn) == product && x.Field<Product>(_substituteColumn) == substituteToRemove);
    //        if (dr != null) SubstituteTable.Rows.Remove(dr);
    //    }
    //}


    #endregion

}
