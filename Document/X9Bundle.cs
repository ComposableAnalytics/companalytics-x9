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
        public List<X9ReturnItem> ReturnItems { get; set; } = new List<X9ReturnItem>();
        [DataMember]
        public BundleTrailerRecord Trailer { get; set; }

        public bool IsReturn => this.ReturnItems != null && this.ReturnItems.Any() && (this.DepositItems == null || !this.DepositItems.Any());
        public bool IsDeposit => this.DepositItems != null && this.DepositItems.Any() && (this.ReturnItems == null || !this.ReturnItems.Any());
        public bool IsInvalid => !(this.IsDeposit || this.IsReturn);

        public override List<X9Record> GetRecords()
        {
            var recs = new List<X9Record>
            {
                this.Header,
                this.Trailer
            };
            recs.AddRange(this.DepositItems.Cast<X9Record>());
            recs.AddRange(this.ReturnItems.Cast<X9Record>());
            return recs;
        }

        public void ThrowIfInvalid()
        {
            if (this.IsInvalid)
            {
                throw new X9AuthoringException("An X9Bundle must have one or more DepositItems or ReturnItems (but not both) to be written.");
            }
        }
    }
}
