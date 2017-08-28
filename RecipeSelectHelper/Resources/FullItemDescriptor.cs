using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;

namespace RecipeSelectHelper.Resources
{
    public static class FullItemDescriptor
    {
        public static string GetDescription(Recipe r)
        {
            return $"Name: {r.Name}\n" +
                   $"Servings: {r.Servings}\n" +
                   StringIfContainsAnyMembers(r.Categories, $"Categories: {r.CategoriesAsString}\n") +
                   StringIfContainsAnyMembers(r.GroupedCategories, $"Types: {r.GroupedCategoriesAsString}\n") +
                   $"Description: {r.Description}\n" +
                   StringIfContainsAnyMembers(r.Instructions.ToList(), $"{GetDescription(r.Instructions)}\n") +
                   StringIfContainsAnyMembers(r.Ingredients, $"Ingredients: {string.Join(",\n", r.Ingredients.ConvertAll(GetDescription))}\n");
        }

        private static string GetDescription(StringList instructions)
        {
            if (instructions == null) return null;
            int i = 0;
            const string indent = "    ";
            return instructions.Aggregate("Instructions: \n", (current, instruction) => current + $"{indent}{i++}. {instruction}\n");
            // Same in non-linq below:
            //string str = "Instructions: \n";
            //foreach (string instruction in instructions)
            //{
            //    str += $"{indent}{i++}. {instruction}\n";
            //}
            //return str;
        }

        private static string GetDescription(Ingredient i)
        {
            return $"{i.CorrespondingProduct.Name} ({i.AmountNeeded} needed)";
        }

        private static string StringIfContainsAnyMembers<T>(List<T> collection, string stringToDisplay)
        {
            return collection.IsNullOrEmpty() ? null : stringToDisplay;
        }

        public static string GetDescription(Product p, SubstituteRelationsDictionary substituteRepository = null)
        {
            List<Product> subs = substituteRepository?.FindSubstitutes(p);
            return $"{p.Name}\n" +
                   StringIfContainsAnyMembers(p.Categories, $"Categories: {p.CategoriesAsString}\n") +
                   StringIfContainsAnyMembers(p.GroupedCategories, $"Types: {p.GroupedCategoriesAsString}\n") +
                   (subs == null ? null : StringIfContainsAnyMembers(subs, $"Substitutes: {string.Join(", ", subs.ConvertAll(s => s.Name))}\n"));
        }

        public static string GetDescription(BoughtProduct bp)
        {
            return $"{bp.CorrespondingProduct.Name}\n" +
                   $"Amount: {bp.Amount}\n" +
                   $"{(bp.ExpirationData == null ? null : GetDescription(bp.ExpirationData))}\n";
        }

        public static string GetDescription(ExpirationInfo exp)
        {
            return $"{(exp.ProductCreatedTime == null ? null : $"Created date: {exp.ProductCreatedTime}\n")}" +
                   $"{(exp.ProductExpirationTime == null ? null : $"Expiration date: {exp.ProductExpirationTime}\n")}";
        }

        public static string GetDescription<T>(GroupedSelection<T> gpc)
        {
            return $"Select {gpc.MinSelect} to {gpc.MaxSelect}: {string.Join(", ", gpc.GroupedItems)}";
        }
    }
}
