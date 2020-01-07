using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class ReturnRecord : X9Record
    {
        public ReturnRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField PayorBankRoutingNumber { get; set; } = new X9TextField(new X9FieldType("PayorBankRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField MicrOnUs { get; set; } = new X9TextField(new X9FieldType("MicrOnUs", X9FieldDataTypes.NumericBlankSpecialMicrOnUs, 20));
        [Order]
        [DataMember]
        public X9TextField Amount { get; set; } = new X9TextField(new X9FieldType("Amount", X9FieldDataTypes.Numeric, 10));
        [Order]
        [DataMember]
        public X9TextField ReturnReason { get; set; } = new X9TextField(new X9FieldType("ReturnReason", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField ReturnRecordAddendumCount { get; set; } = new X9TextField(new X9FieldType("ReturnRecordAddendumCount", X9FieldDataTypes.Numeric, 2));
        [Order]
        [DataMember]
        public X9TextField ReturnDocumentationTypeIndicator { get; set; } = new X9TextField(new X9FieldType("ReturnDocumentationTypeIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField ForwardBusinessDate { get; set; } = new X9TextField(new X9FieldType("ForwardBusinessDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField EceInstitutionItemSequenceNumber { get; set; } = new X9TextField(new X9FieldType("EceInstitutionItemSequenceNumber", X9FieldDataTypes.NumericBlank, 15));
        [Order]
        [DataMember]
        public X9TextField ExternalProcessingCode { get; set; } = new X9TextField(new X9FieldType("ExternalProcessingCode", X9FieldDataTypes.NumericSpecial, 1));
        [Order]
        [DataMember]
        public X9TextField ReturnNotificationIndicator { get; set; } = new X9TextField(new X9FieldType("ReturnNotificationIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField ArchiveTypeIndicator { get; set; } = new X9TextField(new X9FieldType("ArchiveTypeIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField NumberOfTimesReturned { get; set; } = new X9TextField(new X9FieldType("NumberOfTimesReturned", X9FieldDataTypes.NumericBlank, 1));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 8));
    }
}
