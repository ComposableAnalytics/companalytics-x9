using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace CompAnalytics.X9
{
    [DataContract]
    [Serializable]
    public abstract class X9Record
    {
        static readonly Encoding Edcbic = Encoding.GetEncoding("IBM037");

        [Order]
        [DataMember]
        public X9TextField RecordType { get; set; }
        IEnumerable<X9Field> Fields { get; set; }

        protected X9Record()
        {
            string recordType = RecordTypes.Get(GetType());
            this.RecordType = new X9TextField(new X9FieldType("RecordType", X9FieldDataTypes.Numeric, 2));
            this.RecordType.Deserialize(recordType);
        }

        /// <summary>
        /// Returns the X9Fields within this X9Record in the order they're declared, with RecordType first.
        /// This reflects the order they appear in the record string.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<X9Field> GetFields()
        {
            if (this.Fields != null)
            {
                return this.Fields;
            }

            var results = new List<X9Field>
            {
                this.RecordType
            };

            results.AddRange(GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => typeof(X9Field).IsAssignableFrom(prop.PropertyType) && Attribute.IsDefined(prop, typeof(OrderAttribute)) && !prop.Name.Equals("RecordType"))
                .OrderBy(prop => ((OrderAttribute)prop.GetCustomAttributes(typeof(OrderAttribute), false).Single()).Order)
                .Select(prop => (X9Field)prop.GetValue(this)));

            return results;
        }

        /// <summary>
        /// Return multiple lists of X9Fields comprising this X9Record, where fixed-length fields are grouped together into single segments, while dynamic-length fields
        /// are placed in their own segments. Currently, this only supports one dynamic length field per record, which must come after all fixed-length fields in said record.
        /// 
        /// For a record with the following fields: Fixed1, Fixed2, Fixed3, Dynamic1, the resulting segments would look like [Fixed1, Fixed2, Fixed3], [Dynamic1]
        /// </summary>
        internal List<RecordByteSegment> GetSegments()
        {
            IEnumerable<X9Field> fields = this.GetFields();

            IEnumerable<X9Field> fixedLengthFields = fields.Where(f => f.FieldType.IsFixedLength);
            X9Field dynamicLengthField = fields.FirstOrDefault(f => !f.FieldType.IsFixedLength);

            var fixedSegment = new RecordByteSegment
            {
                Offset = 0,
                Length = fixedLengthFields.Sum(f => f.FieldType.Length),
                Fields = fixedLengthFields.ToList()
            };
            List<RecordByteSegment> segs = new List<RecordByteSegment>
            {
                fixedSegment
            };
            if (dynamicLengthField != null)
            {
                segs.Add(new RecordByteSegment
                {
                    Offset = fixedSegment.Offset + fixedSegment.Length.Value,
                    Fields = new List<X9Field> { dynamicLengthField }
                });
            }

            return segs;
        }

        /// <summary>
        /// Given a byte array representing this record in an X9 file, read its contents and fill in each field in this record with
        /// its value from the binary representation.
        /// </summary>
        /// <param name="record"></param>
        public void ReadRecord(byte[] record)
        {
            List<RecordByteSegment> segments = this.GetSegments();
            int offset = 0;
            foreach (RecordByteSegment segment in segments)
            {
                if (segment.Length.HasValue)
                {
                    foreach (X9Field f in segment.Fields)
                    {
                        X9TextField field = (X9TextField)f;
                        int len = field.FieldType.Length.Value;
                        string fieldStr = Edcbic.GetString(record.Skip(offset).Take(len).ToArray());
                        // Do any de-padding we might need
                        field.Deserialize(fieldStr);
                        offset += len;
                    }
                }
                else
                {
                    (this as X9DynamicLengthRecord)?.ReadDynamicField(record, offset);
                }
            }
        }

        /// <summary>
        /// Convert the values of this record and its fields to a binary representation
        /// </summary>
        public byte[] WriteRecord()
        {
            byte[] result;
            List<RecordByteSegment> segments = GetSegments();
            using (var recordStream = new MemoryStream())
            {
                foreach (RecordByteSegment segment in segments)
                {
                    foreach (X9Field field in segment.Fields)
                    {
                        byte[] fieldBytes = null;
                        if (field is X9TextField textField)
                        {
                            fieldBytes = Edcbic.GetBytes(textField.SerializedValue);
                            if (fieldBytes.Length != textField.FieldType.Length)
                            {
                                throw new IndexOutOfRangeException($"SerializedValue was too long ({fieldBytes.Length} chars) for the field ({textField.FieldType.Length} chars)");
                            }
                        }
                        else if (field is X9ImageField imageField)
                        {
                            fieldBytes = imageField.GetImageBytes();
                        }
                        else
                        {
                            throw new InvalidOperationException("Unsupported field type");
                        }
                        recordStream.Write(fieldBytes, 0, fieldBytes.Length);
                    }
                }

                result = recordStream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// Compare this record to another (usually of the same type) and return whether or not they are identical.
        /// </summary>
        public bool Equals(X9Record that)
        {
            if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                List<X9Field> thisFields = this.GetFields().ToList();
                List<X9Field> thatFields = that.GetFields().ToList();
                if (thisFields.Count() != thatFields.Count())
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < thisFields.Count(); i++)
                    {
                        X9Field thisField = thisFields[i];
                        X9Field thatField = thatFields[i];
                        if (thisField.GetType() != thatField.GetType())
                        {
                            return false;
                        }
                        else
                        {
                            if (thisField.GetType() == typeof(X9TextField))
                            {
                                if (!((X9TextField)thisField).Equals((X9TextField)thatField))
                                {
                                    return false;
                                }
                            }
                            else if (thisField.GetType() == typeof(X9ImageField))
                            {
                                if (!((X9ImageField)thisField).Equals((X9ImageField)thatField))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        public int Order { get; private set; }
        public OrderAttribute([CallerLineNumber]int order = 0)
        {
            this.Order = order;
        }
    }
}
