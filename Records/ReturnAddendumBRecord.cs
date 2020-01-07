using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class ReturnAddendumBRecord : X9Record
    {
        public ReturnAddendumBRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField PayorBankName { get; set; } = new X9TextField(new X9FieldType("PayorBankName", X9FieldDataTypes.AlphamericSpecial, 18));
        [Order]
        [DataMember]
        public X9TextField AuxiliaryOnUs { get; set; } = new X9TextField(new X9FieldType("AuxiliaryOnUs", X9FieldDataTypes.NumericBlankSpecialMicr, 15));
        [Order]
        [DataMember]
        public X9TextField PayorBankItemSequenceNumber { get; set; } = new X9TextField(new X9FieldType("PayorBankItemSequenceNumber", X9FieldDataTypes.NumericBlank, 15));
        [Order]
        [DataMember]
        public X9TextField PayorBankBusinessDate { get; set; } = new X9TextField(new X9FieldType("PayorBankBusinessDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField PayorAccountName { get; set; } = new X9TextField(new X9FieldType("PayorAccountName", X9FieldDataTypes.AlphamericSpecial, 22));
    }
}
