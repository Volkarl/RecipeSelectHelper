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
        public Type Type { get; }
        public object Aggregator { get; }

        private AggregatedValue(Type type, object o)
        {
            Type = type;
            Aggregator = o;
        }

        public AggregatedValue(ProductCategory pc) : this(pc.GetType(), pc) { }
        public AggregatedValue(RecipeCategory rc) : this(rc.GetType(), rc) { }
        public AggregatedValue(Product p) : this(p.GetType(), p) { }
        public AggregatedValue(BoughtProduct bp) : this(bp.GetType(), bp) { }
        public AggregatedValue(Ingredient i) : this(i.GetType(), i) { }
        public AggregatedValue(Recipe r) : this(r.GetType(), r) { }
    }
}
