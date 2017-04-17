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
                foreach (BoughtProduct sub in FindSubstitutesInFridge(pd))
                {
                    bp.CorrespondingProduct.AddValueToCorrespondingIngredients(sub.OwnValue, sub.Amount);
                }
            }
            throw new NotImplementedException();
        }

        private List<BoughtProduct> FindSubstitutesInFridge(ProgramData pd)
        {

            throw new NotImplementedException();
        }
    }
}
