using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class ReturnAddendumDRecord : X9Record
    {
        public ReturnAddendumDRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField CheckDetailAddendumDRecordNumber { get; set; } = new X9TextField(new X9FieldType("CheckDetailAddendumDRecordNumber", X9FieldDataTypes.Numeric, 2));
        [Order]
        [DataMember]
        public X9TextField EndorsingBankRoutingNumber { get; set; } = new X9TextField(new X9FieldType("EndorsingBankRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField EndorsingBankEndorsementDate { get; set; } = new X9TextField(new X9FieldType("EndorsingBankEndorsementDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField EndorsingBankItemSequenceNumber { get; set; } = new X9TextField(new X9FieldType("EndorsingBankItemSequenceNumber", X9FieldDataTypes.NumericBlank, 15));
        [Order]
        [DataMember]
        public X9TextField TruncationIndicator { get; set; } = new X9TextField(new X9FieldType("TruncationIndicator", X9FieldDataTypes.Alphabetic, 1));
        [Order]
        [DataMember]
        public X9TextField EndorsingBankConversionIndicator { get; set; } = new X9TextField(new X9FieldType("EndorsingBankConversionIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField EndorsingBankCorrectionIndicator { get; set; } = new X9TextField(new X9FieldType("EndorsingBankCorrectionIndicator", X9FieldDataTypes.Numeric, 1));
        [Order]
        [DataMember]
        public X9TextField ReturnReason { get; set; } = new X9TextField(new X9FieldType("ReturnReason", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField UserField { get; set; } = new X9TextField(new X9FieldType("UserField", X9FieldDataTypes.AlphamericSpecial, 19));
        [Order]
        [DataMember]
        public X9TextField EndorsingBankIdentifier { get; set; } = new X9TextField(new X9FieldType("EndorsingBankIdentifier", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 20));
    }
}
