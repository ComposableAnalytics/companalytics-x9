using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.JPMorganAuthoring
{
    [DataContract]
    [Serializable]
    public class DepositAuthoringInfo
    {
        /// <summary>
        /// Customer identifier assigned by JPM
        /// </summary>
        [DataMember]
        public long CustId { get; set; }
        /// <summary>
        /// Uniquely identifies a cash letter within a particular business date
        /// </summary>
        [DataMember]
        public string CashLetterId { get; set; }
        /// <summary>
        /// Unique location identifier
        /// </summary>
        [DataMember]
        public long? Ulid { get; set; }
        [DataMember]
        public DateTimeOffset BusinessDate { get; set; }
        /// <summary>
        /// The year, month, and day that the institution that creates the cash letter expects settlement
        /// </summary>
        [DataMember]
        public DateTimeOffset? SettlementDate { get; set; }
        [DataMember]
        public List<BundleAuthoringInfo> Bundles { get; set; }
    }
}
