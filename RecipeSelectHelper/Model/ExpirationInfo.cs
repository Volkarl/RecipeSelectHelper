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
            double now = (timeNow ?? DateTime.Now).Ticks;
            long diff = ProductExpirationTime.Value.Ticks - ProductCreatedTime.Value.Ticks;
            return diff / now;
        }
        // Make sure the above one works and doesn't lose the fraction
    }
}