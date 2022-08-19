using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ArticleNotFoundException : ExceptionBase
    {
        public ArticleNotFoundException(string articleId): base($"There is no article with ${articleId} id")
        {

        }
        public override string Code => Constants.ErrorCodes.ArticleNotFound;

        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}
