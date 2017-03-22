using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Resources
{
    public static class UtilityMethods
    {
        public static string GetExePath()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
        }

        public static string GetExeDirectoryPath()
        {
            return System.IO.Path.GetDirectoryName(GetExePath());
        }

        public static bool PathIsValid(string path)
        {
            throw new NotImplementedException();
        }
    }
}
