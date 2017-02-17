using System;
using System.Runtime.Serialization;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "BoughtProduct")]
    public class BoughtProduct : IBoughtProduct
    {
        [DataMember]
        public Product CorrespondingProduct { get; set; }
        [DataMember]
        public ExpirationInfo ExpirationData { get; set; }
//        [DataMember]
//        public int ID { get; set; }

        public int Value { get; set; } = 0;

        //private static int _boughtProductCreatedNumber = 0;
        // Counter starts at 0 at every new program execution, it needs to get this from settings.settings? 

        public BoughtProduct(Product correspondingProduct, ExpirationInfo expirationData)
        {
            CorrespondingProduct = correspondingProduct;
            ExpirationData = expirationData;
 //           Value = _boughtProductCreatedNumber++;
        }
    }
}