using System.Collections.Generic;

namespace CompAnalytics.X9
{
    internal class RecordByteSegment
    {
        /// <summary>
        /// Offset in bytes (characters) of the start of this byte segment
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// Null if this segment has a dynamic length
        /// </summary>
        public int? Length { get; set; }

        public List<X9Field> Fields { get; set; }
    }
}
