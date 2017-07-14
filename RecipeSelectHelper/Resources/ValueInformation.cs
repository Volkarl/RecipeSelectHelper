using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model.SortingMethods;

namespace RecipeSelectHelper.Resources
{
    public class ValueInformation
    {
        private List<Tuple<int, Preference>> _ownValue = new List<Tuple<int, Preference>>();
        public void AddValue(int value, Preference sender) => _ownValue.Add(new Tuple<int, Preference>(value, sender));
        public int GetValue => _ownValue.Sum(tuple => tuple.Item1); 
        public void Reset() => _ownValue = new List<Tuple<int, Preference>>();
    }
}
