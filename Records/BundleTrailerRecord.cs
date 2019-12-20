using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class BundleTrailerRecord : X9Record
    {
        public BundleTrailerRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField ItemsWithinBundleCount { get; set; } = new X9TextField(new X9FieldType("ItemsWithinBundleCount", X9FieldDataTypes.Numeric, 4));
        [Order]
        [DataMember]
        public X9TextField BundleAmount { get; set; } = new X9TextField(new X9FieldType("BundleAmount", X9FieldDataTypes.Numeric, 12));
        [Order]
        [DataMember]
        public X9TextField MicrValidTotalAmount { get; set; } = new X9TextField(new X9FieldType("MicrValidTotalAmount", X9FieldDataTypes.Numeric, 12));
        [Order]
        [DataMember]
        public X9TextField ImagesWithinBundleCount { get; set; } = new X9TextField(new X9FieldType("ImagesWithinBundleCount", X9FieldDataTypes.Numeric, 5));
        [Order]
        [DataMember]
        public X9TextField UserField { get; set; } = new X9TextField(new X9FieldType("UserField", X9FieldDataTypes.AlphamericSpecial, 20));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 25));

        public void SetJPMorganFields(
            int itemsInBundle,
            int imagesInBundle,
            decimal bundleTotalDollars
        )
        {
            this.ItemsWithinBundleCount.SetValue(itemsInBundle);
            this.BundleAmount.SetValue(Math.Floor(bundleTotalDollars * 100));
            this.ImagesWithinBundleCount.SetValue(imagesInBundle);
        }
    }
}
