using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class UpdatePageDTO
    {
        public Dictionary<string, string> ShortDescription { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string CoverColor { get; set; }
        public Dictionary<string, List<SocialMediaLinkDTO>> SocialMediaLinks { get; set; }
    }
}