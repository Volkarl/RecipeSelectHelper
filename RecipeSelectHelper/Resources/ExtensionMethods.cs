using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Resources
{
    public static class ExtensionMethods
    {
        public static bool ContainsCaseInsensitive(this string str1, string str2) => str1.ToLower().Contains(str2.ToLower());

        public static bool ContainsAll<T>(this ICollection<T> collection1, ICollection<T> collection2)
        {
            if (collection1.Count < collection2.Count) return false;
            return !collection1.Except(collection2).Any();
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            MemberInfo[] fis = typeof(T).GetFields();

            foreach (var fi in fis)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0 && attributes[0].Description == description)
                    return (T)Enum.Parse(typeof(T), fi.Name);
            }

            throw new KeyNotFoundException("Enum Value not found.");
            // return (T)Enum.Parse(typeof(T), description);
        }
    }
}
