using System;
using System.Runtime.Serialization;

namespace CompAnalytics.X9
{
    [DataContract]
    [Serializable]
    public class X9FieldDataType
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string SpecTypeName { get; set; }
        /// <summary>
        /// We can safely represent data stored in this type using this CLR type, but it may not be
        /// the type we'd like to use for reading and writing the value
        /// </summary>
        public Type NominalClrType { get; set; }

        [DataMember]
        public PaddingType PaddingType { get; set; } = PaddingType.None;

        internal object DeserializeToNominalType(string val)
        {
            string unpadded = this.UnpadValue(val);
            if (!string.IsNullOrWhiteSpace(unpadded))
            {
                if (this.NominalClrType == typeof(long?))
                {
                    return long.Parse(unpadded);
                }
                else
                {
                    return Convert.ChangeType(unpadded, this.NominalClrType);
                }
            }
            else
            {
                return null;
            }
        }

        internal string SerializeFromNominalType(object val, X9FieldType fieldType)
        {
            int length = fieldType.Length.Value;
            string unpadded = Convert.ToString(val) ?? "";
            if (unpadded.Length > length)
            {
                throw new ArgumentException($"Cannot set field {fieldType.Name} value to '{unpadded}'. The length of the data " +
                    $"({unpadded.Length}) exceeds the available length for this field ({length}).");
            }
            return this.PadValue(unpadded, length);
        }

        internal string UnpadValue(string serialized)
        {
            string result;
            switch (this.PaddingType)
            {
                case PaddingType.BlankLeft:
                case PaddingType.BlankRight:
                    result = serialized.Trim();
                    break;
                case PaddingType.ZeroLeft:
                    result = long.Parse(serialized).ToString();
                    break;
                default:
                    result = serialized;
                    break;
            }
            return result;
        }

        internal string PadValue(string deserialized, int length)
        {
            // TODO: Throw if string is longer than desired
            string result;
            switch (this.PaddingType)
            {
                case PaddingType.BlankLeft:
                    result = deserialized.PadLeft(length);
                    break;
                case PaddingType.BlankRight:
                    result = deserialized.PadRight(length);
                    break;
                case PaddingType.ZeroLeft:
                    result = deserialized.PadLeft(length, '0');
                    break;
                default:
                    result = deserialized;
                    break;
            }
            return result;
        }
    }

    public static class X9FieldDataTypes
    {
        public static readonly X9FieldDataType Alphabetic = new X9FieldDataType
        {
            Name = "Alphabetic",
            SpecTypeName = "A",
            NominalClrType = typeof(string),
            PaddingType = PaddingType.BlankRight
        };
        public static readonly X9FieldDataType Numeric = new X9FieldDataType
        {
            Name = "Numeric",
            SpecTypeName = "N",
            NominalClrType = typeof(long?),
            PaddingType = PaddingType.ZeroLeft
        };
        public static readonly X9FieldDataType Blank = new X9FieldDataType
        {
            Name = "Blank",
            SpecTypeName = "B",
            NominalClrType = typeof(string),
            PaddingType = PaddingType.BlankRight
        };
        public static readonly X9FieldDataType SpecialCharacters = new X9FieldDataType
        {
            Name = "SpecialCharacters",
            SpecTypeName = "S",
            NominalClrType = typeof(string),
            PaddingType = PaddingType.BlankRight
        };
        public static readonly X9FieldDataType Alphameric = new X9FieldDataType
        {
            Name = "Alphameric",
            SpecTypeName = "AN",
            NominalClrType = typeof(string),
            PaddingType = PaddingType.BlankRight
        };
        public static readonly X9FieldDataType AlphamericSpecial = new X9FieldDataType
        {
            Name = "AlphamericSpecial",
            SpecTypeName = "ANS",
            NominalClrType = typeof(string),
            PaddingType = PaddingType.BlankRight
        };
        public static readonly X9FieldDataType NumericBlank = new X9FieldDataType
        {
            Name = "NumericBlank",
            SpecTypeName = "NB",
            NominalClrType = typeof(long?),
            PaddingType = PaddingType.BlankRight
        };
        public static readonly X9FieldDataType NumericSpecial = new X9FieldDataType
        {
            Name = "NumericSpecial",
            SpecTypeName = "NS",
            NominalClrType = typeof(string),
            PaddingType = PaddingType.BlankRight
        };
        public static readonly X9FieldDataType NumericBlankSpecialMicr = new X9FieldDataType
        {
            Name = "NumericBlankSpecialMicr",
            SpecTypeName = "NBSM",
            NominalClrType = typeof(string),
            PaddingType = PaddingType.BlankLeft
        };
        public static readonly X9FieldDataType NumericBlankSpecialMicrOnUs = new X9FieldDataType
        {
            Name = "NumericBlankSpecialMicrOnUs",
            SpecTypeName = "NB",
            NominalClrType = typeof(string),
            PaddingType = PaddingType.BlankLeft
        };
        public static readonly X9FieldDataType Binary = new X9FieldDataType
        {
            Name = "Binary",
            SpecTypeName = "Binary"
        };
    }

    public enum PaddingType
    {
        None,
        /// <summary>
        /// Left-justify, pad with spaces on the right
        /// </summary>
        BlankRight,
        /// <summary>
        /// Right-justify, pad with spaces on the left
        /// </summary>
        BlankLeft,
        /// <summary>
        /// Right-justify, pad with zeroes on the right
        /// </summary>
        ZeroLeft
    }
}
