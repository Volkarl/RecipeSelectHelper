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
        [DataMember]
        public uint Amount { get; set; }

        public int OwnValue { get; set; } = 0;

        public BoughtProduct(Product correspondingProduct, uint amount, ExpirationInfo expirationData = null)
        {
            if(correspondingProduct == null) throw new ArgumentException("No product selected");
            CorrespondingProduct = correspondingProduct;
            Amount = amount;
            ExpirationData = expirationData;
        }
    }
}