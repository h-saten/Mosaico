using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectVisitors
{
    public class GetProjectVisitorsQueryValidator: AbstractValidator<GetProjectVisitorsQuery>
    {
        public GetProjectVisitorsQueryValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty();
        }
    }
}
