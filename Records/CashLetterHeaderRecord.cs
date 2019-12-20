using System;
using System.Runtime.Serialization;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class CashLetterHeaderRecord : X9Record
    {
        public CashLetterHeaderRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField CollectionTypeIndicator { get; set; } = new X9TextField(new X9FieldType("CollectionTypeIndicator", X9FieldDataTypes.Numeric, 2));
        [Order]
        [DataMember]
        public X9TextField DestinationRoutingNumber { get; set; } = new X9TextField(new X9FieldType("DestinationRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField UniqueCustomerIdentifier { get; set; } = new X9TextField(new X9FieldType("UniqueCustomerIdentifier", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField CashLetterBusinessDate { get; set; } = new X9TextField(new X9FieldType("CashLetterBusinessDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField CashLetterCreationDate { get; set; } = new X9TextField(new X9FieldType("CashLetterCreationDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField CashLetterCreationTime { get; set; } = new X9TextField(new X9FieldType("CashLetterCreationTime", X9FieldDataTypes.Numeric, 4, typeof(TimeSpan)));
        [Order]
        [DataMember]
        public X9TextField CashLetterRecordTypeIndicator { get; set; } = new X9TextField(new X9FieldType("CashLetterRecordTypeIndicator", X9FieldDataTypes.Alphabetic, 1));
        [Order]
        [DataMember]
        public X9TextField CashLetterDocumentationTypeIndicator { get; set; } = new X9TextField(new X9FieldType("CashLetterDocumentationTypeIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField CashLetterId { get; set; } = new X9TextField(new X9FieldType("CashLetterId", X9FieldDataTypes.Alphameric, 8));
        [Order]
        [DataMember]
        public X9TextField OriginatorContactName { get; set; } = new X9TextField(new X9FieldType("OriginatorContactName", X9FieldDataTypes.AlphamericSpecial, 14));
        [Order]
        [DataMember]
        public X9TextField PhoneNumberOrUlid { get; set; } = new X9TextField(new X9FieldType("PhoneNumberOrUlid", X9FieldDataTypes.Numeric, 10));
        [Order]
        [DataMember]
        public X9TextField WorkType { get; set; } = new X9TextField(new X9FieldType("WorkType", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField ReturnIndicator { get; set; } = new X9TextField(new X9FieldType("ReturnIndicator", X9FieldDataTypes.Alphabetic, 1));
        [Order]
        [DataMember]
        public X9TextField UserField { get; set; } = new X9TextField(new X9FieldType("UserField", X9FieldDataTypes.AlphamericSpecial, 1));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 1));

        public void SetJPMorganFields(
            long custId,
            string cashLetterId,
            long? ulid,
            DateTimeOffset businessDate,
            DateTimeOffset createDate
        ) {
            this.CollectionTypeIndicator.SetValue(JPMorganConsts.CollectionTypeInd);
            this.DestinationRoutingNumber.SetValue(JPMorganConsts.DestinationRoutingNumber);
            this.UniqueCustomerIdentifier.SetValue(custId);
            this.CashLetterBusinessDate.SetValue(businessDate);
            this.CashLetterCreationDate.SetValue(createDate);
            this.CashLetterCreationTime.SetValue(createDate);
            this.CashLetterRecordTypeIndicator.SetValue("I");
            this.CashLetterDocumentationTypeIndicator.SetValue("G");
            this.CashLetterId.SetValue(cashLetterId);
            if (ulid.HasValue)
            {
                this.PhoneNumberOrUlid.SetValue(ulid);
            }
        }
    }
}
