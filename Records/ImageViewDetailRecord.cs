using System;
using System.Runtime.Serialization;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class ImageViewDetailRecord : X9Record
    {
        public ImageViewDetailRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField ImageIndicator { get; set; } = new X9TextField(new X9FieldType("ImageIndicator", X9FieldDataTypes.Numeric, 1));
        [Order]
        [DataMember]
        public X9TextField ImageCreatorRoutingNumber { get; set; } = new X9TextField(new X9FieldType("ImageCreatorRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField ImageCreatorDate { get; set; } = new X9TextField(new X9FieldType("ImageCreatorDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField ImageViewFormatIndicator { get; set; } = new X9TextField(new X9FieldType("ImageViewFormatIndicator", X9FieldDataTypes.Numeric, 2));
        [Order]
        [DataMember]
        public X9TextField ImageCompressionAlgorithmIndicator { get; set; } = new X9TextField(new X9FieldType("ImageCompressionAlgorithmIndicator", X9FieldDataTypes.Numeric, 2));
        [Order]
        [DataMember]
        public X9TextField ImageViewDataSize { get; set; } = new X9TextField(new X9FieldType("ImageViewDataSize", X9FieldDataTypes.Numeric, 7));
        [Order]
        [DataMember]
        public X9TextField ViewSideIndicator { get; set; } = new X9TextField(new X9FieldType("ViewSideIndicator", X9FieldDataTypes.Numeric, 1, typeof(bool?)));
        [Order]
        [DataMember]
        public X9TextField ViewDescriptor { get; set; } = new X9TextField(new X9FieldType("ViewDescriptor", X9FieldDataTypes.Numeric, 2));
        [Order]
        [DataMember]
        public X9TextField DigitalSignatureIndicator { get; set; } = new X9TextField(new X9FieldType("DigitalSignatureIndicator", X9FieldDataTypes.Numeric, 1));
        [Order]
        [DataMember]
        public X9TextField DigitalSignatureMethod { get; set; } = new X9TextField(new X9FieldType("DigitalSignatureMethod", X9FieldDataTypes.NumericBlank, 2));
        [Order]
        [DataMember]
        public X9TextField SecurityKeySize { get; set; } = new X9TextField(new X9FieldType("SecurityKeySize", X9FieldDataTypes.NumericBlank, 5));
        [Order]
        [DataMember]
        public X9TextField StartOfProtectedData { get; set; } = new X9TextField(new X9FieldType("StartOfProtectedData", X9FieldDataTypes.NumericBlank, 7));
        [Order]
        [DataMember]
        public X9TextField LengthOfProtectedData { get; set; } = new X9TextField(new X9FieldType("LengthOfProtectedData", X9FieldDataTypes.NumericBlank, 7));
        [Order]
        [DataMember]
        public X9TextField ImageRecreateIndicator { get; set; } = new X9TextField(new X9FieldType("ImageRecreateIndicator", X9FieldDataTypes.Numeric, 1, typeof(bool?)));
        [Order]
        [DataMember]
        public X9TextField UserField { get; set; } = new X9TextField(new X9FieldType("UserField", X9FieldDataTypes.AlphamericSpecial, 8));
        [Order]
        [DataMember]
        public X9TextField ImageTiffVarianceIndicator { get; set; } = new X9TextField(new X9FieldType("ImageTiffVarianceIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField ImageOverrideIndicator { get; set; } = new X9TextField(new X9FieldType("ImageOverrideIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 13));

        public void SetJPMorganFields(
            DateTimeOffset scanDate,
            bool isBackOfCheck,
            bool canRecreateImage
        )
        {
            this.ImageIndicator.SetValue(1);
            this.ImageCreatorRoutingNumber.SetValue(JPMorganConsts.DestinationRoutingNumber);
            this.ImageCreatorDate.SetValue(scanDate);
            this.ImageViewFormatIndicator.SetValue("00");
            this.ViewSideIndicator.SetValue(isBackOfCheck);
            this.ViewDescriptor.SetValue("00");
            this.DigitalSignatureIndicator.SetValue("0");
            this.ImageRecreateIndicator.SetValue(canRecreateImage);
        }
    }
}
