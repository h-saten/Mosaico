using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mosaico.Persistence.SqlServer.Contexts.DataProtection
{
    public class SqlXmlRepository : IXmlRepository
    {
        private readonly DataProtectionDbContext _dataProtectionDbContext;

        public SqlXmlRepository(DataProtectionDbContext context)
        {
            _dataProtectionDbContext = context;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            _dataProtectionDbContext.Database.EnsureCreated();
            var pendingMigrations =  _dataProtectionDbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                _dataProtectionDbContext.Database.Migrate();
            }

            return new ReadOnlyCollection<XElement>(_dataProtectionDbContext.DataProtectionXmlElements.Select(x => XElement.Parse(x.Xml)).ToList());
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            _dataProtectionDbContext.Database.EnsureCreated();
            var pendingMigrations =  _dataProtectionDbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                _dataProtectionDbContext.Database.Migrate();
            }
            
            _dataProtectionDbContext.DataProtectionXmlElements.Add(
                new DataProtectionElement
                {
                    Xml = element.ToString(SaveOptions.DisableFormatting)
                }
            );

            _dataProtectionDbContext.SaveChanges();
        }
    }
}