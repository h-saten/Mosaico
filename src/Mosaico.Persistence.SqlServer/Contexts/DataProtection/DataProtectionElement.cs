using System;

namespace Mosaico.Persistence.SqlServer.Contexts.DataProtection
{
    public class DataProtectionElement
    {
        public DataProtectionElement()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Xml { get; set; }
    }
}