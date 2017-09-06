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

        private ExpirationInfo() { }

        public ExpirationInfo(DateTime? createdTime = null, DateTime? expirationTime = null)
        {
            ProductCreatedTime = createdTime;
            ProductExpirationTime = expirationTime;
        }

        public bool HasValue => ProductCreatedTime.HasValue && ProductExpirationTime.HasValue;

        public double GetExpiredPercentage(DateTime? timeNow = null)
        {
            if (ProductCreatedTime == null || ProductExpirationTime == null) return 0;
            if (ProductCreatedTime == ProductExpirationTime) return 1;
            long now = (timeNow ?? DateTime.Now).Ticks;
            long pCreation = ProductCreatedTime.Value.Ticks;
            long diffNow = now - pCreation;
            long diffExpire = ProductExpirationTime.Value.Ticks - pCreation;
            return diffNow / (double)diffExpire;
        }
    }
}