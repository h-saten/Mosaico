using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;

namespace Mosaico.Application.ProjectManagement.Services
{
    public class TokenPageService : ITokenPageService
    {
        private readonly IProjectDbContext _projectDbContext;

        public TokenPageService(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task CreateTokenPageAsync(Project project)
        {
            if (project != null)
            {
                var page = new Page
                {
                    PageCovers = GetDefaultPageCovers(),
                    Faqs = GetDefaultFaqs(),
                    TeamMembers = await GetDefaultPageTeamMembers(),
                    PagePartners = await GetDefaultPagePartners(),
                    ProjectId = project.Id,
                    Project = project
                };
                _projectDbContext.TokenPages.Add(page);
                project.Page = page;
                project.PageId = page.Id;
                _projectDbContext.Projects.Update(project);
                await _projectDbContext.SaveChangesAsync();
            }
        }

        private List<PageCover> GetDefaultPageCovers()
        {
            //TODO: to json resource
            return new List<PageCover>();
        }

        private List<Faq> GetDefaultFaqs()
        {
            //TODO: to json resource
            return new List<Faq>
            {
                new Faq
                {
                    Content = new FaqContent
                    {
                        Key = "TEST_FAQ",
                        Title = "Test FAQ",
                        Translations = new List<FaqContentTranslation>
                        {
                            new FaqContentTranslation
                            {
                                Language = Mosaico.Base.Constants.Languages.English,
                                Value = "This is test FAQ"
                            },
                            new FaqContentTranslation
                            {
                                Language = Mosaico.Base.Constants.Languages.Polish,
                                Value = "To jest FAQ po polsku"
                            }
                        }
                    },
                    Order = 1,
                    Title = new FaqTitle
                    {
                        Key = "FAQ_QUESTION_TITLE",
                        Title = "Faq Question Title",
                        Translations = new List<FaqTitleTranslation>
                        {
                            new FaqTitleTranslation
                            {
                                Language = Mosaico.Base.Constants.Languages.English,
                                Value = "Ask me anything?"
                            },
                            new FaqTitleTranslation
                            {
                                Language = Mosaico.Base.Constants.Languages.Polish,
                                Value = "Zapytasz?"
                            }
                        }
                    },
                    IsHidden = false
                }
            };
        }

        private Task<List<PageTeamMember>> GetDefaultPageTeamMembers()
        {
            //TODO: to json
            var teamMember = new PageTeamMember
            {
                Name = $"John Doe",
                Order = 1,
                Position = "Participant",
                IsHidden = false
            };
            
            return Task.FromResult(new List<PageTeamMember>
            {
                teamMember
            });
        }
        private Task<List<PagePartners>> GetDefaultPagePartners()
        {
            //TODO: to json
            var partner = new PagePartners
            {
                Name = $"John Doe",
                Order = 1,
                Position = "Participant",
                IsHidden = false
            };

            return Task.FromResult(new List<PagePartners>
            {
                partner
            });
        }
    }
}