namespace RecipeSelectHelper.Model.SortingMethods
{
    public interface ISortingMethod
    {
        string Name { get; set; }
        void Execute(ProgramData data);
    }
}