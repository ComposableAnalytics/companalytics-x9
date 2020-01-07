# CompAnalytics.X9

[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/ComposableAnalytics/companalytics-x9?color=%230a0&label=GitHub)](https://github.com/ComposableAnalytics/companalytics-x9) [![Nuget version](https://img.shields.io/nuget/v/CompAnalytics.X9?label=Nuget)](https://www.nuget.org/packages/CompAnalytics.X9/) [![Nuget downloads](https://img.shields.io/nuget/dt/CompAnalytics.X9?color=blue&label=Nuget)](https://www.nuget.org/packages/CompAnalytics.X9/) [![API Specification](https://img.shields.io/badge/API-%0ASpec-ff69b4)](https://dev.composable.ai/api/CompAnalytics.X9.html)

This library contains classes that represent, read, and write binary X9.100-187 image cash letter files used for electronic transmission of checks to/from a bank. It includes tools for reading the files from disk into an `X9Document` representation that mimics the structure of the file's various records and fields in friendly, interoperable types. `X9Document`s can also be created from scratch, then written out to the binary X9 format, enabling simple X9 file creation for your .NET environment use case. All types also offer robust type coercion, bounds-checking for each fixed-length field, comparison utilities to ensure equality across files, and informative exception messages when attempting to violate the X9.100-187 specification. The library is being used in production for transmission of millions of dollars by [Composable Analytics](https://composable.ai).

## Authors

1. Ryan O'Shea [https://ryanoshea.com](https://ryanoshea.com) for [Composable Analytics](https://composable.ai), from Oct. 2019 to Jan. 2020.

## API Specification

Full public API specifications can be [found here](https://dev.composable.ai/api/CompAnalytics.X9.html).

## Source

Full source is [available on GitHub](https://github.com/ComposableAnalytics/companalytics-x9). You may encounter problems trying to build as the csproj found the repository is the one we use internally at Composable, so you'll need to manually resolve DLL dependencies for `CompAnalytics.*` libraries.

## Supported Record Types

The following X9 file record types are fully supported for reading and writing.

| Record                         | Class Name                 |  Record Type Code |
|--------------------------------|----------------------------|-------------------|
| File Header Record             | FileHeaderRecord           | 01                |
| Cash Letter Header Record      | CashLetterHeaderRecord     | 10                |
| Bundle Header Record           | BundleHeaderRecord         | 20                |
| Check Detail Record            | CheckDetailRecord          | 25                |
| Check Detail Addendum A Record | CheckDetailAddendumARecord | 26                |
| Return Record                  | ReturnRecord               | 31                |
| Return Addendum A Record       | ReturnAddendumARecord      | 32                |
| Return Addendum B Record       | ReturnAddendumBRecord      | 33                |
| Return Addendum D Record       | ReturnAddendumDRecord      | 35                |
| Image View Detail Record       | ImageViewDetailRecord      | 50                |
| Image View Data Record*        | ImageViewDataRecord        | 52                |
| Bundle Trailer Record          | BundleTrailerRecord        | 70                |
| Cash Letter Trailer Record     | CashLetterTrailerRecord    | 90                |
| File Trailer Record            | FileTrailerRecord          | 99                |

\* See [Limitations](#Limitations)

## Examples

### Reading an X9 File

The following example reads an X9 file from disk and extracts the embedded check images to TIFFs on disk.

```csharp
string path = @"C:\Temp\sample.x9";
string imageOutDir = @"C:\Temp\SampleCheckImages";
using (Stream x9File = File.OpenRead(path))
using (X9Reader reader = new X9Reader(x9File))
{
    doc = reader.ReadX9Document();
    reader.WriteImagesToDisk(imageOutDir);
}
```

### Authoring an X9Document

Below is an example of producing an (incomplete) `X9Document` by creating most of the required records and setting a few field values. This example leaves out a few fields and may contain a few mistakes, but the general pattern should be apparent at least. Each X9Document starts completely empty, and each `X9Record` or additional `X9DocumentComponent` must be initialized and filled out.

This example authors a normal ICL file. For an ICLr return file, simply populate the `ReturnItems` within each `X9Bundle` rather than the `DepositItems`. 

```csharp
public static X9Document Create(List<CheckInfo> checks)
{
    DateTimeOffset createTime = DateTimeOffset.Now;
    DateTimeOffset busDate = DateTimeOffset.Now;
    string custId = "938476";
    int numRecords = 0;
    int numItems = 0;
    decimal totalAmount = 0;

    var doc = new X9Document
    {
        Header = new FileHeaderRecord(),
        Trailer = new FileTrailerRecord()
    };
    numRecords += 2;

    // Configure header
    doc.Header.TestFileIndicator.SetValue(true);
    doc.Header.ImmediateOriginRoutingNumber.SetValue("044000037");
    doc.Header.FileCreationDate.SetValue(createTime); // The same DateTimeOffset can be passed into a date & time
    doc.Header.FileCreationTime.SetValue(createTime); // field separately, and the appropriate values will be set
    doc.Header.ResendIndicator.SetValue(false);
    doc.Header.ImmediateOriginName.SetValue("MY COMPANY");

    // Create a single deposit
    var dep = new X9Deposit
    {
        CashLetterHeader = new CashLetterHeaderRecord(),
        CashLetterTrailer = new CashLetterTrailerRecord()
    };
    numRecords += 2;

    dep.CashLetterHeader.CashLetterBusinessDate.SetValue(busDate);
    dep.CashLetterHeader.UniqueCustomerIdentifier.SetValue(custId);
    dep.CashLetterHeader.CashLetterCreationDate.SetValue(createTime);
    dep.CashLetterHeader.CashLetterCreationTime.SetValue(createTime);

    // Create a single bundle
    int bundleIdx = 0;
    decimal depositTotalAmount = 0;
    int depositNumItems = 0;
    int depositNumImages = 0;
        
    var bundle = new X9Bundle
    {
        Header = new BundleHeaderRecord(),
        Trailer = new BundleTrailerRecord()
    };
    numRecords += 2;

    bundle.Header.UniqueCustomerIdentifier.SetValue(custId);
    bundle.Header.BundleBusinessDate.SetValue(busDate);
    bundle.Header.BundleCreationDate.SetValue(createTime);
    bundle.Header.BundleSequenceNumber.SetValue(bundleIdx);

    // Create a few checks based on the ones passed in
    int bundleNumImages = 0;
    decimal bundleTotalAmount = 0;
    foreach (CheckInfo depositItemInfo in checks)
    {
        var depItem = new X9DepositItem()
        {
            CheckDetail = new CheckDetailRecord()
        };
        numRecords += 1;

        depItem.CheckDetail.Amount.SetValue(depositItemInfo.Amount);
        // You'd also set the various MICR fields here

        // Add images for the check

        // Front first
        var frontImageRecord = new X9DepositItemImage()
        {
            ImageViewDetail = new ImageViewDetailRecord(),
            ImageViewData = new ImageViewDataRecord()
        };
        numRecords += 2;
        byte[] frontImageBytes;
        using (FileStream frontImageFile = File.OpenRead(depositItemInfo.FrontImagePath))
        {
            frontImageBytes = new byte[frontImageFile.Length];
            using (MemoryStream byteStream = new MemoryStream(frontImageBytes))
            {
                frontImageFile.CopyTo(byteStream);
            }
        }
        frontImageRecord.ImageViewDetail.ViewSideIndicator.SetValue(0); // front
        frontImageRecord.ImageViewDetail.ImageRecreateIndicator.SetValue(1);
        frontImageRecord.ImageViewData.BundleBusinessDate.SetValue(busDate);
        frontImageRecord.ImageViewData.ImageData.SetImageBytes(frontImageBytes);
        frontImageRecord.ImageViewData.LengthOfImageData.SetValue(frontImageBytes.Length);
        depItem.Images.Add(frontImageRecord);
        bundleNumImages++;

        // Back
        var backImageRecord = new X9DepositItemImage()
        {
            ImageViewDetail = new ImageViewDetailRecord(),
            ImageViewData = new ImageViewDataRecord()
        };
        numRecords += 2;
        byte[] backImageBytes;
        using (FileStream backImageFile = File.OpenRead(depositItemInfo.BackImagePath))
        {
            backImageBytes = new byte[backImageFile.Length];
            using (MemoryStream byteStream = new MemoryStream(backImageBytes))
            {
                backImageFile.CopyTo(byteStream);
            }
        }
        backImageRecord.ImageViewDetail.ViewSideIndicator.SetValue(0); // front
        backImageRecord.ImageViewDetail.ImageRecreateIndicator.SetValue(1);
        backImageRecord.ImageViewData.BundleBusinessDate.SetValue(busDate);
        backImageRecord.ImageViewData.ImageData.SetImageBytes(backImageBytes);
        backImageRecord.ImageViewData.LengthOfImageData.SetValue(backImageBytes.Length);
        depItem.Images.Add(backImageRecord);
        bundleNumImages++;

        bundle.DepositItems.Add(depItem);
        bundleTotalAmount += depositItemInfo.Amount;
    }

    // Configure bundle trailer
    bundle.Trailer.ItemsWithinBundleCount.SetValue(bundle.DepositItems.Count);
    bundle.Trailer.ImagesWithinBundleCount.SetValue(bundleNumImages);
    bundle.Trailer.MicrValidTotalAmount.SetValue(bundleTotalAmount);

    dep.Bundles.Add(bundle);
    bundleIdx++;
    depositTotalAmount += bundleTotalAmount;
    depositNumItems += bundle.DepositItems.Count;
    depositNumImages += bundleNumImages;

    // Configure cash letter trailer
    dep.CashLetterTrailer.BundleCount.SetValue(bundleIdx);
    dep.CashLetterTrailer.ImagesWithinCashLetterCount.SetValue(depositNumImages);
    dep.CashLetterTrailer.ItemsWithinCashLetterCount.SetValue(depositNumItems);
    dep.CashLetterTrailer.CashLetterTotalAmount.SetValue(depositTotalAmount);

    numItems += depositNumItems;
    totalAmount += depositTotalAmount;

    doc.Deposits.Add(dep);

    // Configure file trailer
    doc.Trailer.CashLetterCount.SetValue(doc.Deposits.Count);
    doc.Trailer.TotalRecordCount.SetValue(numRecords);
    doc.Trailer.TotalFileAmount.SetValue(totalAmount);
    doc.Trailer.TotalItemCount.SetValue(numItems);

    return doc;
}
```

### Writing an X9 File

This example uses an `X9Writer` to write the contents of an `X9Document` to a binary X9 file on disk.

```csharp
X9Document doc = ...; // create this using example above
string outFilePath = @"C:\Temp\example.x9";
using (X9Writer writer = new X9Writer(doc))
using (MemoryStream byteStream = new MemoryStream(writer.WriteX9Document()))
using (FileStream x9FileStream = File.Create(outFilePath))
{
    byteStream.CopyTo(x9FileStream);
}
```

## Limitations

This library was originally developed in 2019 during an accounts-receivable automation project undertaken by Composable Analytics, Inc., where the destination institution was JPMorgan Chase. As a result, the library has the following limitations:

1. Only the X9.100-187 file standard is supported. Specifically, the library was modeled after JPMorgan's Merchant ICL Deposit specifications v4, R7.
1. Some classes, notably the various classes modeling each type of record in the X9 file (`X9Record`s), contain a method for quickly setting the values of some fields based on a limited set of arguments and populating other fields with JPMorgan's defaults. There are also `DataContract` classes under `CompAnalytics.X9.JPMorganAuthoring` that specifically model only the non-static fields needed when sending these files to JPMorgan. While these have been left in for compatibility with our original use case, these specialized classes & methods can be safely ignored and the library can be used for general-purpose X9 file creation.
1. To reduce complexity, the library only supports one dynamic-length field per record. As a result, the Image View Data Record (`Records.ImageViewDatRecord`) supports arbitrary-length image data (Field 19), but the other dynamic-length fields in that record are not supported and must be left empty to produce a valid file. This includes Field 17, the digital signature, and Field 15, the image reference key. The corresponding length fields for these two fields (14 and 16, respectively), must also be left at their default value, `0`, indicating that the fields both have a length of zero bytes.
1. Images are provided to the `ImageViewDataRecord` by supplying a byte[] representing the image file. These must be from TIFF images compressed with CCITT Group 4. Check your institution for image dimension and resolution requirements.

Additionally, the library takes a dependency on Entity Framework due to its reliance on `CompAnalytics.Contracts`, our base assembly for all `DataContracts` sent over the wire in the [Composable](https://composable.ai) data analytics platform, of which this library is a component. We would like to remove this dependency, but we currently aren't able to. None of the Entity Framework types referenced by `CompAnalytics.Contracts` are used by `CompAnalytics.X9`, so as long as you can include the EF dependency and resolve any type load issues, you won't need to worry about runtime errors caused by version mismatches.
