using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources
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
