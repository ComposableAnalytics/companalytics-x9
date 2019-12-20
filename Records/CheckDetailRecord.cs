using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using CompAnalytics.X9.JPMorganAuthoring;

namespace CompAnalytics.X9.Records
{
    [DataContract]
    [Serializable]
    public class CheckDetailRecord : X9Record
    {
        public CheckDetailRecord() : base() { }

        [Order]
        [DataMember]
        public X9TextField AuxiliaryOnUs { get; set; } = new X9TextField(new X9FieldType("AuxiliaryOnUs", X9FieldDataTypes.NumericBlankSpecialMicr, 15));
        [Order]
        [DataMember]
        public X9TextField ExternalProcessingCode { get; set; } = new X9TextField(new X9FieldType("ExternalProcessingCode", X9FieldDataTypes.NumericSpecial, 1));
        [Order]
        [DataMember]
        public X9TextField PayorBankRoutingNumber { get; set; } = new X9TextField(new X9FieldType("PayorBankRoutingNumber", X9FieldDataTypes.Numeric, 9));
        [Order]
        [DataMember]
        public X9TextField MICROnUs { get; set; } = new X9TextField(new X9FieldType("MICROnUs", X9FieldDataTypes.NumericBlankSpecialMicrOnUs, 20));
        [Order]
        [DataMember]
        public X9TextField Amount { get; set; } = new X9TextField(new X9FieldType("Amount", X9FieldDataTypes.Numeric, 10));
        [Order]
        [DataMember]
        public X9TextField EceInstitutionItemSequenceNumber { get; set; } = new X9TextField(new X9FieldType("EceInstitutionItemSequenceNumber", X9FieldDataTypes.NumericBlank, 15));
        [Order]
        [DataMember]
        public X9TextField DocumentationTypeIndicator { get; set; } = new X9TextField(new X9FieldType("DocumentationTypeIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField ReturnAcceptanceIndicator { get; set; } = new X9TextField(new X9FieldType("ReturnAcceptanceIndicator", X9FieldDataTypes.Alphameric, 1));
        [Order]
        [DataMember]
        public X9TextField MicrValidIndicator { get; set; } = new X9TextField(new X9FieldType("MicrValidIndicator", X9FieldDataTypes.Numeric, 1));
        [Order]
        [DataMember]
        public X9TextField BofdIndicator { get; set; } = new X9TextField(new X9FieldType("BofdIndicator", X9FieldDataTypes.Alphabetic, 1));
        [Order]
        [DataMember]
        public X9TextField CheckDetailRecordAddendumCount { get; set; } = new X9TextField(new X9FieldType("CheckDetailRecordAddendumCount", X9FieldDataTypes.Numeric, 2));
        [Order]
        [DataMember]
        public X9TextField CorrectionIndicator { get; set; } = new X9TextField(new X9FieldType("CorrectionIndicator", X9FieldDataTypes.Numeric, 1));
        [Order]
        [DataMember]
        public X9TextField ArchiveTypeIndicator { get; set; } = new X9TextField(new X9FieldType("ArchiveTypeIndicator", X9FieldDataTypes.Alphameric, 1));

        public void SetJPMorganFields(
            string auxOnUs,
            string extProcessingCode,
            string payorBankRouting,
            string onUs,
            decimal amountDollars,
            string institutionItemSeq
        )
        {
            if (auxOnUs != null)
            {
                string formattedAuxOnUs = new Regex(@"[U\s]")
                    .Replace(auxOnUs, "")
                    .Replace('D', '-');
                this.AuxiliaryOnUs.SetValue(formattedAuxOnUs);
            }
            
            if (extProcessingCode == "2" || extProcessingCode == "5")
            {
                throw new ArgumentException($"JPMorgan does not support value {extProcessingCode} for the External Processing Code.");
            }
            this.ExternalProcessingCode.SetValue(extProcessingCode);
            
            this.PayorBankRoutingNumber.SetValue(payorBankRouting);

            if (onUs != null)
            {
                string formattedOnUs = new Regex(@"\s").Replace(onUs, "")
                    .Replace('U', '/')
                    .Replace('D', '-');
                this.MICROnUs.SetValue(formattedOnUs);
            }

            this.Amount.SetValue(Math.Floor(amountDollars * 100));
            this.EceInstitutionItemSequenceNumber.SetValue(institutionItemSeq);
            this.BofdIndicator.SetValue(JPMorganConsts.BofdIndicator);
            this.CheckDetailRecordAddendumCount.SetValue("01");
        }
    }
}
