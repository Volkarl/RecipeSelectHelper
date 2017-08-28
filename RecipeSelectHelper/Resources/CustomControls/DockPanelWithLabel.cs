using System.Windows;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources.CustomControls
{
    public class DockPanelWithLabel : DockPanel
    {
        public UIElement SecondElement { get; private set; }
        public DockPanelWithLabel(object labelContents, UIElement secondElement)
        {
            SecondElement = secondElement;
            Children.Add(new Label {Content = labelContents, Height = 30, Width = 200});
            Children.Add(secondElement);
        }
    }
}
