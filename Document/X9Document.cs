using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CompAnalytics.X9.Records;

namespace CompAnalytics.X9.Document
{
    /// <summary>
    /// Represents a full X9.100-187 ICL file, consisting of a series of records with constituent fields.
    /// </summary>
    [DataContract]
    [Serializable]
    public class X9Document : X9DocumentComponent
    {
        /// <summary>
        /// File Header Record
        /// </summary>
        [DataMember]
        public FileHeaderRecord Header { get; set; }
        /// <summary>
        /// Deposits being made in this file
        /// </summary>
        [DataMember]
        public List<X9Deposit> Deposits { get; set; } = new List<X9Deposit>();
        /// <summary>
        /// File Trailer/Control Record
        /// </summary>
        [DataMember]
        public FileTrailerRecord Trailer { get; set; }

        public override List<X9Record> GetRecords()
        {
            var recs = new List<X9Record>
            {
                this.Header,
                this.Trailer
            };
            recs.AddRange(this.Deposits.Cast<X9Record>());
            return recs;
        }
    }
}
