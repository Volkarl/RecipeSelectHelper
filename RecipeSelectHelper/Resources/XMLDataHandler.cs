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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return deserializedData;
        }

        public static void SaveToXml(string filePath, ProgramData data)
        {
            var serializer = new DataContractSerializer(data.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            // int.MaxValue?
            using (var xmlw = XmlWriter.Create(filePath))
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
