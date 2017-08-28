using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecipeSelectHelper.Resources.ConcreteTypesForXaml
{
    [CollectionDataContract(Name = "StringList")]
    public class StringList : ObservableCollection<string>
    {
        private StringList() { } // Needed for deserialization

        public StringList(params string[] strings) : this(new ObservableCollection<string>(strings)) { }

        public StringList(IEnumerable<string> strings = null) : base(strings ?? new ObservableCollection<string>()) { }

        public enum MoveDirection
        {
            Up, Down
        }

        public void MoveInDirection(string selectedElement, MoveDirection direction)
        {
            MoveInDirection(IndexOf(selectedElement), direction);
        }

        public void MoveInDirection(int index, MoveDirection direction)
        {
            if (direction == MoveDirection.Up)
            {
                if(index > 0) this.MoveElement(index, index - 1);
            }
            else
            {
                if(index < this.LastIndex()) this.MoveElement(index, index + 1);
            }
        }
    }
}
