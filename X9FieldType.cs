using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace CompAnalytics.X9
{
    [DataContract]
    [Serializable]
    public class X9FieldType
    {
        static readonly string DateFormat = "yyyyMMdd";
        static readonly string TimeFormat = "hhmm";

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public X9FieldDataType DataType { get; set; }
        /// <summary>
        /// The type we'd prefer to use when getting/setting the value. Can be null.
        /// </summary>
        public Type InteropClrType { get; set; }
        [DataMember]
        public int? Length { get; set; }
        [DataMember]
        public bool IsFixedLength { get; private set; }

        public X9FieldType(string name, X9FieldDataType dataType, int? length = null, Type interopType = null)
        {
            Name = name;
            DataType = dataType;
            Length = length;
            IsFixedLength = Length.HasValue;
            InteropClrType = interopType;
        }

        internal object ConvertVal(object value, Type fromType, Type toType)
        {
            if (fromType == toType)
            {
                return value;
            }
            else if (fromType == typeof(long?) && toType == typeof(DateTimeOffset))
            {
                if ((long?)value == 0)
                {
                    return null;
                }
                else
                {
                    string str = Convert.ToString(value).PadLeft(Length.Value, '0');
                    return new DateTimeOffset(DateTime.ParseExact(str, DateFormat, CultureInfo.InvariantCulture.DateTimeFormat), TimeSpan.FromHours(0));
                }
            }
            else if (toType == typeof(long?) && fromType == typeof(DateTimeOffset))
            {
                DateTimeOffset dt = (DateTimeOffset)value;
                return int.Parse(dt.ToString(DateFormat));
            }

            else if (fromType == typeof(long?) && toType == typeof(TimeSpan))
            {
                if ((long?)value == 0)
                {
                    return null;
                }
                else
                {
                    string str = Convert.ToString(value).PadLeft(Length.Value, '0');
                    return TimeSpan.ParseExact(str, TimeFormat, CultureInfo.InvariantCulture);
                }
            }
            else if (toType == typeof(long?) && fromType == typeof(TimeSpan))
            {
                TimeSpan ts = (TimeSpan)value;
                return int.Parse(ts.ToString(TimeFormat));
            }

            else if (fromType == typeof(long?) && toType == typeof(bool?))
            {
                if ((long?)value == 0)
                {
                    return false;
                }
                else if ((long?)value == 1)
                {
                    return true;
                }
                else
                {
                    throw new ArgumentException("Invalid boolean value.");
                }
            }
            else if (toType == typeof(long?) && fromType == typeof(bool?))
            {
                bool val = (bool)value;
                return val ? 1 : 0;
            }
            else
            {
                if (fromType == typeof(long?) && toType != typeof(long?))
                {
                    long? nullableVal = (long?)value;
                    if (nullableVal.HasValue)
                    {
                        return Convert.ChangeType(nullableVal.Value, toType);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return Convert.ChangeType(value, toType);
                }
            }
        }
    }
}
