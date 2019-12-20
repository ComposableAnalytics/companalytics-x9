using System;
using System.Runtime.Serialization;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class ImageViewDataRecord : X9DynamicLengthRecord
    {
        public ImageViewDataRecord() : base()
        {
            this.DynamicField = this.ImageData;
            this.DynamicFieldLengthField = this.LengthOfImageData;
        }

        [Order]
        [DataMember]
        public X9TextField EceInstitutionRoutingNumber { get; set; } = new X9TextField(new X9FieldType("EceInstitutionRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField BundleBusinessDate { get; set; } = new X9TextField(new X9FieldType("BundleBusinessDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField CycleNumber { get; set; } = new X9TextField(new X9FieldType("CycleNumber", X9FieldDataTypes.Alphameric, 2));
        [Order]
        [DataMember]
        public X9TextField EceInstitutionItemSequenceNumber { get; set; } = new X9TextField(new X9FieldType("EceInstitutionItemSequenceNumber", X9FieldDataTypes.NumericBlank, 15));
        [Order]
        [DataMember]
        public X9TextField SecurityOriginatorName { get; set; } = new X9TextField(new X9FieldType("SecurityOriginatorName", X9FieldDataTypes.AlphamericSpecial, 16));
        [Order]
        [DataMember]
        public X9TextField SecurityAuthenticatorName { get; set; } = new X9TextField(new X9FieldType("SecurityAuthenticatorName", X9FieldDataTypes.AlphamericSpecial, 16));
        [Order]
        [DataMember]
        public X9TextField SecurityKeyName { get; set; } = new X9TextField(new X9FieldType("SecurityKeyName", X9FieldDataTypes.AlphamericSpecial, 16));
        [Order]
        [DataMember]
        public X9TextField ClippingOrigin { get; set; } = new X9TextField(new X9FieldType("ClippingOrigin", X9FieldDataTypes.NumericBlank, 1));
        [Order]
        [DataMember]
        public X9TextField ClippingCoordinateH1 { get; set; } = new X9TextField(new X9FieldType("ClippingCoordinateH1", X9FieldDataTypes.NumericBlank, 4));
        [Order]
        [DataMember]
        public X9TextField ClippingCoordinateH2 { get; set; } = new X9TextField(new X9FieldType("ClippingCoordinateH2", X9FieldDataTypes.NumericBlank, 4));
        [Order]
        [DataMember]
        public X9TextField ClippingCoordinateV1 { get; set; } = new X9TextField(new X9FieldType("ClippingCoordinateV1", X9FieldDataTypes.NumericBlank, 4));
        [Order]
        [DataMember]
        public X9TextField ClippingCoordinateV2 { get; set; } = new X9TextField(new X9FieldType("ClippingCoordinateV2", X9FieldDataTypes.NumericBlank, 4));
        [Order]
        [DataMember]
        public X9TextField LengthOfImageReferenceKey { get; set; } = new X9TextField(new X9FieldType("LengthOfImageReferenceKey", X9FieldDataTypes.NumericBlank, 4));
        [Order]
        [DataMember]
        public X9TextField LengthOfDigitalSignature { get; set; } = new X9TextField(new X9FieldType("LengthOfDigitalSignature", X9FieldDataTypes.NumericBlank, 5));
        [Order]
        [DataMember]
        public X9TextField LengthOfImageData { get; set; } = new X9TextField(new X9FieldType("LengthOfImageData", X9FieldDataTypes.NumericBlank, 7));
        [Order]
        [DataMember]
        public X9ImageField ImageData { get; set; } = new X9ImageField(new X9FieldType("ImageData", X9FieldDataTypes.Binary));

        public void SetJPMorganFields(
            DateTimeOffset businessDate,
            string institutionItemSeq,
            ref byte[] imageData
        )
        {
            this.EceInstitutionRoutingNumber.SetValue(JPMorganConsts.DestinationRoutingNumber);
            this.BundleBusinessDate.SetValue(businessDate);
            this.ClippingOrigin.SetValue(0);
            this.LengthOfImageReferenceKey.SetValue(0);
            this.LengthOfDigitalSignature.SetValue(0);
            this.LengthOfImageData.SetValue(imageData.Length);
            this.ImageData.SetImageBytes(imageData);
        }
    }
}
