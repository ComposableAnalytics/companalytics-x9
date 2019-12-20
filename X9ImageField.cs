using CompAnalytics.Contracts;
using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9
{
    [DataContract]
    [Serializable]
    public class X9ImageField : X9Field
    {
        byte[] Value { get; set; }
        [DataMember]
        public FileReference Image { get; set; }

        public X9ImageField(X9FieldType fieldType) : base(fieldType) { }

        public void SetImageBytes(byte[] val)
        {
            this.Value = (byte[])val.Clone();
        }

        public byte[] GetImageBytes()
        {
            return this.Value;
        }

        public bool Equals(X9ImageField that)
        {
            if (this.Value != null && that.Value != null)
            {
                if (this.Value.Length != that.Value.Length)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < this.Value.Length; i++)
                    {
                        if (this.Value[i] != that.Value[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            else
            {
                // Rubber stamp if we don't have access to the files
                return true;
            }
        }

        public override string ToString()
        {
            return $"{this.FieldType.Name}";
        }
    }
}
