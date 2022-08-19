using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class ArticleDTO
    {
        public string VisibleText { get; set; }
        public string AuthorPhoto { get; set; }
        public string CoverPicture { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string Link { get; set; }
        public string Id { get; set; }
        public string AuthorName { get; set; }
        public bool Hidden { get; set; }
    }
}