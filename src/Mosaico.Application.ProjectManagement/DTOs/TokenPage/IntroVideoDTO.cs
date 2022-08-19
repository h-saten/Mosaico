using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.DTOs.TokenPage
{
    public class IntroVideoDTO
    {
        public Guid Id { get; set; }
        public string VideoUrl { get; set; }
        public string VideoExternalLink { get; set; }
        public bool ShowLocalVideo { get; set; }
        public bool IsExternalLink { get; set; }
    }
}
