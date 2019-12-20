using CompAnalytics.X9.Records;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.Document
{
    [DataContract]
    [Serializable]
    public class X9DepositItemImage : X9DocumentComponent
    {
        [DataMember]

        public ImageViewDetailRecord ImageViewDetail { get; set; }
        [DataMember]
        public ImageViewDataRecord ImageViewData { get; set; }

        public override List<X9Record> GetRecords()
        {
            return new List<X9Record>
            {
                this.ImageViewData,
                this.ImageViewDetail
            };
        }
    }
}
