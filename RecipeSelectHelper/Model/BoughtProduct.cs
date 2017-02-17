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

        public int Value { get; set; } = 0;

        public BoughtProduct(Product correspondingProduct, ExpirationInfo expirationData)
        {
            CorrespondingProduct = correspondingProduct;
            ExpirationData = expirationData;
        }
    }
}