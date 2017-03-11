using System;
using System.Runtime.Serialization;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "ExpirationInfo")]
    public class ExpirationInfo
    {
        [DataMember]
        public DateTime? ProductCreatedTime { get; set; }   
        [DataMember]
        public DateTime? ProductExpirationTime { get; set; }

        public ExpirationInfo(DateTime? createdTime = null, DateTime? expirationTime = null)
        {
            ProductCreatedTime = createdTime;
            ProductExpirationTime = expirationTime;
        }
    }
}