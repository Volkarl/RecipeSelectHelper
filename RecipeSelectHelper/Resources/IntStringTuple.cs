using System;

namespace RecipeSelectHelper.Resources
{
    public class IntStringTuple : Tuple<int, string>
    {
        public int TotalValue { get; }

        public IntStringTuple(int integer, string text, int totalValue) : base(integer, text)
        {
            TotalValue = totalValue;
        }

        // TODO RENAME THIS
    }
}
