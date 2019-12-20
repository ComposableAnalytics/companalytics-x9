using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CompAnalytics.X9.Records;

namespace CompAnalytics.X9.Document
{
    [DataContract]
    [Serializable]
    public class X9DepositItem : X9DocumentComponent
    {
        [DataMember]
        public CheckDetailRecord CheckDetail { get; set; }
        [DataMember]
        public CheckDetailAddendumRecord CheckDetailAddendum { get; set; }
        [DataMember]
        public List<X9DepositItemImage> Images = new List<X9DepositItemImage>();

        public override List<X9Record> GetRecords()
        {
            var recs = new List<X9Record>
            {
                this.CheckDetail,
                this.CheckDetailAddendum
            };
            recs.AddRange(this.Images.Cast<X9Record>());
            return recs;
        }
    }
}
