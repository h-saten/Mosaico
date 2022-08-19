using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageFaq
{
    public class GetPageFaqQueryResponse
    {
        public List<FaqDTO> Faqs { get; set; }
    }
}