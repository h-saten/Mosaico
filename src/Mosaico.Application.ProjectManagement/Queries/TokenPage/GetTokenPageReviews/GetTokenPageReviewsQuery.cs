using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPageReviews
{
    public class GetTokenPageReviewsQuery : IRequest<List<PageReviewDTO>>
    {
        public Guid PageId { get; set; }
    }
}