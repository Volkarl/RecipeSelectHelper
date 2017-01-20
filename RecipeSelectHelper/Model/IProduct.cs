using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    public interface IProduct
    {
        string Name { get; set; }
        int Value { get; set; }
        int BonusValue { get; set; }
        int ProductID { get; }
        List<ICategory> Categories { get; set; }
        FridgeInfo inFridgeInfo { get; set; }
    }
}
