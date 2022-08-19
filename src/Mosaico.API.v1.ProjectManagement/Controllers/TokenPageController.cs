using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.DeleteArticle;
using Mosaico.Application.ProjectManagement.Commands.DeleteInvestmentPackage;
using Mosaico.Application.ProjectManagement.Commands.DisplayArticle;
using Mosaico.Application.ProjectManagement.Commands.HideArticle;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateFaq;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectPartner;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectTeamMember;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteFaq;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.DeletePageReview;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteProjectPartner;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteProjectTeamMember;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.UpdateProjectIntroVideoUrl;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.UploadProjectIntroVideo;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.UpsertAbout;
using Mosaico.Application.ProjectManagement.Commands.TokenPage.UpsertPageReview;
using Mosaico.Application.ProjectManagement.Commands.UpdatePage;
using Mosaico.Application.ProjectManagement.Commands.UpsertInvestmentPackage;
using Mosaico.Application.ProjectManagement.Commands.UpsertProjectArticles;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Application.ProjectManagement.Queries.GetArticles;
using Mosaico.Application.ProjectManagement.Queries.GetInvestmentPackages;
using Mosaico.Application.ProjectManagement.Queries.GetProjectPartners;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetAbout;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageFaq;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageForUpdate;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageIntroVideo;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetProjectTeamMembers;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPage;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPageReviews;
using Mosaico.Authorization.Base;
using Constants = Mosaico.Base.Constants;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/pages")]
    [Route("api/v{version:apiVersion}/pages")]
    public class TokenPageController : ControllerBase
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMediator _mediator;

        public TokenPageController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResult<PageDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPageAsync([FromRoute] Guid id, string language = Mosaico.Base.Constants.Languages.English)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = _currentUserContext.Language;
            }

            var response = await _mediator.Send(new GetTokenPageQuery
            {
                Id = id,
                Language = language
            });
            return new SuccessResult(response);
        }

        #region team members
        [HttpPost("{id}/team-member")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateUpdateProjectTeamMember([FromRoute] Guid id,
            [FromBody] CreateUpdateProjectTeamMemberCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.PageId = id;

            var memberId = await _mediator.Send(command);

            return new SuccessResult(memberId) { StatusCode = 201 };
        }

        [HttpDelete("{id}/team-member/{memberId}")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteProjectTeamMember([FromRoute] Guid id, [FromRoute] Guid memberId)
        {
            await _mediator.Send(new DeleteProjectTeamMemberCommand
            {
                Id = memberId,
                PageId = id
            });
            return new SuccessResult(true);
        }


        [HttpGet("{id}/team-members")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectTeamMembersQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProjectTeamMember([FromRoute] Guid id)
        {
            if (id == Guid.Empty) throw new InvalidParameterException(nameof(id));
            var response = await _mediator.Send(new GetProjectTeamMembersQuery { PageId = id });
            return new SuccessResult(response);
        }
        #endregion

        #region partners
        [HttpPost("{id}/partner")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateUpdateProjectPartner([FromRoute] Guid id,
            [FromBody] CreateUpdateProjectPartnerCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.PageId = id;

            var partnerId = await _mediator.Send(command);

            return new SuccessResult(partnerId) { StatusCode = 201 };
        }

        [HttpDelete("{id}/partner/{partnerId}")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteProjectPartner([FromRoute] Guid id, [FromRoute] Guid partnerId)
        {
            await _mediator.Send(new DeleteProjectPartnerCommand
            {
                Id = partnerId,
                PageId = id
            });
            return new SuccessResult(true);
        }

        [HttpGet("{id}/partners")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectPartnersQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProjectPartners([FromRoute] Guid id)
        {
            if (id == Guid.Empty) throw new InvalidParameterException(nameof(id));
            var response = await _mediator.Send(new GetProjectPartnersQuery { PageId = id });
            return new SuccessResult(response);
        }
        #endregion

        [HttpGet("{id}/faq")]
        [ProducesResponseType(typeof(SuccessResult<GetPageFaqQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetFaqs([FromRoute] Guid id, [FromQuery] string language = null)
        {
            if (string.IsNullOrWhiteSpace(language))
                language = _currentUserContext.Language;

            var response = await _mediator.Send(new GetPageFaqQuery
            {
                Language = language,
                PageId = id
            });

            return new SuccessResult(response);
        }

        [HttpPut("{id}/faq/{faqId}")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdateFaq([FromRoute] Guid id, [FromRoute] Guid faqId,
            [FromBody] CreateUpdateFaqCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.Language)) command.Language = _currentUserContext.Language;
            command.PageId = id;
            command.Id = faqId;

            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }

        [HttpPost("{id}/faq")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> CreateFaq([FromRoute] Guid id, [FromBody] CreateUpdateFaqCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            if (string.IsNullOrWhiteSpace(command.Language)) command.Language = _currentUserContext.Language;
            command.PageId = id;

            var response = await _mediator.Send(command);
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpDelete("{id}/faq/{faqId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteFaq([FromRoute] Guid id, [FromRoute] Guid faqId)
        {
            var response = await _mediator.Send(new DeleteFaqCommand
            {
                FaqId = faqId,
                PageId = id
            });
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> PatchPage([FromRoute] Guid id, [FromBody] JsonPatchDocument<UpdatePageDTO> doc)
        {
            if (doc == null) throw new InvalidParameterException(nameof(doc));
            var pageDTO = await _mediator.Send(new GetPageForUpdateQuery { Id = id });
            doc.ApplyTo(pageDTO);

            await _mediator.Send(new UpdatePageCommand
            {
                PageId = id,
                Page = pageDTO
            });
            return new SuccessResult();
        }

        [HttpGet("{id}/about")]
        [ProducesResponseType(typeof(SuccessResult<AboutDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAboutPage([FromRoute] Guid id, [FromQuery] string language = null)
        {
            if (string.IsNullOrWhiteSpace(language))
                language = Mosaico.Base.Constants.Languages.English;

            var response = await _mediator.Send(new GetAboutQuery
            {
                Language = language,
                PageId = id
            });

            return new SuccessResult(response);
        }

        [HttpPost("{id}/about")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpsertAbout([FromRoute] Guid id, [FromBody] UpsertAboutCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.PageId = id;

            var memberId = await _mediator.Send(command);

            return new SuccessResult(memberId) { StatusCode = 201 };
        }

        [HttpGet("{id}/packages")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SuccessResult<GetInvestmentPackagesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPackagesAsync([FromRoute] Guid id, [FromQuery] string language = null)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                language = _currentUserContext.Language;
            }

            var response = await _mediator.Send(new GetInvestmentPackagesQuery
            {
                Id = id,
                Language = language
            });
            return new SuccessResult(response);
        }

        [HttpPut("{id}/packages/{packageId}")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdateInvestmentPackage([FromRoute] Guid id, [FromRoute] Guid packageId, [FromBody] UpsertInvestmentPackageCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.Language))
                command.Language = _currentUserContext.Language;

            command.PageId = id;
            command.InvestmentPackageId = packageId;

            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }

        [HttpPost("{id}/packages")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> CreateInvestmentPackage([FromRoute] Guid id, [FromBody] UpsertInvestmentPackageCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));

            if (string.IsNullOrWhiteSpace(command.Language))
                command.Language = _currentUserContext.Language;

            command.PageId = id;

            var response = await _mediator.Send(command);
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpDelete("{id}/packages/{packageId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteInvestmentPackage([FromRoute] Guid id, [FromRoute] Guid packageId)
        {
            await _mediator.Send(new DeleteInvestmentPackageCommand
            {
                PageId = id,
                InvestmentPackageId = packageId
            });
            return new SuccessResult();
        }

        [HttpGet("{id}/articles")]
        [ProducesResponseType(typeof(SuccessResult<GetArticlesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectArticlesAsync([FromRoute] string id, [FromQuery] int skip = 0, [FromQuery] int take = 6)
        {

            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
                throw new InvalidParameterException(nameof(id));

            var response = await _mediator.Send(new GetArticlesQuery { ProjectId = projectId, Take = take, Skip = skip });
            return new SuccessResult(response);
        }

        [HttpPost("{id}/article")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpsertProjectArticleAsync([FromRoute] string id, [FromBody] UpsertProjectArticlesCommand command)
        {
            if (command == null)
            {
                throw new InvalidParameterException(nameof(command));
            }
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
                throw new InvalidParameterException(nameof(id));

            command.ProjectId = projectId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }
        [HttpDelete("{id}/article")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteProjectArticleAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var articleId))
                throw new InvalidParameterException(nameof(id));

            var response = await _mediator.Send(new DeleteArticleCommand { ArticleId = articleId });
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }
        [HttpPost("{id}/article/hide")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> HideProjectArticleAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var articleId))
                throw new InvalidParameterException(nameof(id));

            var response = await _mediator.Send(new HideArticleCommand { ArticleId = articleId });
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPost("{id}/article/display")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DisplayProjectArticleAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var articleId))
                throw new InvalidParameterException(nameof(id));

            var response = await _mediator.Send(new DisplayArticleCommand { ArticleId = articleId });
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPost("{id}/introvideo/upload")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UploadPageIntroVideoAsync([FromRoute] Guid id, [FromForm] string videoExternalLink, [FromForm] bool showLocalVideo,[FromForm] IFormFile file)
        {
            var command = new UploadProjectIntroVideoCommand();
            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    command.Content = stream.ToArray();
                }
            }
            else
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.PageId = id;
            command.VideoExternalLink = videoExternalLink;
            command.ShowLocalVideo = showLocalVideo;
            var response = await _mediator.Send(command);
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPost("{id}/introvideourl/update")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdatePageIntroVideoUrlAsync([FromRoute] Guid id, [FromForm] string videoExternalLink, [FromForm] bool showLocalVideo)
        {
            var command = new UpdateProjectIntroVideoUrlCommand();
            if (string.IsNullOrEmpty(videoExternalLink))
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.VideoUrl = videoExternalLink;
            command.ShowLocalVideo = showLocalVideo;
            command.PageId = id;
            var response = await _mediator.Send(command);
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpGet("{id}/introvideos")]
        [ProducesResponseType(typeof(SuccessResult<GetPageIntroVideoQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPageIntroVideosAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var PageId))
                throw new InvalidParameterException(nameof(id));

            var response = await _mediator.Send(new GetPageIntroVideoQuery { PageId = PageId });
            return new SuccessResult(response);
        }
        
        [HttpGet("{pageId}/reviews")]
        [ProducesResponseType(typeof(SuccessResult<List<PageReviewDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPageReviews([FromRoute] Guid pageId)
        {
            var response = await _mediator.Send(new GetTokenPageReviewsQuery
            {
                PageId = pageId
            });
            return new SuccessResult(response);
        }

        [HttpDelete("{pageId}/reviews/{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePageReviewAsync([FromRoute] Guid pageId, [FromRoute] Guid id)
        {
            await _mediator.Send(new DeletePageReviewCommand
            {
                Id = id,
                PageId = pageId
            });
            return new SuccessResult();
        }
        
        [HttpPost("{pageId}/reviews")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreatePageReviewAsync([FromRoute] Guid pageId, [FromBody] UpsertPageReviewCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.PageId = pageId;
            var response = await _mediator.Send(command);
            return new SuccessResult(response){StatusCode = StatusCodes.Status201Created};
        }
        
        [HttpPut("{pageId}/reviews/{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpsertPageReviewAsync([FromRoute] Guid pageId, [FromRoute] Guid id, [FromBody] UpsertPageReviewCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.PageId = pageId;
            command.Id = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}
