using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecipeSelectHelper.Resources.ConcreteTypesForXaml
{
    public class StringList : List<string>
    {
        public StringList(params string[] strings) : this(new List<string>(strings)) { }

        public StringList(List<string> strings = null) : base(strings ?? new List<string>()) { }

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
