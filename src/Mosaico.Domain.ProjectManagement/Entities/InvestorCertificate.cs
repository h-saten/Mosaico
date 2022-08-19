using System;
using System.Collections.Generic;
using System.Linq;
using Mosaico.Domain.Base;
using Mosaico.Domain.ProjectManagement.Models.CertificateGenerator;
using Newtonsoft.Json;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class InvestorCertificate : EntityBase
    {
        public string ConfigurationJson { get; private set; }
        public DateTimeOffset? SendingEnabledAt { get; private set; }
        public virtual List<InvestorCertificateBackground> Backgrounds { get; set; } = new();
        public virtual Project Project { get; set; }
        public Guid ProjectId { get; set; }
        
        public InvestorCertificate() {}
        
        public InvestorCertificate(Guid projectId, CertificateConfiguration configuration = null)
        {
            ProjectId = projectId;
            if (configuration == null)
            {
                ConfigurationJson = JsonConvert.SerializeObject(new DefaultCertificateConfiguration());
            }
            else
            {
                UpdateConfiguration(configuration);
            }
        }

        public void UpdateConfiguration(CertificateConfiguration configuration)
        {
            ConfigurationJson = JsonConvert.SerializeObject(configuration);
        }

        public InvestorCertificateBackground AddBackground(string language, string url)
        {
            var backgroundEntry = Backgrounds.SingleOrDefault(x => x.Language == language);

            if (backgroundEntry is null)
            {
                backgroundEntry = new InvestorCertificateBackground
                {
                    Language = language,
                    Url = url,
                    InvestorCertificateId = Id
                };
                Backgrounds.Add(backgroundEntry);
            }
            else
            {
                backgroundEntry.Url = url;
            }
            
            return backgroundEntry;
        }

        public void EnableSending()
        {
            if (SendingEnabledAt is null)
            {
                SendingEnabledAt = DateTimeOffset.UtcNow;
            }
        }

        public void DisableSending()
        {
            if (SendingEnabledAt is not null)
            {
                SendingEnabledAt = null;
            }
        }

        public bool IsSendingEnabled()
        {
            return SendingEnabledAt is not null;
        }

        public CertificateConfiguration GetConfiguration()
        {
            return JsonConvert.DeserializeObject<CertificateConfiguration>(ConfigurationJson);
        }
        
        public string GetBackgroundPath(string language)
        {
            return Backgrounds
                .Where(x => x.Language == language)
                .Select(x => x.Url)
                .SingleOrDefault();
        }
    }
}