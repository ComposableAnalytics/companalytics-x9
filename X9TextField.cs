using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9
{
    [DataContract]
    [Serializable]
    public class X9TextField : X9Field
    {
        [DataMember]
        public object NominalValue { get; private set; }
        [DataMember]
        object Value { get; set; }
        string serializedValue;
        [DataMember]
        public string SerializedValue
        {
            get
            {
                return this.serializedValue ?? this.FieldType.DataType.SerializeFromNominalType(this.NominalValue, this.FieldType);
            }
            private set
            {
                this.serializedValue = value;
            }
        }
        Type InteropClrType => this.FieldType.InteropClrType ?? this.FieldType.DataType.NominalClrType;

        public X9TextField(X9FieldType fieldType) : base(fieldType) { }

        public void SetValue(object obj)
        {
            this.SetValueInternal(obj);
        }

        void SetValueInternal(object obj)
        {
            object nominalVal = this.FieldType.ConvertVal(obj, this.InteropClrType, this.FieldType.DataType.NominalClrType);
            string serializedVal = this.FieldType.DataType.SerializeFromNominalType(nominalVal, this.FieldType);

            this.Value = obj;
            this.NominalValue = nominalVal;
            this.SerializedValue = serializedVal;
        }

        public void SetValue(DateTimeOffset date)
        {
            if (this.FieldType.InteropClrType == typeof(DateTimeOffset))
            {
                this.SetValueInternal(new DateTimeOffset(date.Date, date.Offset));
            }
            else if (this.FieldType.InteropClrType == typeof(TimeSpan))
            {
                this.SetValueInternal(date.TimeOfDay);
            }
            else
            {
                throw new InvalidOperationException("Cannot set the value of a field using a DateTimeOffset unless its interop CLR type is DateTimeOffset or TimeSpan");
            }
        }

        public void Deserialize(string val)
        {
            this.SerializedValue = val;
            this.NominalValue = this.FieldType.DataType.DeserializeToNominalType(val);
            this.Value = this.FieldType.ConvertVal(this.NominalValue, this.FieldType.DataType.NominalClrType, this.InteropClrType);
        }

        public bool Equals(X9TextField that)
        {
            return this.SerializedValue == that.SerializedValue;
        }

        public override string ToString()
        {
            return $"{this.FieldType.Name}, {this.NominalValue}";
        }
    }
}
