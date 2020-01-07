using System;
using System.IO;
using CompAnalytics.X9.Document;

namespace CompAnalytics.X9
{
    public class X9Writer : X9Modifier
    {
        X9Document Document { get; set; }

        public X9Writer(X9Document doc) : base()
        {
            this.Document = doc;
        }

        /// <summary>
        /// Convert the provided X9 record to a sequence of bytes and write it to the byte stream
        /// </summary>
        void WriteRecord(X9Record record)
        {
            // Generate bytes for record and its size prefix
            byte[] recordBytes = record.WriteRecord();
            byte[] recordSizePrefix = BitConverter.GetBytes(recordBytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(recordSizePrefix);
            }

            // Write them to the stream
            this.ByteStream.Write(recordSizePrefix, 0, recordSizePrefix.Length);
            this.ByteStream.Write(recordBytes, 0, recordBytes.Length);
        }

        /// <summary>
        /// Converts the X9Document provided in the constructor to a byte array holding the full X9 file
        /// </summary>
        public byte[] WriteX9Document()
        {
            this.ByteStream = new MemoryStream();

            this.WriteRecord(Document.Header);

            foreach (X9Deposit deposit in Document.Deposits)
            {
                this.WriteRecord(deposit.CashLetterHeader);

                foreach (X9Bundle bundle in deposit.Bundles)
                {
                    bundle.ThrowIfInvalid();
                    this.WriteRecord(bundle.Header);
                    
                    if (bundle.IsDeposit)
                    {
                        foreach (X9DepositItem item in bundle.DepositItems)
                        {
                            this.WriteRecord(item.CheckDetail);
                            if (item.CheckDetailAddendum != null)
                            {
                                this.WriteRecord(item.CheckDetailAddendum);
                            }

                            foreach (X9DepositItemImage image in item.Images)
                            {
                                this.WriteRecord(image.ImageViewDetail);
                                this.WriteRecord(image.ImageViewData);
                            }
                        }
                    }
                    else
                    {
                        foreach (X9ReturnItem item in bundle.ReturnItems)
                        {
                            this.WriteRecord(item.Return);
                            if (item.ReturnAddendumA != null)
                            {
                                this.WriteRecord(item.ReturnAddendumA);
                            }
                            if (item.ReturnAddendumB != null)
                            {
                                this.WriteRecord(item.ReturnAddendumB);
                            }
                            if (item.ReturnAddendumD != null)
                            {
                                this.WriteRecord(item.ReturnAddendumD);
                            }

                            foreach (X9DepositItemImage image in item.Images)
                            {
                                this.WriteRecord(image.ImageViewDetail);
                                this.WriteRecord(image.ImageViewData);
                            }
                        }
                    }

                    this.WriteRecord(bundle.Trailer);
                }

                this.WriteRecord(deposit.CashLetterTrailer);
            }

            this.WriteRecord(Document.Trailer);

            byte[] ret;
            using (var ms = new MemoryStream())
            {
                this.ByteStream.Position = 0;
                this.ByteStream.CopyTo(ms);
                ret = ms.ToArray();
            }
            return ret;
        }
    }
}
