using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model.SortingMethods
{
    public class SubstitutesAllowedPreference : Preference
    {
        public override void Calculate(ProgramData pd)
        {
            // this needs to go last? Just prior to bubbling? It fills each ingredient's amount-to-value-counter with all boughtProducts, 
            // that are substitutes to the ingredient's correspondingProduct

            foreach (BoughtProduct bp in pd.AllBoughtProducts)
            {
                foreach (BoughtProduct sub in FindSubstitutesInFridge(bp.CorrespondingProduct, pd))
                {
                    bp.CorrespondingProduct.AddValueToCorrespondingIngredients(sub.OwnValue, sub); //Add ref to bp here,
                    // otherwise I can never sort out bp's that have been used multiple times (as multiple subs or, one ingredient
                    // and one or more subs.
                    throw new NotImplementedException();
                }
            }
        }

        private List<BoughtProduct> FindSubstitutesInFridge(Product p, ProgramData data)
        {
            List<BoughtProduct> bpSubs = new List<BoughtProduct>();
            foreach (Product sub in data.ProductSubstitutes.FindSubstitutes(p))
            {
                bpSubs.AddRange(data.AllBoughtProducts.Where(x => x.CorrespondingProduct == sub));
            }
            return bpSubs;
        }
    }
}
