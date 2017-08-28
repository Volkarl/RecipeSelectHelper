using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources.ConcreteTypesForXaml
{
    public class FilterProductCategory : Boolable<ProductCategory>
    {
        //public Boolable<ProductCategory> Boolable { get; set; }
        //public FilterProductCategory(Boolable<ProductCategory> rcBoolable)
        //{
        //    if(rcBoolable == null) throw new ArgumentException();
        //    Boolable = rcBoolable;
        //}

        public FilterProductCategory(ProductCategory instance) : base(instance)
        {
        }
    }
}
