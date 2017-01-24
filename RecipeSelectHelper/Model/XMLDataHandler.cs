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
    public class XMLDataHandler
    {
        private string _filePath;

        public XMLDataHandler(string filepath = "data.xml")
        {
            _filePath = filepath;
        }

        public ProgramData FromXML()
        {
            var data = new ProgramData();
            var deserializer = new DataContractSerializer(data.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);

            var deserializedData = new ProgramData();

            try
            {
                using (var fs = new FileStream(_filePath, FileMode.Open))
                {
                    using (var xmlr = XmlReader.Create(fs))
                    {
                        deserializedData = (ProgramData)deserializer.ReadObject(xmlr, true);
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }

            return deserializedData;
        }

        //public void TestFromXML()
        //{
        //    var deserializer = new DataContractSerializer(AllRecipes.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
        //    // int to max value?
        //    List<Recipe> deserializedRecipes = new List<Recipe>();

        //    using (var fs = new FileStream(_fileName, FileMode.Open))
        //    {
        //        using (var xmlr = XmlReader.Create(fs))
        //        {
        //            deserializedRecipes = (List<Recipe>)deserializer.ReadObject(xmlr, true);
        //        }
        //    }

        //    string s = "Deserialized: ";
        //    foreach (Recipe recipe in deserializedRecipes)
        //    {
        //        s += recipe.Name + recipe.CategoriesAsString;
        //    }
        //    MessageBox.Show(s);
        //    AllRecipes.AddRange(deserializedRecipes);
        //}

        public void SaveToXML(ProgramData data)
        {
            var serializer = new DataContractSerializer(data.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            // int.MaxValue?
            using (var xmlw = XmlWriter.Create(_filePath))
            {
                serializer.WriteObject(xmlw, data);
            }


            //try
            //{
            //}
            //catch (InvalidDataContractException iExc)
            //{
            //    MessageBox.Show("You have an invalid data contract: " + iExc.Message);
            //}
            //catch (SerializationException sExc)
            //{
            //    MessageBox.Show("SerializationException: " + sExc.Message);
            //}
            MessageBox.Show("Serialized");



            //try
            //{
            //    var serializer = new DataContractSerializer(AllRecipes.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            //    // int.MaxValue?
            //    using (var xmlw = XmlWriter.Create(_fileName))
            //    {
            //        serializer.WriteObject(xmlw, AllRecipes);
            //    }
            //}
            //catch (InvalidDataContractException iExc)
            //{
            //    MessageBox.Show("You have an invalid data contract: " + iExc.Message);
            //}
            //catch (SerializationException sExc)
            //{
            //    MessageBox.Show("SerializationException: " + sExc.Message);
            //}
            //MessageBox.Show("Serialized");

        }
    }
}
