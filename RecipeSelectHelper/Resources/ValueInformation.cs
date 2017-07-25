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

        public void AddValue(int value, Preference sender) => _preferenceValues.Add(
            new Tuple<int, Preference>(value, sender));

        public void AddValue(int value, AggregatedValue sender) =>
            _aggregationValues.Add(new Tuple<int, AggregatedValue>(value, sender));

        public int GetValue => _preferenceValues.Sum(tuple => tuple.Item1) +
                               _aggregationValues.Sum(tuple => tuple.Item1);

        public void Reset()
        {
            _preferenceValues = new List<Tuple<int, Preference>>();
            _aggregationValues = new List<Tuple<int, AggregatedValue>>();
        }

        public List<ProgressInfo> GetSenders
        {
            get
            {
                List<ProgressInfo> combinedSenderList = new List<ProgressInfo>();
                int totalValue = GetValue;
                combinedSenderList.AddRange(
                    _preferenceValues.ConvertAll(x => new ProgressInfo(x.Item1, totalValue, x.Item2.Description)));
                combinedSenderList.AddRange(_aggregationValues.ConvertAll(x => new ProgressInfo(x.Item1, totalValue, 
                    $"Aggregated {x.Item1} from {x.Item2.Aggregator.ToString().GetLastSubstring('.')}")));
                return combinedSenderList;
            }
        }
    }
}
