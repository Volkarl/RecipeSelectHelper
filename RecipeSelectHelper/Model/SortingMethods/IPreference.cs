namespace RecipeSelectHelper.Model.SortingMethods
{
    public interface IPreference
    {
        string Description { get; }
        void Calculate(ProgramData data);
    }
}