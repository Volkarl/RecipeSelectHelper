using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Xml;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Properties;

namespace RecipeSelectHelper.Resources
{
    public static class XmlDataHandler
    {
        public static ProgramData FromXml(string filePath)
        {
            var data = new ProgramData();
            var deserializer = new DataContractSerializer(data.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            var deserializedData = new ProgramData();

            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    using (var xmlr = XmlReader.Create(fs))
                    {
                        deserializedData = (ProgramData)deserializer.ReadObject(xmlr, true);
                    }
                }
            }
            catch (Exception ex)  // What happens if a file is only able to be read halfway through? Is a halfcompleted object returned?
            {
                MessageBox.Show(ex.Message);
                // throw; //Resharper tells me that if this exists then the new ProgramData is never used, which must mean that it either completes
                // the readthrough or returns an empty object.
            }

            return deserializedData;
        }

        public static ProgramData FromXmlString(string xmlData)
        {
            var data = new ProgramData();
            var deserializer = new DataContractSerializer(data.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            var deserializedData = new ProgramData();

            try
            {
                using (var sr = new StringReader(xmlData))
                {
                    using (var xmlr = XmlReader.Create(sr))
                    {
                        deserializedData = (ProgramData)deserializer.ReadObject(xmlr, true);
                    }
                }
            }
            catch (Exception ex)  // What happens if a file is only able to be read halfway through? Is a halfcompleted object returned?
            {
                MessageBox.Show(ex.Message);
                // throw; //Resharper tells me that if this exists then the new ProgramData is never used, which must mean that it either completes
                // the readthrough or returns an empty object.
            }

            return deserializedData;
        }

        public static void SaveToXml(string filePath, ProgramData data)
        {
            var serializer = new DataContractSerializer(data.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            // int.MaxValue?
            using (var xmlw = XmlWriter.Create(new Uri(filePath).LocalPath)) 
            {
                serializer.WriteObject(xmlw, data);
            }
        }

        public static string XmlAsString(ProgramData data)
        {
            var serializer = new DataContractSerializer(data.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            var sb = new StringBuilder();
            XmlWriterSettings indent = new XmlWriterSettings {Indent = true};
            using (var xmlw = XmlWriter.Create(sb, indent))
            {
                serializer.WriteObject(xmlw, data);
                xmlw.Flush();
                return sb.ToString();
            }
        }
    }
}
