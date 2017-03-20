using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Properties;

namespace RecipeSelectHelper.Resources
{
    public class XmlDataHandler
    {
        private string _filePath;

        public XmlDataHandler(string filePath = "data.xml")
        {
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filePath);
        }

        public ProgramData FromXml()
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

        public void SaveToXml(ProgramData data)
        {
            var serializer = new DataContractSerializer(data.GetType(), null, 0x7FFF, false, true /*preserveObjectReferences*/, null);
            // int.MaxValue?
            using (var xmlw = XmlWriter.Create(_filePath))
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
