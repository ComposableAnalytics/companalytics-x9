using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9
{
    [DataContract]
    [Serializable]
    public abstract class X9DynamicLengthRecord : X9Record
    {
        public X9DynamicLengthRecord() : base() { }

        public X9TextField DynamicFieldLengthField { get; set; }
        public X9ImageField DynamicField { get; set; }

        internal void ReadDynamicField(byte[] record, int offset)
        {
            long len = (long)this.DynamicFieldLengthField.NominalValue;
            byte[] slice = new byte[len];
            Array.Copy(record, offset, slice, 0, len);
            this.DynamicField.SetImageBytes(slice);
        }
    }
}
