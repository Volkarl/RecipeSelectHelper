using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public static class UtilityMethods
    {
        public static string AddDefaultFileName(string directoryPath)
        {
            return Path.Combine(directoryPath, "data.xml");
        }

        public static string GetExePath()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
        }

        public static string GetExeDirectoryPath()
        {
            return System.IO.Path.GetDirectoryName(GetExePath());
        }

        public static bool DirectoryPathIsValid(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    string temp = Path.Combine(path, Path.GetRandomFileName());
                    File.Create(temp).Dispose();
                    File.Delete(temp);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            return false;
        }

        public static bool IsListWithoutElements(object value)
        {
            var list = value as IList;
            return list?.Count == 0;
        }
    }
}
