using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Domain.ProjectManagement;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.SDK.Base.Exceptions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Models;
using Serilog;

namespace Mosaico.SDK.ProjectManagement
{
    public class ProjectManagementClient : IProjectManagementClient
    {
        private readonly IProjectDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ProjectManagementClient(IProjectDbContext context, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<MosaicoProject>> GetActiveProjectsAsync(CancellationToken token = new())
        {
            var projects = await _context.Projects.Where(p => p.Status.Key == Constants.ProjectStatuses.InProgress).ToListAsync(token);
            return projects.Select(p => _mapper.Map<MosaicoProject>(p)).ToList();
        }

        public async Task<MosaicoProject> GetProjectAsync(Guid id, CancellationToken token = new())
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id, token);
            if (project == null)
                throw new MosaicoException($"Project {id} not found", "PROJECT_NOT_FOUND", 404);
            return _mapper.Map<MosaicoProject>(project);
        }

        public async Task SetAirdropWithdrawnAsync(Guid id, List<string> userIds, string transactionHash, CancellationToken token = new())
        {
            var a = await _context.AirdropCampaigns.Include(p => p.Project).Include(p => p.Participants)
                .FirstOrDefaultAsync(t => t.Id == id, token);
            if (a == null) throw new AirdropNotFoundException(id.ToString());
            if (userIds != null)
            {
                foreach (var participant in a.Participants.Where(p => p.UserId != null && userIds.Contains(p.UserId?.Trim().ToLower())))    
                {
                    participant.WithdrawnAt = DateTimeOffset.UtcNow;
                    participant.TransactionHash = transactionHash;
                }

                await _context.SaveChangesAsync(token);
            }
        }

        public async Task<List<MosaicoAirdrop>> GetStageAirdropsAsync(Guid id, CancellationToken token = new())
        {
            var airdrops = await _context.AirdropCampaigns.Where(a => a.StageId == id)
                .Include(p => p.Project).Include(p => p.Participants)
                .ToListAsync(cancellationToken: token);
            return airdrops.Select(a => _mapper.Map<MosaicoAirdrop>(a)).ToList();
        }

        public async Task<MosaicoAirdrop> GetProjectAirdropAsync(Guid id, CancellationToken token = new())
        {
            var a = await _context.AirdropCampaigns.Include(p => p.Project).Include(p => p.Participants)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, token);
            if (a == null) return null;
            return _mapper.Map<MosaicoAirdrop>(a);
        }

        public async Task<MosaicoProject> GetProjectAsync(string title, CancellationToken token = new())
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Title == title, token);
            if (project == null)
                //TODO: if API integration than exception should be filled from object response
                throw new MosaicoException($"Project {title} not found", "PROJECT_NOT_FOUND", 404);

            //TODO: introduce Automapper here
            return _mapper.Map<MosaicoProject>(project);
        }

        public async Task<ProjectStage> CurrentProjectSaleStage(Guid id, CancellationToken token = new())
        {
            var project = await _context.Projects.Include(p => p.Stages).ThenInclude(s => s.PurchaseLimits).FirstOrDefaultAsync(p => p.Id == id, cancellationToken: token);
            if (project == null)
            {
                throw new ProjectNotFoundException(id);
            }

            var currentStage = project.ActiveStage();

            if (currentStage == null)
                //TODO: if API integration than exception should be filled from object response
                throw new MosaicoException($"Project with ID: '{id}' current stage not found", "STAGE_NOT_FOUND", 404);

            return _mapper.Map<ProjectStage>(currentStage);
        }

        public async Task<MosaicoProjectDetails> GetProjectDetailsAsync(Guid id, CancellationToken token = new())
        {
            var project = await _context
                .Projects
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(p => p.Id == id, token);
            
            if (project == null)
                //TODO: if API integration than exception should be filled from object response
                throw new MosaicoException($"Project {id} not found", "PROJECT_NOT_FOUND", 404);
            //TODO: introduce Automapper here
            var projectDetails = _mapper.Map<MosaicoProjectDetails>(project);
            projectDetails.ActiveStageId = project.ActiveStage()?.Id;
            return projectDetails;
        }
        
        public async Task<List<MosaicoProjectInvestor>> GetProjectPrivateInvestorsAsync(Guid id)
        {
            var projectStages = await _context.Stages.Where(s => s.ProjectId == id).Select(s => s.Id).ToListAsync();
            if (!projectStages.Any())
            {
                return new List<MosaicoProjectInvestor>();
            }
            
            var investors = await _context
                            .ProjectInvestors
                            .Where(p => projectStages.Contains(p.StageId))
                            .ToListAsync();
            return investors.Select(p => _mapper.Map<MosaicoProjectInvestor>(p)).ToList();
        }

        public async Task<MosaicoProjectDetails> GetProjectDetailsAsync(string slug, CancellationToken token = new CancellationToken())
        {
            var project = await _context
                .Projects
                .Include(x => x.PaymentMethods)
                .Include(x => x.Stages)
                .ThenInclude(x => x.Status)
                .FirstOrDefaultAsync(p => p.Slug == slug, token);
            
            if (project == null)
            {
                //TODO: if API integration than exception should be filled from object response
                throw new MosaicoException($"Project {slug} not found", "PROJECT_NOT_FOUND", 404);
            }
            //TODO: introduce Automapper here
            var projectDetails = _mapper.Map<MosaicoProjectDetails>(project);
            projectDetails.ActiveStageId = project.ActiveStage()?.Id;
            return projectDetails;
        }
        
        public async Task<MosaicoProjectTransaction> GetProjectTransactionAsync(Guid projectId, CancellationToken token = new CancellationToken())
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken: token);
            if (project == null)
                throw new MosaicoException($"Project of ID {projectId} not found", "PROJECT_NOT_FOUND", 404);
            
            return _mapper.Map<MosaicoProjectTransaction>(project);
        }

        public async Task<MosaicoProjectDocument> GetProjectDocumentAsync(Guid id, CancellationToken token = new())
        {
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.Id == id, token);
            if (document == null) return null;

            return _mapper.Map<MosaicoProjectDocument>(document);
        }

        public async Task<MosaicoProjectDocument> GetProjectDocumentAsync(Guid projectId, string type, string lang = Mosaico.Base.Constants.Languages.English, CancellationToken token = new())
        {
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.ProjectId == projectId && d.Type.Key == type && d.Language == lang, token);
            if (document == null) return null;

            return _mapper.Map<MosaicoProjectDocument>(document);
        }

        public async Task<List<string>> GetProjectPaymentCurrenciesAsync(Guid projectId)
        {
            var crowdsale = await _context.Crowdsales.FirstOrDefaultAsync(t => t.ProjectId == projectId);
            return crowdsale?.SupportedStableCoins;
        }

        public async Task<List<MosaicoProject>> GetProjectsByTokenAsync(Guid tokenId, CancellationToken token = new CancellationToken())
        {
            var projects = await _context.Projects
                .Include(p => p.Stages).ThenInclude(s => s.PurchaseLimits)
                .Include(p => p.Stages).ThenInclude(s => s.Status)
                .AsNoTracking()
                .Where(t => t.TokenId == tokenId)
                .ToListAsync(token);
            return projects.Select(p => _mapper.Map<MosaicoProject>(p)).ToList();
        }

        public async Task<List<MosaicoProject>> GetProjectsOfCompanyAsync(Guid id, int limit = 3, CancellationToken token = new CancellationToken())
        {
            var projects = await _context.Projects
                .Include(s => s.Stages).ThenInclude(s => s.PurchaseLimits)
                .Include(s => s.Stages).ThenInclude(s => s.Status)
                .Where(c => c.CompanyId == id && c.IsVisible).AsNoTracking()
                .ToListAsync(token);
            return projects.Select(p => _mapper.Map<MosaicoProject>(p)).ToList();
        }

        public async Task<ProjectStage> GetStageAsync(Guid id, CancellationToken token = new CancellationToken())
        {
            var s = await _context.Stages.Include(s => s.Status).FirstOrDefaultAsync(s => s.Id == id, cancellationToken: token);
            return s == null ? null : _mapper.Map<ProjectStage>(s);
        }

        public Task<List<KeyValuePair<Guid, string>>> GetTokensCrowdSaleAsync(List<Guid> tokensId, CancellationToken token = new())
        {
            return _context
                .Projects
                .Include(x => x.Crowdsale)
                .AsNoTracking()
                .Where(x => x.TokenId != null
                            && x.Crowdsale != null
                            && tokensId.Contains((Guid) x.TokenId)
                            && x.Crowdsale.ContractAddress != null)
                .Select(x => new KeyValuePair<Guid, string>((Guid) x.TokenId, x.Crowdsale.ContractAddress))
                .ToListAsync(cancellationToken: token);
        }
        
        public async Task<int> SubscribersAmountAsync(Guid id, CancellationToken token = new())
        {
            return await _context
                .ProjectNewsletterSubscriptions
                .AsNoTracking()
                .Where(x => x.ProjectId == id)
                .CountAsync(token);
        }
        
        public async Task<ActiveSaleProject> GetProjectWithActiveSale(Guid tokenId, CancellationToken token = new())
        {
            var projects = await _context
                .Projects
                .Include(p => p.Stages).ThenInclude(p => p.Status)
                .Include(s => s.Stages).ThenInclude(s => s.PurchaseLimits)
                .AsNoTracking()
                .Where(t => t.TokenId == tokenId)
                .ToListAsync(token);

            Project projectWithActiveSale = null;            
            Guid? projectActiveStageId = null;            
            foreach (var project in projects)
            {
                var projectActiveStage = project.ActiveStage(); 
                if (projectActiveStage == null)
                    continue;
                
                projectWithActiveSale = project;
                projectActiveStageId = projectActiveStage.Id;

                if (projectWithActiveSale is not null)
                    break;
            }

            if (projectWithActiveSale is null)
            {
                return null;
            }
            
            var responseProject = _mapper.Map<MosaicoProject>(projectWithActiveSale);
            return new ActiveSaleProject
            {
                Project = responseProject,
                ActiveStageId = (Guid) projectActiveStageId
            };
        }
    }
}
