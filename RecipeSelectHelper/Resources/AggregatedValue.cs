using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class AggregatedValue
    {
        public object Aggregator { get; }

        private AggregatedValue(object o)
        {
            Aggregator = o;
        }

        public string GetString()
        {
            //var ingredient = Aggregator as Ingredient;
            //if (ingredient != null) return ingredient.CorrespondingProduct.Name;

            return Aggregator.ToString().GetLastSubstring('.'); // Get the last part of the full type definition
        }

        //public AggregatedValue(ProductCategory pc) : this(pc.GetType(), pc) { }
        //public AggregatedValue(RecipeCategory rc) : this(rc.GetType(), rc) { }
        //public AggregatedValue(Product p) : this(p.GetType(), p) { }
        //public AggregatedValue(BoughtProduct bp) : this(bp.GetType(), bp) { }
        public AggregatedValue(Ingredient i) : this((object) i) { }
        //public AggregatedValue(Recipe r) : this(r.GetType(), r) { }
    }
}
