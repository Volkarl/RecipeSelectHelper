using System.Windows.Controls;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.View.Miscellaneous
{
    public interface IParentPage
    {
        ProgramData Data { get; }

        void SaveChanges();
        void Shutdown(bool saveChanges = true);
        void SetRootPage(Page newpage, Button highlightSpecificButton = null);
        void NavigatePageBack();
        void SetPage(Page newpage, string title = null, bool addNavigationEvent = true);
        void SetPage(AddElementBasePage newpage);
    }
}