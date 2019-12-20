using CompAnalytics.Contracts;
using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.JPMorganAuthoring
{
    [DataContract]
    [Serializable]
    public class ImageAuthoringInfo
    {
        [DataMember]
        public DateTimeOffset ScanDate { get; set; }
        [DataMember]
        public bool IsBackOfCheck { get; set; }
        [DataMember]
        public bool CanRecreateImage { get; set; }
        [DataMember]
        public FileReference ImageFile { get; set; }
        public byte[] ImageData;
    }
}
