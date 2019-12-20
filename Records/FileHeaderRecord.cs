using System;
using System.Runtime.Serialization;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class FileHeaderRecord : X9Record
    {
        public FileHeaderRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField StandardLevel { get; set; } = new X9TextField(new X9FieldType("StandardLevel", X9FieldDataTypes.Numeric, 2));
        [Order]
        [DataMember]
        public X9TextField TestFileIndicator { get; set; } = new X9TextField(new X9FieldType("TestFileIndicator", X9FieldDataTypes.Alphabetic, 1));
        [Order]
        [DataMember]
        public X9TextField ImmediateDestinationRoutingNumber { get; set; } = new X9TextField(new X9FieldType("ImmediateDestinationRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField ImmediateOriginRoutingNumber { get; set; } = new X9TextField(new X9FieldType("ImmediateOriginRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField FileCreationDate { get; set; } = new X9TextField(new X9FieldType("FileCreationDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField FileCreationTime { get; set; } = new X9TextField(new X9FieldType("FileCreationTime", X9FieldDataTypes.Numeric, 4, typeof(TimeSpan)));
        [Order]
        [DataMember]
        public X9TextField ResendIndicator { get; set; } = new X9TextField(new X9FieldType("ResendIndicator", X9FieldDataTypes.Alphabetic, 1));
        [Order]
        [DataMember]
        public X9TextField ImmediateDestinationName { get; set; } = new X9TextField(new X9FieldType("ImmediateDestinationName", X9FieldDataTypes.Alphameric, 18));
        [Order]
        [DataMember]
        public X9TextField ImmediateOriginName { get; set; } = new X9TextField(new X9FieldType("ImmediateOriginName", X9FieldDataTypes.Alphameric, 18));
        [Order]
        [DataMember]
        public X9TextField FileIdModifier { get; set; } = new X9TextField(new X9FieldType("FileIdModifier", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField CountryCode { get; set; } = new X9TextField(new X9FieldType("CountryCode", X9FieldDataTypes.Alphabetic, 2));
        [Order]
        [DataMember]
        public X9TextField UserField { get; set; } = new X9TextField(new X9FieldType("UserField", X9FieldDataTypes.AlphamericSpecial, 4));
        [Order]
        [DataMember]
        public X9TextField CompanionDocumentVersionIndicator { get; set; } = new X9TextField(new X9FieldType("CompanionDocumentVersionIndicator", X9FieldDataTypes.Alphameric, 1));

        public void SetJPMorganFields(
            bool isTest,
            long origRoutingNum,
            DateTimeOffset createTime,
            bool isResend,
            string origName
        ) {
            this.TestFileIndicator.SetValue(isTest ? "T" : "P");
            this.ImmediateOriginRoutingNumber.SetValue(origRoutingNum);
            this.FileCreationDate.SetValue(createTime);
            this.FileCreationTime.SetValue(createTime);
            this.ResendIndicator.SetValue(isResend ? "Y" : "N");
            this.ImmediateOriginName.SetValue($"ICL {origName}");

            this.StandardLevel.SetValue(JPMorganConsts.StandardLevel);
            this.ImmediateDestinationRoutingNumber.SetValue(JPMorganConsts.DestinationRoutingNumber);
            this.CompanionDocumentVersionIndicator.SetValue(JPMorganConsts.DocumentVersionInd);

        }
    }
}
