using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageIntroVideo
{
    public class GetPageIntroVideoQueryValidator: AbstractValidator<GetPageIntroVideoQuery>
    {
        public GetPageIntroVideoQueryValidator()
        {
            RuleFor(c => c.PageId).NotEmpty();
        }
    }
}
