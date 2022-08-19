using System;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.ProjectManagement.DTOs.TokenPage
{
    public class PageReviewDTO
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PageReviewCategory Category { get; set; }
        
        public string Link { get; set; }
        public bool IsHidden { get; set; }
        public Guid Id { get; set; }
    }
}