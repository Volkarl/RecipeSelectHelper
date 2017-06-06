using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class AmountNeededValueCalculator
    {
        public List<BpValue> OrderedBpValues { get; }

        public AmountNeededValueCalculator(Product correspondingProduct)
        {
            CorrespondingProduct = correspondingProduct;
        }

        public Product CorrespondingProduct { get; }

        public int GetOptimalValueCombination(Dictionary<BoughtProduct, uint> bpAmountsRemaining)
        {
            // The idea is that we use up the front (most value per amount) BoughtProducts first
            throw new NotImplementedException();
        }

        public void AddBoughtProduct(BoughtProduct bp)
        {
            throw new NotImplementedException();
        }
    }
}
