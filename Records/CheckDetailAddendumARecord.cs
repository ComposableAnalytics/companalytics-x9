using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    /// <remarks>
    /// The field types here were inferred from a sample file and may be incorrect.
    /// </remarks>
    [DataContract]
    [Serializable]
    public class CheckDetailAddendumARecord : X9Record
    {
        public CheckDetailAddendumARecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField CheckDetailAddendumARecordNumber { get; set; } = new X9TextField(new X9FieldType("CheckDetailAddendumARecordNumber", X9FieldDataTypes.Numeric, 1));
        [Order]
        [DataMember]
        public X9TextField ReturnLocationRoutingNumber { get; set; } = new X9TextField(new X9FieldType("ReturnLocationRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField BofdEndorsementBusinessDate { get; set; } = new X9TextField(new X9FieldType("BofdEndorsementBusinessDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField BofdItemSequenceNumber { get; set; } = new X9TextField(new X9FieldType("BofdItemSequenceNumber", X9FieldDataTypes.Numeric, 15));
        [Order]
        [DataMember]
        public X9TextField DepositAccountNumberAtBofd { get; set; } = new X9TextField(new X9FieldType("DepositAccountNumberAtBofd", X9FieldDataTypes.NumericBlank, 18));
        [Order]
        [DataMember]
        public X9TextField BofdDepositBranch { get; set; } = new X9TextField(new X9FieldType("BofdDepositBranch", X9FieldDataTypes.NumericBlank, 5));
        [Order]
        [DataMember]
        public X9TextField PayeeName { get; set; } = new X9TextField(new X9FieldType("PayeeName", X9FieldDataTypes.Alphameric, 15));
        [Order]
        [DataMember]
        public X9TextField TruncationIndicator { get; set; } = new X9TextField(new X9FieldType("TruncationIndicator", X9FieldDataTypes.Alphabetic, 1));
        [Order]
        [DataMember]
        public X9TextField BofdConversionIndicator { get; set; } = new X9TextField(new X9FieldType("BofdConversionIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField BofdCorrectionIndicator { get; set; } = new X9TextField(new X9FieldType("BofdCorrectionIndicator", X9FieldDataTypes.Numeric, 1));
        [Order]
        [DataMember]
        public X9TextField UserField { get; set; } = new X9TextField(new X9FieldType("UserField", X9FieldDataTypes.Blank, 1));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 3));

    }
}
