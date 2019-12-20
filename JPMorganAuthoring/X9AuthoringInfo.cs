using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.JPMorganAuthoring
{
    [DataContract]
    [Serializable]
    public class X9AuthoringInfo
    {
        /// <summary>
        /// Is the file to be used for testing (not a real payment)?
        /// </summary>
        [DataMember]
        public bool IsTest { get; set; }
        [DataMember]
        public long OrigRoutingNum { get; set; }
        /// <summary>
        /// True if this file has already been transmitted in its entirety
        /// </summary>
        [DataMember]
        public bool IsResend { get; set; }
        /// <summary>
        /// Customer name
        /// </summary>
        [DataMember]
        public string OrigName { get; set; }
        [DataMember]
        public List<DepositAuthoringInfo> Deposits { get; set; }
    }
}
