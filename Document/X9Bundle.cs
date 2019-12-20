using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CompAnalytics.X9.Records;

namespace CompAnalytics.X9.Document
{
    [DataContract]
    [Serializable]
    public class X9Bundle : X9DocumentComponent
    {
        [DataMember]
        public BundleHeaderRecord Header { get; set; }
        [DataMember]
        public List<X9DepositItem> DepositItems { get; set; } = new List<X9DepositItem>();
        [DataMember]
        public BundleTrailerRecord Trailer { get; set; }

        public override List<X9Record> GetRecords()
        {
            var recs = new List<X9Record>
            {
                this.Header,
                this.Trailer
            };
            recs.AddRange(this.DepositItems.Cast<X9Record>());
            return recs;
        }
    }
}
