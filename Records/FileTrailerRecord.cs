using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class FileTrailerRecord : X9Record
    {
        public FileTrailerRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField CashLetterCount { get; set; } = new X9TextField(new X9FieldType("CashLetterCount", X9FieldDataTypes.Numeric, 6));
        [Order]
        [DataMember]
        public X9TextField TotalRecordCount { get; set; } = new X9TextField(new X9FieldType("TotalRecordCount", X9FieldDataTypes.Numeric, 8));
        [Order]
        [DataMember]
        public X9TextField TotalItemCount { get; set; } = new X9TextField(new X9FieldType("TotalItemCount", X9FieldDataTypes.Numeric, 8));
        [Order]
        [DataMember]
        public X9TextField TotalFileAmount { get; set; } = new X9TextField(new X9FieldType("TotalFileAmount", X9FieldDataTypes.Numeric, 16));
        [Order]
        [DataMember]
        public X9TextField ImmediateOriginContactName { get; set; } = new X9TextField(new X9FieldType("ImmediateOriginContactName", X9FieldDataTypes.AlphamericSpecial, 14));
        [Order]
        [DataMember]
        public X9TextField ImmediateOriginContactPhone { get; set; } = new X9TextField(new X9FieldType("ImmediateOriginContactPhone", X9FieldDataTypes.Numeric, 10));
        [Order]
        [DataMember]
        public X9TextField Reserved { get; set; } = new X9TextField(new X9FieldType("Reserved", X9FieldDataTypes.Blank, 16));

        public void SetJPMorganFields(
            int numCashLetters,
            int totalRecordCount,
            int totalItemCount,
            decimal fileTotalAmount
        )
        {
            this.CashLetterCount.SetValue(numCashLetters);
            this.TotalRecordCount.SetValue(totalRecordCount);
            this.TotalItemCount.SetValue(totalItemCount);
            this.TotalFileAmount.SetValue(Math.Floor(fileTotalAmount * 100));
        }
    }
}
