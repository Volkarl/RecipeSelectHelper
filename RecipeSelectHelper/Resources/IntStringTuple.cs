using System;

namespace RecipeSelectHelper.Resources
{
    public class IntStringTuple : Tuple<int, string>
    {
        private readonly int _totalValue;
        public int TotalValue => _totalValue;
        // This needs to be able to show 0/0
        //todo rename to something like exactMaximum

        public int ProgressMaximum => _totalValue == 0 && Item1 == 0 ? 1 : _totalValue;
        // To avoid having fully filled progress bars whenever our values are 0/0
        // todo rename to something like progressBarMaximum


        public int PercentageCompletion => (Item1 / ProgressMaximum) * 100;

        public IntStringTuple(int integer, string text, int totalValue) : base(integer, text)
        {
            if(integer < 0 || totalValue < 0) throw new ArgumentException(integer < 0 ? nameof(integer) : nameof(totalValue));    
            _totalValue = totalValue;
        }

        // TODO RENAME THIS progressbarsomething
    }
}
