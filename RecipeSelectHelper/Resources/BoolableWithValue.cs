using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Resources
{
    public class BoolableWithValue<T1, T2> : Boolable<T1>
    {
        public T2 Value { get; set; }
        public BoolableWithValue(T1 instance, bool startValue = false, T2 value = default(T2)) : base(instance, startValue)
        {
            Value = value;
        }
    }
}
