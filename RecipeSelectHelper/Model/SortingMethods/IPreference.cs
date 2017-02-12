namespace RecipeSelectHelper.Model.SortingMethods
{
    public interface IPreference
    {
        string Description { get; set; }
        void Calculate(ProgramData data);
    }
}