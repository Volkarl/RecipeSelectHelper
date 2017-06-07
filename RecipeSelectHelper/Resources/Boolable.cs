using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Resources
{
    [DataContract(Name = "Boolable")]
    public class Boolable<T>
    {
        [DataMember]
        public T Instance { get; set; }
        [DataMember]
        public bool Bool { get; set; }

        private Boolable() { }
        public Boolable(T instance, bool startValue = false)
        {
            Bool = startValue;
            Instance = instance;
        }
    }
}
