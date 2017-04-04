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

        public double GetExpiredPercentage(DateTime? timeNow = null)
        {
            if (ProductCreatedTime == null || ProductExpirationTime == null)
            {
                return 0;
            }
            long now = (timeNow ?? DateTime.Now).Ticks;
            long pCreation = ProductCreatedTime.Value.Ticks;
            long diffNow = now - pCreation;
            long diffExpire = ProductExpirationTime.Value.Ticks - pCreation;
            return diffNow / (double)diffExpire;
        }
        // Untested outside of mathcad
    }
}