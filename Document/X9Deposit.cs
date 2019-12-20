using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CompAnalytics.X9.Records;

namespace CompAnalytics.X9.Document
{
    [DataContract]
    [Serializable]
    public class X9Deposit : X9DocumentComponent
    {
        [DataMember]
        public CashLetterHeaderRecord CashLetterHeader { get; set; }
        [DataMember]
        public List<X9Bundle> Bundles { get; set; } = new List<X9Bundle>();
        [DataMember]
        public CashLetterTrailerRecord CashLetterTrailer { get; set; }

        public override List<X9Record> GetRecords()
        {
            var recs = new List<X9Record>
            {
                this.CashLetterHeader,
                this.CashLetterTrailer
            };
            recs.AddRange(this.Bundles.Cast<X9Record>());
            return recs;
        }
    }
}
