using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class CashLetterTrailerRecord : X9Record
    {
        public CashLetterTrailerRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField BundleCount { get; set; } = new X9TextField(new X9FieldType("BundleCount", X9FieldDataTypes.Numeric, 6));
        [Order]
        [DataMember]
        public X9TextField ItemsWithinCashLetterCount { get; set; } = new X9TextField(new X9FieldType("ItemsWithinCashLetterCount", X9FieldDataTypes.Numeric, 8));
        [Order]
        [DataMember]
        public X9TextField CashLetterTotalAmount { get; set; } = new X9TextField(new X9FieldType("CashLetterTotalAmount", X9FieldDataTypes.Numeric, 14));
        [Order]
        [DataMember]
        public X9TextField ImagesWithinCashLetterCount { get; set; } = new X9TextField(new X9FieldType("ImagesWithinCashLetterCount", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField EceInstitutionName { get; set; } = new X9TextField(new X9FieldType("EceInstitutionName", X9FieldDataTypes.AlphamericSpecial, 18));
        [Order]
        [DataMember]
        public X9TextField SettlementDate { get; set; } = new X9TextField(new X9FieldType("SettlementDate", X9FieldDataTypes.Numeric, 8, typeof(DateTimeOffset)));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 15));

        public void SetJPMorganFields(
            int numBundles,
            int numItems,
            int numImages,
            decimal cashLetterTotalAmount,
            DateTimeOffset? settlementDate
        )
        {
            this.BundleCount.SetValue(numBundles);
            this.ItemsWithinCashLetterCount.SetValue(numItems);
            this.ImagesWithinCashLetterCount.SetValue(numImages);
            this.CashLetterTotalAmount.SetValue(Math.Floor(cashLetterTotalAmount * 100));
            if (settlementDate.HasValue)
            {
                this.SettlementDate.SetValue(settlementDate);
            }
        }
    }
}
