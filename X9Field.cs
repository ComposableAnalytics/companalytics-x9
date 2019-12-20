using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9
{
    [DataContract]
    [Serializable]
    [KnownType(typeof(X9TextField))]
    [KnownType(typeof(X9ImageField))]
    public class X9Field
    {
        public X9FieldType FieldType { get; set; }

        public X9Field(X9FieldType fieldType)
        {
            this.FieldType = fieldType;
        }
    }
}
