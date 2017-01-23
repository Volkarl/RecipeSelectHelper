using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "ProgramData")]
    public class ProgramData
    {
        [DataMember]
        public List<Product> AllProducts { get; set; }
        [DataMember]
        public List<BoughtProduct> AllBoughtProducts { get; set; }
        [DataMember]
        public List<Recipe> AllRecipes { get; set; }
        [DataMember]
        public List<ProductCategory> AllProductCategories { get; set; }
        [DataMember]
        public List<RecipeCategory> AllRecipeCategories { get; set; }

        [DataMember]
        public List<SortingMethod> AllSortingMethods { get; set; }
        // ?? How?

        private string _fileName = "data.xml";

        public ProgramData()
        {

        }

        public void FromXML()
        {
            AllRecipes = new List<Recipe> { new Recipe("Antipasta", categories: (new List<RecipeCategory> { new RecipeCategory("Tomatoes"), new RecipeCategory("Fish") })) };


        }

        public void TestFromXML()
        {
            // can it deserialize itself? That seems dumb?! Make the methods static (class methods)?

            var deserializer = new DataContractSerializer(AllRecipes.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            // int to max value?
            List<Recipe> deserializedRecipes = new List<Recipe>();

            using (var fs = new FileStream(_fileName, FileMode.Open))
            {
                using (var xmlr = XmlReader.Create(fs))
                {
                    deserializedRecipes = (List<Recipe>)deserializer.ReadObject(xmlr, true);
                }
            }

            string s = "Deserialized: ";
            foreach (Recipe recipe in deserializedRecipes)
            {
                s += recipe.Name + recipe.CategoriesAsString;
            }
            MessageBox.Show(s);
        }

        public void SaveToXML()
        {
            // XML THINGY TEST ATM

            try
            {
                var serializer = new DataContractSerializer(AllRecipes.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
                // int.MaxValue?
                using (var xmlw = XmlWriter.Create(_fileName))
                {
                    serializer.WriteObject(xmlw, AllRecipes);
                }
            }
            catch (InvalidDataContractException iExc)
            {
                MessageBox.Show("You have an invalid data contract: " + iExc.Message);
            }
            catch (SerializationException sExc)
            {
                MessageBox.Show("SerializationException: " + sExc.Message);
            }
            MessageBox.Show("Serialized");

        }
    }
}
