using System;
using System.Runtime.Serialization;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class BundleHeaderRecord : X9Record
    {
        public BundleHeaderRecord() : base() { }

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
        public X9TextField BundleBusinessDate { get; set; } = new X9TextField(new X9FieldType("BundleBusinessDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField BundleCreationDate { get; set; } = new X9TextField(new X9FieldType("BundleCreationDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField BundleId { get; set; } = new X9TextField(new X9FieldType("BundleId", X9FieldDataTypes.Alphameric, 10));
        [Order]
        [DataMember]
        public X9TextField BundleSequenceNumber { get; set; } = new X9TextField(new X9FieldType("BundleSequenceNumber", X9FieldDataTypes.NumericBlank, 4));
        [Order]
        [DataMember]
        public X9TextField CycleNumber { get; set; } = new X9TextField(new X9FieldType("CycleNumber", X9FieldDataTypes.Blank, 2));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 9));
        [Order]
        [DataMember]
        public X9TextField UserField { get; set; } = new X9TextField(new X9FieldType("UserField", X9FieldDataTypes.AlphamericSpecial, 5));
        [Order]
        [DataMember]
        public X9TextField Reserved2 { get; set; } = new X9TextField(new X9FieldType("Reserved2", X9FieldDataTypes.Blank, 12));

        public void SetJPMorganFields(
            long custId, 
            string bundleId,
            int bundleIndex,
            DateTimeOffset businessDate,
            DateTimeOffset createDate
        )
        {
            this.CollectionTypeIndicator.SetValue(JPMorganConsts.CollectionTypeInd);
            this.DestinationRoutingNumber.SetValue(JPMorganConsts.DestinationRoutingNumber);
            this.UniqueCustomerIdentifier.SetValue(custId);
            this.BundleBusinessDate.SetValue(businessDate);
            this.BundleCreationDate.SetValue(createDate);
            this.BundleId.SetValue(bundleId);
            this.BundleSequenceNumber.SetValue(bundleIndex);
        }
    }
}
