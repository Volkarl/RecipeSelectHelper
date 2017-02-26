using System;
using System.Runtime.Serialization;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "ExpirationInfo")]
    public class ExpirationInfo
    {
        [DataMember]
        public DateTime? ProductCreatedTime { get; set; }   //how is this going to work and how do I make it as easy as possible for the user?
        // A slider maybe? In stead of selecting the exact date. We dont really need to be perfectly precise, do we?

        [DataMember]
        public DateTime? ProductExpirationTime { get; set; }

        public ExpirationInfo(DateTime? createdTime = null, DateTime? expirationTime = null)
        {
            ProductCreatedTime = createdTime;
            ProductExpirationTime = expirationTime;
        }
    }
}