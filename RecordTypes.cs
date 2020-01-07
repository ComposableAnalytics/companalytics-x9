using System;
using System.Collections.Generic;
using System.Linq;
using CompAnalytics.X9.Records;

namespace CompAnalytics.X9
{
    internal static class RecordTypes
    {
        static readonly Dictionary<Type, string> Lut = new Dictionary<Type, string>
        {
            { typeof(FileHeaderRecord), "01" },
            { typeof(CashLetterHeaderRecord), "10" },
            { typeof(BundleHeaderRecord), "20" },
            { typeof(CheckDetailRecord), "25" },
            { typeof(CheckDetailAddendumARecord), "26" },
            { typeof(ReturnRecord), "31" },
            { typeof(ReturnAddendumARecord), "32" },
            { typeof(ReturnAddendumBRecord), "33" },
            { typeof(ReturnAddendumDRecord), "35" },
            { typeof(ImageViewDetailRecord), "50" },
            { typeof(ImageViewDataRecord), "52" },
            { typeof(BundleTrailerRecord), "70" },
            { typeof(CashLetterTrailerRecord), "90" },
            { typeof(FileTrailerRecord), "99" }
        };

        public static string Get(Type type)
        {
            return Lut[type];
        }

        public static Type GetByCode(string recordTypeStr)
        {
            return Lut.Where(kvp => kvp.Value.Equals(recordTypeStr)).Select(kvp => kvp.Key).FirstOrDefault();
        }

        public static X9Record Create(string recordTypeStr)
        {
            Type recordType = Lut.First(kvp => kvp.Value.Equals(recordTypeStr)).Key;
            return (X9Record)Activator.CreateInstance(recordType);
        }
    }
}
