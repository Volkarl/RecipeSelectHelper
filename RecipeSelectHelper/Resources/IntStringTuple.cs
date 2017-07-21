using System;

namespace RecipeSelectHelper.Resources
{
    public class IntStringTuple : Tuple<int, string>
    {
        private readonly int _totalValue;

        public int ProgressMaximum => _totalValue == 0 && Item1 == 0 ? 1 : 0;
        // To avoid having fully filled progress bars whenever our values are 0/0

        public IntStringTuple(int integer, string text, int totalValue) : base(integer, text)
        {
            if(integer < 0 || totalValue < 0) throw new ArgumentException(integer < 0 ? nameof(integer) : nameof(totalValue));    
            _totalValue = totalValue;
        }

        // TODO RENAME THIS progressbarsomething
    }
}
