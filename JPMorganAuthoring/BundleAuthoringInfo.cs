using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.JPMorganAuthoring
{
    [DataContract]
    [Serializable]
    public class BundleAuthoringInfo
    {
        /// <summary>
        /// A number that identifies the bundle, assigned by the depositor that created the bundle. 
        /// Must be unique within a Cash Letter business date.
        /// </summary>
        [DataMember]
        public string BundleId { get; set; }
        [DataMember]
        public List<DepositItemAuthoringInfo> DepositItems { get; set; }
    }
}
