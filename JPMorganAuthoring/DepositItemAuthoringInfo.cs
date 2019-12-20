using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.JPMorganAuthoring
{
    [DataContract]
    [Serializable]
    public class DepositItemAuthoringInfo
    {
        /// <summary>
        /// A code used on commercial checks at the discretion of the payor bank. Mandatory if present on the MICR lined 
        /// 7 or the serial number.
        /// </summary>
        /// <remarks>
        /// Rules for formatting this field are:
        /// • Dashes must be retained
        /// • Spaces may be omitted
        /// • Blank fill any unused positions
        /// • If the field is not present on the item, the field must be formatted with spaces
        /// • On-Us symbols on the MICR line, shall not be included
        /// </remarks>
        [DataMember]
        public string AuxOnUs { get; set; }
        /// <summary>
        /// A code used for special purposes, also known as Position 44. Mandatory if present on the MICR line.
        /// The External Processing Code is position 44 or 45 of the MICR line.This field is located immediately to
        /// the left of the Routing Transit field. This field is also known as field 6. 
        /// </summary>
        /// <remarks>
        /// Rules for formatting this field are:
        /// - Values ‘2’ and ‘5’ shall not be used
        /// </remarks>
        [DataMember]
        public string ExtProcessingCode { get; set; }
        [DataMember]
        public string PayorBankRoutingNum { get; set; }
        /// <summary>
        /// Mandatory if present on the MICR Line. The On-Us field of the MICR document is located between positions 
        /// 13 and 32 of the MICR line of the item.
        /// </summary>
        /// <remarks>
        /// • Translate On-Us symbols to forward slashes "/"
        /// • Right-justify the data
        /// • Dashes must be retained
        /// • Spaces must be suppressed, not included in this field
        /// </remarks>
        [DataMember]
        public string OnUs { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string InstitutionItemSeq { get; set; }
        [DataMember]
        public List<ImageAuthoringInfo> DepositItemImages { get; set; }
    }
}
