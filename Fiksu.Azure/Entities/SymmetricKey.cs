using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace Fiksu.Azure.Entities
{
    public class SymmetricKey : TableEntity
    {
        public string CertificateThumbprint { get; set; }
        public DateTime CreateDate { get; set; }
        public byte[] Key { get; set; }
        public byte[] iv { get; set; }

        public int Version
        {
            get => int.Parse(RowKey);
            set => RowKey = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
