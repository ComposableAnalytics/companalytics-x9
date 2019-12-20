using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CompAnalytics.X9.Document
{
    /// <summary>
    /// Base class for a structure that contains a certain collection of X9Records (or other X9DocumentComponents).
    /// These model the exact structure of an X9 file and its parts by representing the relationship among the various
    /// X9Records within the file.
    /// </summary>
    [DataContract]
    [Serializable]
    [KnownType(typeof(X9Document))]
    [KnownType(typeof(X9Deposit))]
    [KnownType(typeof(X9Bundle))]
    [KnownType(typeof(X9DepositItem))]
    [KnownType(typeof(X9DepositItemImage))]
    public abstract class X9DocumentComponent
    {
        /// <summary>
        /// Returns a list of all X9Records within this X9DocumentComponent. Order is not guaranteed.
        /// </summary>
        /// <returns></returns>
        public abstract List<X9Record> GetRecords();

        /// <summary>
        /// Compares two X9DocumentComponents and returns true if they are of the same type and contain the same contents.
        /// </summary>
        public bool Equals(X9DocumentComponent that)
        {
            if (this.GetType() != that.GetType())
            {
                return false;
            }
            else
            {
                List<X9Record> thisRecords = this.GetRecords();
                List<X9Record> thatRecords = that.GetRecords();
                if (thisRecords.Count != thatRecords.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < thisRecords.Count; i++)
                    {
                        X9Record thisRecord = thisRecords[i];
                        X9Record thatRecord = thatRecords[i];
                        if (!thisRecord.Equals(thatRecord))
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }
    }
}
