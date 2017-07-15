using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Model.SortingMethods;

namespace RecipeSelectHelper.Resources
{
    public class ValueInformation
    {
        private List<Tuple<int, Preference>> _preferenceValues = new List<Tuple<int, Preference>>();
        private List<Tuple<int, AggregatedValue>> _aggregationValues = new List<Tuple<int, AggregatedValue>>();
        public void AddValue(int value, Preference sender) => _preferenceValues.Add(new Tuple<int, Preference>(value, sender));
        public void AddValue(int value, AggregatedValue sender) => _aggregationValues.Add(new Tuple<int, AggregatedValue>(value, sender)); // I need an overload like this, otherwise I cant aggregate values! Perhaps make a sender-class that can hold both preferences, ingredients, product categories and all of that jazz.
        public int GetValue => _preferenceValues.Sum(tuple => tuple.Item1) + _aggregationValues.Sum(tuple => tuple.Item1); 
        public void Reset()
        {
            _preferenceValues = new List<Tuple<int, Preference>>();
            _aggregationValues = new List<Tuple<int, AggregatedValue>>();
        }
        
        public List<Tuple<int, string>> GetSenders
        {
            get
            {
                List<Tuple<int, string>> combinedSenderList = new List<Tuple<int, string>>();
                combinedSenderList.AddRange(_preferenceValues.ConvertAll(x => new Tuple<int, string>(x.Item1, x.Item2.ToString())));
                combinedSenderList.AddRange(_aggregationValues.ConvertAll(x => new Tuple<int, string>(x.Item1, x.Item2.ToString())));
                return combinedSenderList;
            }
        }
    }
}
