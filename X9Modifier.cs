using System;
using System.IO;
using System.Text;

namespace CompAnalytics.X9
{
    public class X9Modifier : IDisposable
    {
        protected static readonly int RecordSizeBytes = 4;
        protected static readonly int RecordTypeBytes = 2;
        protected static readonly Encoding Edcbic = Encoding.GetEncoding("IBM037");

        protected Stream ByteStream { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ByteStream != null)
                {
                    this.ByteStream.Dispose();
                    this.ByteStream = null;
                }
            }
        }
    }
}
