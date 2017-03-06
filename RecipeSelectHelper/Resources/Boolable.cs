using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Resources
{
    public class Boolable<T>
    {
        public T Instance { get; set; }
        public bool Bool { get; set; }

        public Boolable(T instance, bool startValue = false)
        {
            Bool = startValue;
            Instance = instance;
        }
    }
}
