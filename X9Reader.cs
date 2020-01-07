using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CompAnalytics.Contracts;
using CompAnalytics.Utils;
using CompAnalytics.X9.Document;
using CompAnalytics.X9.Records;

namespace CompAnalytics.X9
{
    public class X9Reader : X9Modifier
    {
        X9Document Document { get; set; }

        public X9Reader(byte[] data)
        {
            this.ByteStream = new MemoryStream(data);
        }
        public X9Reader(Stream stream)
        {
            this.ByteStream = new MemoryStream();
            stream.CopyTo(this.ByteStream);
            this.ByteStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Reads a desired number of bytes from the file, starting at the current position.
        /// </summary>
        public byte[] ReadNextBytes(int numBytesToRead)
        {
            if (numBytesToRead > this.ByteStream.Length - this.ByteStream.Position)
            {
                throw new IndexOutOfRangeException("Not enough bytes left to read.");
            }

            var ret = new byte[numBytesToRead];
            this.ByteStream.Read(ret, 0, numBytesToRead);
            return ret;
        }

        /// <summary>
        /// Determines the size of the next X9 record in bytes, as recorded in a 4-byte integer
        /// that be present in the next 4 bytes to be read from the stream. Advances the stream's
        /// position by 4 bytes.
        /// </summary>
        public int ReadNextRecordSize()
        {
            byte[] recordPrefix = this.ReadNextBytes(RecordSizeBytes);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(recordPrefix);
            }
            return BitConverter.ToInt32(recordPrefix, 0);
        }

        /// <summary>
        /// Returns the full sequence of bytes comprising the next X9 record, starting with the 
        /// Record Type field.
        /// </summary>
        public byte[] ReadNextRecordBytes()
        {
            int size = this.ReadNextRecordSize();
            return this.ReadNextBytes(size);
        }

        /// <summary>
        /// Reads and parses the next X9 record from the file
        /// </summary>
        public X9Record ReadNextRecord()
        {
            return this.ReadNextRecord<X9Record>();
        }

        /// <summary>
        /// Reads and parses the next X9 record from the file. The type parameter must match the type of the next record found.
        /// </summary>
        public T ReadNextRecord<T>()
        {
            string recordTypeCode = this.PeekNextRecordTypeCode();
            Type recordType = RecordTypes.GetByCode(recordTypeCode);
            byte[] recordBytes = this.ReadNextRecordBytes();
            if (recordType != null && typeof(T) == recordType)
            {
                X9Record record = (X9Record)Activator.CreateInstance(recordType);
                record.ReadRecord(recordBytes);
                return (T)Convert.ChangeType(record, typeof(T));
            }
            else if (typeof(T) != recordType)
            {
                throw new FormatException($"Expected to see {typeof(T).Name}, saw {(recordType != null ? recordType.Name : recordTypeCode)} instead.");
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Returns the Record Type (two digit code) of the next X9 record in the file
        /// </summary>
        public string PeekNextRecordTypeCode()
        {
            // Skip the record size
            this.ByteStream.Seek(RecordSizeBytes, SeekOrigin.Current);

            // Read the record type code
            byte[] recordTypeRaw = new byte[RecordTypeBytes];
            this.ByteStream.Read(recordTypeRaw, 0, RecordTypeBytes);
            this.ByteStream.Seek(-1 * (RecordTypeBytes + RecordSizeBytes), SeekOrigin.Current);
            return Edcbic.GetString(recordTypeRaw);
        }

        /// <summary>
        /// Returns the .NET type of the next X9 record in the file
        /// </summary>
        public Type PeekNextRecordType()
        {
            string recordTypeStr = this.PeekNextRecordTypeCode();

            // Return the appropriate type if we support it
            return RecordTypes.GetByCode(recordTypeStr);
        }

        /// <summary>
        /// Given a well-structured X9Document, takes each ImageViewDataRecord's ImageData field, represented as a byte[],
        /// and writes those bytes to a specified location on disk.
        /// </summary>
        /// <param name="destDir">Destination directory to store the check images written to disk</param>
        /// <param name="virtualRunPath">
        ///     Optional - If provided, this represents the virtual path of a Composable DataFlow's run directory.
        ///     When this is provided, this method will create a CompAnalytics.Contracts.FileReference for each written
        ///     image file and store it on the ImageViewDataRecord.ImageData.Image property for the image.
        /// </param>
        public void WriteImagesToDisk(DirectoryInfo destDir, string virtualRunPath = null)
        {
            if (this.Document == null)
            {
                throw new InvalidOperationException("Cannot write images because no X9Document has been read yet.");
            }

            if (!destDir.Exists)
            {
                destDir.Create();
            }

            IEnumerable<X9Bundle> bundles = this.Document.Deposits
                .SelectMany(dep => dep.Bundles);
            IEnumerable<ICheckImageContainer> checks = bundles.SelectMany(bund => bund.DepositItems).Cast<ICheckImageContainer>()
                .Concat(bundles.SelectMany(bund => bund.ReturnItems).Cast<ICheckImageContainer>());
            int i = 0;
            foreach (ICheckImageContainer check in checks)
            {
                string itemType = check is X9DepositItem ? "check" : "return";
                string idxLabel = i.ToString().PadLeft(5, '0');
                string frontName = $"{itemType}-{idxLabel}-front.tif";
                string backName = $"{itemType}-{idxLabel}-back.tif";
                Queue<X9DepositItemImage> sides = new Queue<X9DepositItemImage>(check.Images);

                if (sides.Any())
                {
                    X9DepositItemImage front = sides.Dequeue();
                    WriteCheckSideImageToDisk(front, destDir, frontName, virtualRunPath);
                }
                if (sides.Any())
                {
                    X9DepositItemImage back = sides.Dequeue();
                    WriteCheckSideImageToDisk(back, destDir, backName, virtualRunPath);
                }
                i++;
            }
        }

        static void WriteCheckSideImageToDisk(X9DepositItemImage image, DirectoryInfo destDir, string fileName, string virtualRunPath)
        {
            string absPath = Path.Combine(destDir.FullName, fileName);
            X9ImageField imageField = image.ImageViewData.ImageData;

            if (File.Exists(absPath))
            {
                File.Delete(absPath);
            }
            byte[] imageData = imageField.GetImageBytes();
            if (imageData != null && imageData.Any())
            {
                using (var imageBytes = new MemoryStream(imageData))
                using (var outFile = File.OpenWrite(absPath))
                {
                    imageBytes.CopyTo(outFile);
                }

                if (virtualRunPath != null)
                {
                    Uri runRelativeUri = new Uri(StandardPaths.CombinePaths(virtualRunPath, destDir.Name, fileName), UriKind.Relative);
                    imageField.Image = FileReference.Create(absPath, runRelativeUri);
                }
            }
            else
            {
                throw new ArgumentNullException("Could not write image bytes to disk because none were found.");
            }
        }

        /// <summary>
        /// Reads through an entire byte stream comprising an X9 file, returning an X9Document fully representing the X9 file and all its contents.
        /// </summary>
        /// <returns></returns>
        public X9Document ReadX9Document()
        {
            var doc = new X9Document
            {
                Header = this.ReadNextRecord<FileHeaderRecord>()
            };

            while (this.PeekNextRecordType() == typeof(CashLetterHeaderRecord))
            {
                var deposit = new X9Deposit();
                doc.Deposits.Add(deposit);
                deposit.CashLetterHeader = this.ReadNextRecord<CashLetterHeaderRecord>();

                while (this.PeekNextRecordType() == typeof(BundleHeaderRecord))
                {
                    var bundle = new X9Bundle();
                    deposit.Bundles.Add(bundle);
                    bundle.Header = this.ReadNextRecord<BundleHeaderRecord>();

                    if (this.PeekNextRecordType() == typeof(CheckDetailRecord))
                    { // reading a normal ICL
                        do
                        {
                            var depositItem = new X9DepositItem();
                            bundle.DepositItems.Add(depositItem);
                            depositItem.CheckDetail = this.ReadNextRecord<CheckDetailRecord>();
                            if (this.PeekNextRecordType() == typeof(CheckDetailAddendumARecord))
                            {
                                depositItem.CheckDetailAddendum = this.ReadNextRecord<CheckDetailAddendumARecord>();
                            }
                            while (this.PeekNextRecordType() == typeof(ImageViewDetailRecord))
                            {
                                var image = new X9DepositItemImage();
                                depositItem.Images.Add(image);
                                image.ImageViewDetail = this.ReadNextRecord<ImageViewDetailRecord>();
                                image.ImageViewData = this.ReadNextRecord<ImageViewDataRecord>();
                            }
                        }
                        while (this.PeekNextRecordType() == typeof(CheckDetailRecord));
                    }
                    else if (this.PeekNextRecordType() == typeof(ReturnRecord))
                    { // reading an ICLr (return)
                        do
                        {
                            var returnItem = new X9ReturnItem();
                            bundle.ReturnItems.Add(returnItem);
                            returnItem.Return = this.ReadNextRecord<ReturnRecord>();
                            if (this.PeekNextRecordType() == typeof(ReturnAddendumARecord))
                            {
                                returnItem.ReturnAddendumA = this.ReadNextRecord<ReturnAddendumARecord>();
                            }
                            if (this.PeekNextRecordType() == typeof(ReturnAddendumBRecord))
                            {
                                returnItem.ReturnAddendumB = this.ReadNextRecord<ReturnAddendumBRecord>();
                            }
                            if (this.PeekNextRecordType() == typeof(ReturnAddendumDRecord))
                            {
                                returnItem.ReturnAddendumD = this.ReadNextRecord<ReturnAddendumDRecord>();
                            }
                            while (this.PeekNextRecordType() == typeof(ImageViewDetailRecord))
                            {
                                var image = new X9DepositItemImage();
                                returnItem.Images.Add(image);
                                image.ImageViewDetail = this.ReadNextRecord<ImageViewDetailRecord>();
                                image.ImageViewData = this.ReadNextRecord<ImageViewDataRecord>();
                            }
                        }
                        while (this.PeekNextRecordType() == typeof(ReturnRecord));
                    }

                    bundle.Trailer = this.ReadNextRecord<BundleTrailerRecord>();
                }

                deposit.CashLetterTrailer = this.ReadNextRecord<CashLetterTrailerRecord>();
            }

            doc.Trailer = this.ReadNextRecord<FileTrailerRecord>();

            this.Document = doc;
            return doc;
        }
    }
}
