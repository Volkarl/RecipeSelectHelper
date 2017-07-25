using System;
using System.ComponentModel;

namespace RecipeSelectHelper.Resources
{
    public class ProgressInfo
    {
        public int Value { get; }
        public string Description { get; }
        public int Maximum { get; }

        /// <summary>
        /// Use to avoid dividing by zero.
        /// </summary>
        public int MaximumNeverZero => Maximum == 0 ? 1 : Maximum;
        // public int MaximumNeverZero => _totalValue == 0 && Value == 0 ? 1 : _totalValue;
        // To avoid having fully filled progress bars whenever our values are 0/0
        
        public int PercentageCompletion => (Value / MaximumNeverZero) * 100;

        public ProgressInfo(int value, int maxValue, string description = null)
        {
            if (value < 0 || maxValue < 0)
                throw new ArgumentOutOfRangeException("Value cannot be less than zero: " + (value < 0 ? nameof(value) : nameof(maxValue)));
            Maximum = maxValue;
            Value = value;
            Description = description;
        }
    }
}
