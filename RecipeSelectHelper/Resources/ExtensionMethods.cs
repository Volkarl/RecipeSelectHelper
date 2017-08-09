using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public static class ExtensionMethods
    {
        #region NotWorkingCombinePredicates
        //public static Predicate<T> Add<T>(this Predicate<T> predicate1, Predicate<T> predicate2)
        //{
        //    return x => predicate1(x) && predicate2(x);
        //} // Why wont this work?
        #endregion

        public static List<T> CombineElements<T>(this List<T> allElements, Predicate<T> finder,
            Func<List<T>, T> combiner)
        {
            List<T> matchingElements = allElements.FindAll(finder);
            T combinedElement = combiner(matchingElements);
            List<T> newElementList = allElements.RemoveElements(matchingElements);
            newElementList.Add(combinedElement);
            return newElementList;
        }

        public static string GetLastSubstring(this string str, char splitAtChar)
        {
            int index = str.LastIndexOf(splitAtChar);
            return str.Substring(index + 1);
        }

        public static List<BoughtProduct> GetExpiredProducts(this IEnumerable<BoughtProduct> boughtProducts)
        {
            var expiredProducts = new List<BoughtProduct>();
            foreach (BoughtProduct bp in boughtProducts)
            {
                if (bp.ExpirationData.ProductExpirationTime.HasValue)
                {
                    if (DateTime.Now > bp.ExpirationData.ProductExpirationTime.Value)
                    {
                        expiredProducts.Add(bp);
                    }
                }
            }
            return expiredProducts;
        }

        public static List<ProductCategory> GetCheckedGroupedCategories(this Product product)
        {
            var s = new List<ProductCategory>();
            foreach (GroupedProductCategory gpc in product.GroupedCategories)
            {
                foreach (Boolable<ProductCategory> pcBoolable in gpc.GroupedPc)
                {
                    if (pcBoolable.Bool) s.Add(pcBoolable.Instance);
                }
            }
            return s;
        }

        public static List<RecipeCategory> GetCheckedGroupedCategories(this Recipe recipe)
        {
            var s = new List<RecipeCategory>();
            foreach (GroupedRecipeCategory grc in recipe.GroupedCategories)
            {
                foreach (Boolable<RecipeCategory> rcBoolable in grc.GroupedRc)
                {
                    if (rcBoolable.Bool) s.Add(rcBoolable.Instance);
                }
            }
            return s;
        }

        public static List<RecipeCategory> GetSelected(this ObservableCollection<FilterGroupedRecipeCategories> filterGrc)
        {
            var selected = new List<RecipeCategory>();
            foreach (FilterGroupedRecipeCategories gpc in filterGrc)
            {
                selected.AddRange(gpc.GetCheckedCategories());
            }
            return selected;
        }

        public static List<RecipeCategory> GetSelected(this ObservableCollection<FilterRecipeCategory> filterRc)
        {
            var selected = new List<RecipeCategory>();
            foreach (FilterRecipeCategory pc in filterRc)
            {
                if (pc.Bool) selected.Add(pc.Instance);
            }
            return selected;
        }

        public static List<ProductCategory> GetSelected(this ObservableCollection<FilterGroupedProductCategories> filterGpc)
        {
            var selected = new List<ProductCategory>();
            foreach (FilterGroupedProductCategories gpc in filterGpc)
            {
                selected.AddRange(gpc.GetCheckedCategories());
            }
            return selected;
        }

        public static List<ProductCategory> GetSelected(this ObservableCollection<FilterProductCategory> filterPc)
        {
            var selected = new List<ProductCategory>();
            foreach (FilterProductCategory pc in filterPc)
            {
                if (pc.Bool) selected.Add(pc.Instance);
            }
            return selected;
        }

        public static List<T> RemoveElements<T>(this List<T> collection, List<T> itemsToRemove)
        {
            foreach (T element in itemsToRemove)
            {
                collection.Remove(element);
            }
            return collection;
        }

        public static void MoveElement<T>(this List<T> collection, int index, int newIndex)
        {
            T item = collection[index];
            collection.RemoveAt(index);
            collection.Insert(newIndex, item);
        }

        public static int LastIndex<T>(this IEnumerable<T> collection)
        {
            return collection.Count() - 1;
        }

        public static int ToInt(this char c)
        {
            return (int)(c - '0');
        }

        public static bool ContainsCaseInsensitive(this string str1, string str2) => str1.ToLower().Contains(str2.ToLower());

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection.IsEmpty() || collection == null;
        }

        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return !collection.Any();
        }

        public static bool ContainsAll<T>(this ICollection<T> collection1, ICollection<T> collection2)
        {
            if (collection1.Count < collection2.Count) return false;
            return !collection2.Except(collection1).Any();
        }

        public static List<T> ToSingleItemList<T>(this T obj) 
        {
            return new List<T> {obj};
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

            throw new KeyNotFoundException("Enum OwnValue not found.");
            // return (T)Enum.Parse(typeof(T), description);
        }

        public static string Indent(this string textToIndent, int indentAmount = 4)
        {
            var indent = new string(' ', indentAmount);
            return indent + textToIndent.Replace("\n", "\n" + indent);
        }

        public static T1 MaxItem<T1,T2>(this IEnumerable<T1> source, Func<T1,T2> valueToCompare)
        {
            IEnumerable<T1> enumerable = source as T1[] ?? source.ToArray();
            T2 maxValue = enumerable.Max(valueToCompare);
            return enumerable.First(x => valueToCompare(x).Equals(maxValue));
        }
    }
}
