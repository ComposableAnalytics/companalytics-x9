using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CompAnalytics.X9.Records;

namespace CompAnalytics.X9.Document
{
    [DataContract]
    [Serializable]
    public class X9ReturnItem : X9DocumentComponent, ICheckImageContainer
    {
        [DataMember]
        public ReturnRecord Return { get; set; }
        [DataMember]
        public ReturnAddendumARecord ReturnAddendumA { get; set; }
        [DataMember]
        public ReturnAddendumBRecord ReturnAddendumB { get; set; }
        [DataMember]
        public ReturnAddendumDRecord ReturnAddendumD { get; set; }
        [DataMember]
        public List<X9DepositItemImage> Images { get; set; } = new List<X9DepositItemImage>();

        public override List<X9Record> GetRecords()
        {
            var recs = new List<X9Record>
            {
                this.Return,
                this.ReturnAddendumA,
                this.ReturnAddendumB,
                this.ReturnAddendumD
            };
            recs.AddRange(this.Images.Cast<X9Record>());
            return recs;
        }
    }
}
