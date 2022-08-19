using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Extensions;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.Extensions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.Statistics.Abstractions;
using Mosaico.Domain.Statistics.Entities;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.DataSeed;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.SDK.Wallet.Abstractions;
using NBitcoin;
using Serilog;
using Constants = Mosaico.Blockchain.Base.Constants;
using Transaction = Mosaico.Domain.Wallet.Entities.Transaction;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("generate-fake-data", "Generates fake projects in database")]
    public class GenerateFakeDataCommand : CommandBase
    {
        private readonly IProjectDbContext _context;
        private readonly IBusinessDbContext _businessDb;
        private readonly IWalletDbContext _walletContext;
        private readonly IEthereumClientFactory _ethereumClient;
        private readonly IIdentityContext _identityContext;
        private readonly IMigrationRunner _migrationRunner;
        private readonly ILogger _logger;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly ITokenService _tokenService;
        private readonly IIndex<string, ICrowdsaleService> _crowdsaleServices;
        private readonly IIndex<string, ITokenService> _tokenServices;
        private readonly IWalletClient _walletClient;
        private readonly ILocalPaymentTokensDeployer _localPaymentTokensDeployer;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IStatisticsDbContext _statisticsDbContext;

        private readonly List<string> _countries = new List<string>{"Poland", "Germany", "Hungary", "USA", "Slovenia", "Slovakia", "Estonia"};

        public GenerateFakeDataCommand(
            IProjectDbContext context,
            IBusinessDbContext businessDb, 
            IWalletDbContext walletContext,
            IIdentityContext identityContext, 
            IEthereumClientFactory ethereumClient, 
            ILogger logger, 
            IMigrationRunner migrationRunners, 
            IUserWriteRepository userWriteRepository, 
            ITokenService tokenService,
            IIndex<string, ICrowdsaleService> crowdsaleService,
            IIndex<string, ITokenService> tokenServices,
            IWalletClient walletClient, 
            ILocalPaymentTokensDeployer localPaymentTokensDeployer, 
            IExchangeRateRepository exchangeRateRepository, 
            IStatisticsDbContext statisticsDbContext)
        {
            _context = context;
            _businessDb = businessDb;
            _walletContext = walletContext;
            _identityContext = identityContext;
            _ethereumClient = ethereumClient;
            _logger = logger;
            _migrationRunner = migrationRunners;
            _userWriteRepository = userWriteRepository;
            _tokenService = tokenService;
            _crowdsaleServices = crowdsaleService;
            _walletClient = walletClient;
            _localPaymentTokensDeployer = localPaymentTokensDeployer;
            _exchangeRateRepository = exchangeRateRepository;
            _statisticsDbContext = statisticsDbContext;
            _tokenServices = tokenServices;
        }

        public override async Task Execute()
        {
            _migrationRunner?.RunMigrations();
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    await DeployStableCoinsAsync();
                    await FetchExchangeRatesAsync();
                    var users = await GenerateUsersAsync(50);
                    var companies = await GenerateCompaniesAsync(25, users);
                    var projects = await GenerateProjectsAsync(25, companies, users);
                    var projectTokens = await CreateProjectTokensAsync(projects);
                    await GeneratePurchaseTransactionsAsync(projects, users, 25);
                    
                    var defaultUser = await _identityContext.Users.FirstOrDefaultAsync(u => u.Email == "dev@mosaico.ai");
                    if (defaultUser == null)
                    {
                        defaultUser = await CreateDefaultUserAsync();
                    }
                    var defaultUserId = defaultUser.Id;
                    var defaultUserWallet =
                        await _walletContext.Wallets.FirstOrDefaultAsync(w => w.UserId == defaultUserId);
                    if (defaultUserWallet == null)
                    {
                        await CreateWalletAsync(defaultUserId);
                    }
                    var defaultUserCompanies = await GenerateCompaniesAsync(10, new List<ApplicationUser>{defaultUser});
                    var defaultUserProjects = await GenerateProjectsAsync(10, defaultUserCompanies, new List<ApplicationUser>{defaultUser});
                    var defaultUserTokens = await CreateProjectTokensAsync(defaultUserProjects);
                    await GeneratePurchaseTransactionsAsync(defaultUserProjects, new List<ApplicationUser>{defaultUser}, 2);
                    await GenerateArticlesAsync(projects, users, 25);
                    await DeploySmartContractsAsync(projects, projectTokens, companies);

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger?.Error(ex, "");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task<ApplicationUser> CreateDefaultUserAsync()
        {
            var email = "dev@mosaico.ai";
            var normalizedEmail = email.ToUpperInvariant();
            var user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                Registered = DateTime.UtcNow,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                NormalizedEmail = normalizedEmail,
                NormalizedUserName = normalizedEmail,
                EmailConfirmed = true,
                FirstName = "Dev",
                LastName = "Mosaico",
                NewsletterDataProcessingAgree = true,
                NewsletterDataProcessingAgreedDate = DateTime.UtcNow,
                IsAdmin = true
            };
            user.Id = "C86D8234-AAA3-4B7D-95ED-E31BC5B4E3B3";
            _identityContext.Users.Add(user);
            await _identityContext.SaveChangesAsync();
            var hasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = hasher.HashPassword(user, "Mosaico1");
            _identityContext.Users.Update(user);
            await _identityContext.SaveChangesAsync();
            return user;
        }

        private async Task<List<ApplicationUser>> GenerateUsersAsync(int numberOfUsers)
        {
            _logger?.Information($"Generating {numberOfUsers} of users");
            var users = new Faker<ApplicationUser>()
                .RuleFor(u => u.Id, f => Guid.NewGuid().ToString())
                .RuleFor(u => u.Registered, f => f.Date.Past(3))
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => $"{f.Random.Int(0, 100)}{f.Person.Email}")
                .RuleFor(u => u.PhotoUrl, f => f.Internet.Avatar())
                .RuleFor(u => u.EmailConfirmed, true)
                .RuleFor(u => u.NewsletterDataProcessingAgree, true)
                .RuleFor(u => u.AMLStatus, AMLStatus.Confirmed)
                .RuleFor(u => u.LastLogin, f => f.Date.Recent())
                .RuleFor(u => u.ConcurrencyStamp, Guid.NewGuid().ToString())
                .RuleFor(u => u.SecurityStamp, Guid.NewGuid().ToString())
                .FinishWith((f, u) =>
                {
                    u.NormalizedEmail = u.Email.ToUpper();
                    u.UserName = u.Email;
                    u.NormalizedUserName = u.NormalizedEmail;
                    _logger?.Information($"User {u.Email} was generated");
                }).Generate(numberOfUsers);
            _identityContext.Users.AddRange(users);
            await _identityContext.SaveChangesAsync();
            _logger?.Information($"All users saved");
            foreach (var user in users)
            {
                _logger?.Information($"Creating wallet for user {user.Email}");
                await CreateWalletAsync(user.Id);
            }
            _logger?.Information($"Wallets were created and saved");
            await _walletContext.SaveChangesAsync();
            return users;
        }

        private async Task<PaymentCurrency> GetPaymentCurrencyAsync(string ticker, string chain)
        {
            var paymentCurrency = await _walletContext
                .PaymentCurrencies
                .Where(x => x.Ticker == ticker && x.Chain == chain)
                .SingleOrDefaultAsync();

            return paymentCurrency;
        }

        private async Task DeployStableCoinsAsync()
        {
            var paymentCurrencies = await _walletContext
                .PaymentCurrencies
                .Where(x => !x.NativeChainCurrency && x.Ticker == "USDT")
                .ToListAsync();
            
            foreach (var paymentCurrency in paymentCurrencies)
            {
                try
                {
                    var tetherContractAddress = await _localPaymentTokensDeployer.DeployTetherAsync(
                        paymentCurrency.Chain,
                        100000000,
                        "7b9b4f50d30f8eba180081741226ce1deee15c8b894af500106bd36e9123a22d");

                    paymentCurrency.ContractAddress = tetherContractAddress;
                }
                catch (Exception er)
                {
                    _logger.Error(er, "Error during deployment of token");
                    continue;
                }
            }

            await _walletContext.SaveChangesAsync();
        }
        
        private async Task GenerateArticlesAsync(List<Project> projects,List<ApplicationUser> users, int transactionsPerProject)
        {
            foreach (var project in projects)
            {
                _logger?.Information($"Generating articles for the project {project.Title}");
                var transactions = new Faker<Article>()
                       .RuleFor(t => t.AuthorPhoto, f => f.Internet.Avatar())
                       .RuleFor(t => t.CoverPicture, f => f.Internet.Avatar())
                       .RuleFor(t => t.AuthorName, f => f.Name.FullName())
                       .RuleFor(t => t.Date, f => f.Date.Recent(1))
                       .RuleFor(t => t.CreatedAt, f => f.Date.Recent())
                       .RuleFor(t => t.ModifiedAt, (f, t) => t.CreatedAt)
                       .RuleFor(t => t.CreatedById, f => f.PickRandom(users.Select(u => u.Id)))
                       .RuleFor(t => t.ModifiedById, (f, t) => t.CreatedById)
                       .RuleFor(t => t.ProjectId, f => project.Id)
                       .RuleFor(t => t.Project, f => project)
                       .RuleFor(t => t.Hidden, f => false)
                       .RuleFor(t => t.Link, f => f.Internet.Url())
                       .RuleFor(t => t.VisibleText, f => f.Lorem.Sentence(1))
                       .RuleFor(t => t.ModifiedById, (f, t) => t.CreatedById)
                       .FinishWith((f, t) =>
                       {
                       }).Generate(transactionsPerProject);
                _context.Articles.AddRange(transactions);
                await _context.SaveChangesAsync();
                _logger?.Verbose($"Transactions were saved");
            }
        }
        private async Task GeneratePurchaseTransactionsAsync(List<Project> projects, List<ApplicationUser> users, int transactionsPerProject)
        {
            var transactionStatuses = await _walletContext.TransactionStatuses
                .Where(t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed || t.Key == Domain.Wallet.Constants.TransactionStatuses.Canceled)
                .ToListAsync();
            
            var purchaseTransactionType =
                await _walletContext.TransactionType.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionType.Purchase);

            var usdtPaymentCurrency = await GetPaymentCurrencyAsync("USDT", Constants.BlockchainNetworks.Ethereum);

            foreach (var project in projects)
            {
                _logger?.Information($"Generating {transactionsPerProject} for the project {project.Title}");
                var token = await _walletContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId);
                if (token != null)
                {
                    var tokensLeft = token.TotalSupply;

                    var transactions = new Faker<Transaction>()
                        .RuleFor(t => t.Status, f => f.PickRandom(transactionStatuses))
                        .RuleFor(t => t.Type, purchaseTransactionType)
                        .RuleFor(t => t.CorrelationId, Guid.NewGuid().ToString)
                        .RuleFor(t => t.CreatedAt, f => f.Date.Recent())
                        .RuleFor(t => t.ModifiedAt, (f, t) => t.CreatedAt)
                        .RuleFor(t => t.CreatedById, f => f.PickRandom(users.Select(u => u.Id)))
                        .RuleFor(t => t.ModifiedById, (f, t) => t.CreatedById)
                        .RuleFor(t => t.InitiatedAt, f => f.Date.RecentOffset(7))
                        .RuleFor(t => t.FinishedAt, (f, t) => t.InitiatedAt.AddMinutes(3))
                        .RuleFor(t => t.PaymentProcessor, "checkout")
                        .RuleFor(t => t.TokenId, f => project.TokenId)
                        .RuleFor(t => t.PaymentCurrencyId, f => usdtPaymentCurrency.Id)
                        .RuleFor(t => t.Currency, usdtPaymentCurrency.Ticker)
                        .RuleFor(t => t.UserId, f => f.PickRandom(users.Select(u => u.Id)))
                        .FinishWith((f, t) =>
                        {
                            if (t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Canceled)
                            {
                                t.FailureReason = f.Lorem.Sentences(2);
                            }

                            var wallet = _walletContext.Wallets.FirstOrDefault(w => w.UserId == t.UserId);
                            t.WalletAddress = wallet.AccountAddress;

                            var walletToToken = wallet.Tokens.FirstOrDefault(t => t.TokenId == project.TokenId);
                            if (walletToToken == null)
                            {
                                walletToToken = new WalletToToken
                                {
                                    Token = token,
                                    Wallet = wallet,
                                    TokenId = token.Id,
                                    WalletId = wallet.Id,
                                    Balance = 0
                                };
                                wallet.Tokens.Add(walletToToken);
                            }

                            var activeStage = project.Stages.FirstOrDefault(s => s.Type != StageType.Private);
                            if (activeStage != null)
                            {
                                if (tokensLeft > activeStage.MaximumPurchase)
                                {
                                    t.TokenAmount = f.Random.Decimal(activeStage.MinimumPurchase,
                                        activeStage.MaximumPurchase);
                                }
                                else if (tokensLeft > activeStage.MinimumPurchase &&
                                         tokensLeft < activeStage.MaximumPurchase)
                                {
                                    t.TokenAmount = f.Random.Decimal(activeStage.MinimumPurchase, tokensLeft);
                                }
                                else
                                {
                                    return;
                                }

                                t.PayedAmount = t.TokenAmount * activeStage.TokenPrice;
                                walletToToken.Balance += t.TokenAmount.Value;
                                tokensLeft -= t.TokenAmount.Value;
                                _logger?.Information(
                                    $"Wallet {wallet.AccountAddress} just purchased {t.PayedAmount} USDT for {t.TokenAmount} tokens");
                            }
                        })
                        .Generate(transactionsPerProject);
                    _walletContext.Transactions.AddRange(transactions);
                    await _walletContext.SaveChangesAsync();

                    await AddStatisticsPurchaseTransactions(transactions);
                    
                    _logger?.Verbose($"Transactions were saved");
                }
            }
        }

        private async Task AddStatisticsPurchaseTransactions(List<Transaction> transactions)
        {
            var transactionsToStatistics = transactions.Select(t => new PurchaseTransaction
            {
                Id = Guid.NewGuid(),
                Currency = t.Currency,
                Payed = t.PayedAmount.Value,
                USDTAmount = t.PayedAmount.Value,
                TokenId = t.TokenId.Value,
                UserId = Guid.Parse(t.UserId),
                TokensAmount = t.TokenAmount.Value,
                TransactionId = t.Id,
                Date = t.CreatedAt
            }).ToList();
            _statisticsDbContext.PurchaseTransactions.AddRange(transactionsToStatistics);
            await _statisticsDbContext.SaveChangesAsync();
        }

        private async Task<List<Project>> GenerateProjectsAsync(int projectNumbers, List<Company> companies, List<ApplicationUser> users)
        {
            var projectStatuses = await _context.ProjectStatuses.Where(p =>
                p.Key == Domain.ProjectManagement.Constants.ProjectStatuses.New ||
                p.Key == Domain.ProjectManagement.Constants.ProjectStatuses.InProgress ||
                p.Key == Domain.ProjectManagement.Constants.ProjectStatuses.UnderReview ||
                p.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Closed)
                .ToListAsync();
            var ownerProjectRole = await _context.Roles.FirstOrDefaultAsync(r => r.Key == Domain.ProjectManagement.Constants.Roles.Owner);
            
            var defaultPaymentKeys = Domain.ProjectManagement.Constants.PaymentMethods.ProjectDefault;
            var defaultPaymentMethods = await _context
                .PaymentMethods
                .Where(x => defaultPaymentKeys.Contains(x.Key))
                .ToListAsync();
            
            var tetherEntity = await GetPaymentCurrencyAsync("USDT", Constants.BlockchainNetworks.Ethereum);
            var usdcEntity = await GetPaymentCurrencyAsync("USDC", Constants.BlockchainNetworks.Ethereum);
            
            var stageStatuses = await _context.StageStatuses.ToListAsync();
            _logger?.Information($"Generating {projectNumbers} projects");
            var projects = new Faker<Project>()
                .RuleFor(p => p.Title, f => f.Lorem.Sentence(3))
                .RuleFor(p => p.TitleInvariant, (f,p) => p.Title.ToUpperInvariant())
                .RuleFor(p => p.Status, f => f.PickRandom(projectStatuses))
                .RuleFor(p => p.CreatedById, f => f.PickRandom(users.Select(u => u.Id)))
                .RuleFor(p => p.ModifiedById, (f, p) => p.CreatedById)
                .RuleFor(c => c.CreatedAt, f => f.Date.Recent())
                .RuleFor(c => c.ModifiedAt, f => f.Date.Recent())
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph(3))
                .RuleFor(p => p.Slug, f => f.Lorem.Slug(3))
                .RuleFor(p => p.LogoUrl, f => f.Internet.Avatar())
                .RuleFor(p => p.SlugInvariant, (f, p) => p.Slug.ToUpper())
                .FinishWith((f, p) =>
                {
                    p.IsVisible = true;
                    var hardCap = f.Random.Int(100, 2000);
                    p.Crowdsale = new Crowdsale
                    {
                        ContractAddress = p.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Closed ? f.Finance.EthereumAddress() : null,
                        ContractVersion = Integration.Blockchain.Ethereum.Constants.CrowdsaleContractVersions.Version1,
                        HardCap = hardCap,
                        SoftCap = (decimal) (hardCap * 0.1),
                        ProjectId = p.Id,
                        Network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum,
                        SupportedStableCoins = new List<string> {tetherEntity.ContractAddress}
                    };
                    var company = f.PickRandom(companies);
                    p.CompanyId = company.Id;
                    p.Crowdsale.OwnerAddress = /*TODO: replace with company wallet address */ f.Finance.EthereumAddress();
                    
                    var stageStatus = Domain.ProjectManagement.Constants.StageStatuses.Closed;
                    var startDate = f.Date.RecentOffset(5);
                    var endDate = startDate.AddHours(3);
                    if (p.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.InProgress)
                    {
                        stageStatus = Domain.ProjectManagement.Constants.StageStatuses.Active;
                        startDate = f.Date.RecentOffset(1);
                        endDate = startDate.AddDays(15);
                    }
                    if (p.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Approved)
                    {
                        stageStatus = Domain.ProjectManagement.Constants.StageStatuses.Pending;
                        startDate = f.Date.SoonOffset(5);
                        endDate = startDate.AddHours(3);
                    }
                   
                    var minPurchase = f.Random.Long(0, 20);
                    var maxPurchase = f.Random.Long(minPurchase * 2, minPurchase * 5);
                    var tokenPrice = f.Random.Int(1, 5);

                    p.Stages.Add(new Stage
                    {
                        Name = "Public",
                        Order = 1,
                        Status = stageStatuses.FirstOrDefault(s => s.Key == stageStatus),
                        MinimumPurchase = minPurchase,
                        MaximumPurchase = maxPurchase,
                        Type = StageType.Public,
                        StartDate = startDate,
                        EndDate = endDate,
                        TokenPrice = tokenPrice,
                        TokenPriceNativeCurrency = tokenPrice / 3000,
                        CreatedById = p.CreatedById,
                        ModifiedById = p.ModifiedById,
                        ModifiedAt = f.Date.Recent(),
                        CreatedAt = f.Date.Recent(),
                        TokensSupply = hardCap,
                        ProjectId = p.Id
                    });
                    
                    p.PaymentMethods.AddRange(defaultPaymentMethods);
                    
                    var faqs = GenerateFAQs(f, 3);
                    var packages = GenerateInvestmentPackages(f, 4);
                    p.Page = new Page
                    {
                        Faqs = faqs,
                        ShortDescription = new ShortDescription
                        {
                            Key = Guid.NewGuid().ToString(),
                            Title = Guid.NewGuid().ToString(),
                            Translations  = new List<ShortDescriptionTranslation>
                            {
                                new ShortDescriptionTranslation
                                {
                                    Language = Base.Constants.Languages.English,
                                    Value = f.Lorem.Letter(160)
                                }
                            }
                        },
                        SocialMediaLinks = new List<SocialMediaLink>
                        {
                            new SocialMediaLink
                            {
                                Key = Domain.ProjectManagement.Constants.SocialMediaLinks.Facebook,
                                Title = Guid.NewGuid().ToString(),
                                Translations = new List<SocialMediaLinkTranslation>
                                {
                                    new SocialMediaLinkTranslation
                                    {
                                        Language = Base.Constants.Languages.English,
                                        Value = f.Internet.Url()
                                    }
                                }
                            }
                        },
                        InvestmentPackages = packages
                    };
                        
                    _logger?.Information($"Project {p.Title} with one public stage was generated");
                })
                .Generate(projectNumbers);
            _context.Projects.AddRange(projects);
            await _context.SaveChangesAsync();
            foreach (var prj in projects)
            {
                prj.Members.Add(new ProjectMember
                {
                    UserId = prj.CreatedById,
                    AcceptedAt = DateTimeOffset.Now,
                    Role = ownerProjectRole,
                    IsAccepted = true,
                    RoleId = ownerProjectRole.Id,
                    IsInvitationSent = true,
                    CreatedById = prj.CreatedById,
                    ModifiedById = prj.CreatedById,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    Project = prj,
                    ProjectId = prj.Id,
                    Email = users.FirstOrDefault(f => f.Id == prj.CreatedById.ToString())?.Email
                });

                _context.Projects.Update(prj);
            }
            await AddProjectPermissionsAsync(projects);
            await _context.SaveChangesAsync();
            _logger?.Information($"All projects were saved");
            return projects;
        }

        private async Task AddProjectPermissionsAsync(List<Project> projects)
        {
            var permissions = Authorization.Base.Constants.Permissions.Project.GetAll();
            foreach (var project in projects)
            {
                var newPermissions =
                    permissions.ToDictionary<string, string, Guid?>(permission => permission, permission => project.Id);
                await _userWriteRepository.AddUserPermissions(project.CreatedById, newPermissions);
            }
        }

        private List<InvestmentPackage> GenerateInvestmentPackages(Faker f, int count)
        {
            var investmentPackage = new List<InvestmentPackage>();
            for (int i = 0; i < count; i++)
            {
                var package = new InvestmentPackage
                {
                    TokenAmount = f.Random.Int(1, 5000),
                    LogoUrl = f.Internet.Avatar(),
                    Translations = new List<InvestmentPackageTranslation>
                    {
                        new InvestmentPackageTranslation
                        {
                            Benefits = string.Join(Domain.ProjectManagement.Constants.InvestmentPackageBenefitSeparator, f.Lorem.Words(4)),
                            Language = Base.Constants.Languages.English,
                            Name = f.Vehicle.Manufacturer()
                        },
                        new InvestmentPackageTranslation
                        {
                            Benefits = string.Join(Domain.ProjectManagement.Constants.InvestmentPackageBenefitSeparator, f.Lorem.Words(4)),
                            Language = Base.Constants.Languages.Polish,
                            Name = f.Vehicle.Manufacturer()
                        }
                    }
                };
                investmentPackage.Add(package);
            }

            return investmentPackage;
        }

        private List<Faq> GenerateFAQs(Faker f, int count)
        {
            var faqs = new List<Faq>();
            for (int i = 0; i < count; i++)
            {
                var faqKey = Guid.NewGuid().ToString();
                var faq = new Faq
                {
                    Content = new FaqContent
                    {
                        Key = faqKey,
                        Title = faqKey,
                        Translations = new List<FaqContentTranslation>
                        {
                            new FaqContentTranslation
                            {
                                Language = Base.Constants.Languages.English,
                                Value = f.Lorem.Sentence(3)
                            },
                            new FaqContentTranslation
                            {
                                Language = Base.Constants.Languages.Polish,
                                Value = f.Lorem.Sentence(3)
                            }
                        }
                    },
                    Title = new FaqTitle
                    {
                        Key = faqKey,
                        Title = faqKey,
                        Translations = new List<FaqTitleTranslation>
                        {
                            new FaqTitleTranslation
                            {
                                Language = Base.Constants.Languages.English,
                                Value = f.Lorem.Sentence(1)
                            },
                            new FaqTitleTranslation
                            {
                                Language = Base.Constants.Languages.Polish,
                                Value = f.Lorem.Sentence(1)
                            }
                        }
                    }
                };
                faqs.Add(faq);
            }

            return faqs;
        }

        private async Task AddCompanyPermissionsAsync(List<Company> companies)
        {
            var permissions = Authorization.Base.Constants.Permissions.Company.GetAll();
            foreach (var company in companies)
            {
                var newPermissions =
                    permissions.ToDictionary<string, string, Guid?>(permission => permission, permission => company.Id);
                await _userWriteRepository.AddUserPermissions(company.CreatedById, newPermissions);
            }
        }

        private async Task<List<Token>> CreateProjectTokensAsync(List<Project> projects)
        {
            var tokenTypes = await _walletContext.TokenTypes.ToListAsync();
            var tokens = new List<Token>();
            foreach (var project in projects)
            {
                _logger?.Information($"Creating token for project {project.Title}");
                var token = new Faker<Token>()
                    .RuleFor(t => t.Address,
                        f => project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Closed
                            ? f.Finance.EthereumAddress()
                            : null)
                    .RuleFor(t => t.ContractVersion, Integration.Blockchain.Ethereum.Constants.DefaultTokenContractVersion)
                    .RuleFor(t => t.Name, f => f.Lorem.Letter(3).ToUpper() + " Token")
                    .RuleFor(t => t.Symbol, f => f.Lorem.Letter(3).ToUpper())
                    .RuleFor(t => t.Status, f => TokenStatus.Deployed)
                    .RuleFor(t => t.Type, f => f.PickRandom(tokenTypes))
                    .RuleFor(t => t.IsBurnable, f => f.Random.Bool())
                    .RuleFor(t => t.IsMintable, f => f.Random.Bool())
                    .RuleFor(t => t.OwnerAddress, f => f.Finance.EthereumAddress())
                    .RuleFor(c => c.CreatedAt, f => f.Date.Recent())
                    .RuleFor(c => c.ModifiedAt, f => f.Date.Recent())
                    .RuleFor(c => c.ModifiedById, project.ModifiedById)
                    .RuleFor(c => c.CompanyId, project.CompanyId)
                    .RuleFor(c => c.LogoUrl, f => f.Internet.Avatar())
                    .RuleFor(c => c.CreatedById, project.CreatedById)
                    .FinishWith((faker, t) =>
                    {
                        t.Network = Constants.BlockchainNetworks.Ethereum;
                        var initialSupply = project.Stages.First().TokensSupply * 10;
                        t.TotalSupply = t.TokensLeft = initialSupply;
                        if (project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Closed)
                        {
                            t.TokensLeft = 0;
                        }
                        _logger?.Information($"Token {t.Name} was successfully created");
                    }).Generate(1).First();
                _walletContext.Tokens.Add(token);
                await _walletContext.SaveChangesAsync();
                _logger?.Information($"Tokens were saved");
                project.TokenId = token.Id;
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
                tokens.Add(token);
            }

            return tokens;
        }

        private async Task<List<Company>> GenerateCompaniesAsync(int companyNumber, List<ApplicationUser> users)
        {
            var teamMemberRole = await _businessDb.TeamMemberRoles.FirstOrDefaultAsync(tr =>
                tr.Key == Domain.BusinessManagement.Constants.TeamMemberRoles.Owner);
            
            _logger?.Information($"Creating {companyNumber} companies");
            var fakeCompanies = new Faker<Company>()
                .RuleFor(c => c.Country, f => f.PickRandom(_countries))
                .RuleFor(c => c.Size, f => Domain.BusinessManagement.Constants.CompanySizes.Small)
                .RuleFor(c => c.Street, f => f.Address.StreetAddress(true))
                .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                .RuleFor(c => c.CreatedById, f => f.PickRandom(users.Select(u => u.Id).ToList()))
                .RuleFor(c => c.ModifiedById, f => f.PickRandom(users.Select(u => u.Id).ToList()))
                .RuleFor(c => c.CreatedAt, f => f.Date.Recent())
                .RuleFor(c => c.ModifiedAt, f => f.Date.Recent())
                .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
                .RuleFor(c => c.VATId, f => f.Finance.Account())
                .RuleFor(c => c.LogoUrl, f => f.Internet.Avatar())
                .RuleFor(c => c.Email, f => f.Person.Email)
                .RuleFor(c => c.PhoneNumber, f => f.Person.Phone)
                .Generate(companyNumber);

            await _businessDb.Companies.AddRangeAsync(fakeCompanies);
            await _businessDb.SaveChangesAsync();
            
            foreach (var fakeCompany in fakeCompanies)
            {
                fakeCompany.Slug = fakeCompany.CompanyName.ToSlug();
                fakeCompany.TeamMembers.Add(new TeamMember
                {
                    UserId = fakeCompany.CreatedById,
                    TeamMemberRole = teamMemberRole,
                    CreatedById = fakeCompany.CreatedById,
                    ModifiedById = fakeCompany.CreatedById,
                    CreatedAt = DateTimeOffset.UtcNow,
                    ModifiedAt = DateTimeOffset.UtcNow,
                    CompanyId = fakeCompany.Id,
                    IsAccepted = true,
                    IsInvitationSent = true,
                    ExpiresAt = DateTimeOffset.UtcNow.AddDays(-1),
                    AcceptedAt = DateTimeOffset.UtcNow.AddDays(-1),
                    AuthorizationCode = Guid.NewGuid().ToString()
                });
                _businessDb.Companies.Update(fakeCompany);
            }
            await _businessDb.SaveChangesAsync();
            await AddCompanyPermissionsAsync(fakeCompanies);
            _logger?.Information($"All companies were saved");
            return fakeCompanies;
        }
        
        internal class ProjectToDeployEntry
        {
            public Project Project { get; set; }
            public Stage ActiveStage { get; set; }
            public Token Token { get; set; }
            public Company Company { get; set; }
            public CompanyWallet CompanyWallet { get; set; }
            public string TokenContractAddress { get; set; }
            public string CrowdSaleContractAddress { get; set; }
        }
        
        private async Task<List<ProjectToDeployEntry>> DeploySmartContractsAsync(List<Project> projects, List<Token> tokens, List<Company> companies)
        {
            var usdtPaymentCurrency = await GetPaymentCurrencyAsync("USDT", Constants.BlockchainNetworks.Ethereum);
            
            var projectsToDeploy = new List<ProjectToDeployEntry>();
            
            /* Filter projects */
            foreach (var project in projects)
            {
                var projectToken = tokens.SingleOrDefault(x => x.Id == project.TokenId);
                var company = companies.SingleOrDefault(x => x.Id == project.CompanyId);
                var activeStage = project.ActiveStage();
                
                if (projectToken != null && company != null && activeStage != null)
                {
                    projectsToDeploy.Add(new ProjectToDeployEntry
                    {
                        Project = project,
                        Token = projectToken,
                        Company = company,
                        ActiveStage = activeStage
                    });
                }
            }
            
            var distinctCompanies = projectsToDeploy.Select(x => x.Company.Id).Distinct().ToList();
            
            /* Generate company wallets */
            foreach (var companyId in distinctCompanies)
            {
                var entries = projectsToDeploy.Where(x => x.Company.Id == companyId);
                var wallet = await CreateCompanyWalletAsync(companyId);
                foreach (var projectToDeployEntry in entries)
                {
                    projectToDeployEntry.CompanyWallet = wallet;
                }

                var client = _ethereumClient.GetClient(wallet.Network);
                await client.TransferFundsAsync(wallet.AccountAddress, new decimal(0.7));
            }
            await _walletContext.SaveChangesAsync();

            /* Deploy token & crowdsale contracts */
            foreach (var projectToDeployEntry in projectsToDeploy)
            {
                var project = projectToDeployEntry.Project;
                
                /* Token contract deployment */
                var token = await _walletClient.GetTokenAsync(projectToDeployEntry.Token.Id);
                var erc20ContractAddress = await _tokenService.DeployERC20Async(token.Network, configuration =>
                {
                    configuration.Name = token.Name;
                    configuration.Symbol = token.Symbol;
                    configuration.InitialSupply = token.TotalSupply.ConvertToBigInteger();
                    configuration.IsMintable = token.IsMintable;
                    configuration.IsBurnable = token.IsBurnable;
                    configuration.OwnerAddress = projectToDeployEntry.CompanyWallet.AccountAddress;
                    configuration.PrivateKey = projectToDeployEntry.CompanyWallet.PrivateKey;
                });
                projectToDeployEntry.TokenContractAddress = erc20ContractAddress;

                var companyWallet = await _walletClient.GetCompanyWalletAsync(projectToDeployEntry.Project.CompanyId.Value, token.Network);
                await _walletClient.SetTokenDeployedAsync(token.Id, companyWallet.AccountAddress, erc20ContractAddress, Integration.Blockchain.Ethereum.Constants.DefaultTokenContractVersion);
                await _walletClient.MintTokensToCompanyWallet(companyWallet.CompanyId, token.Id);
                
                _logger?.Information($"Token '{token.Id}' ({token.Symbol}) for project '{project.Id}' deployed on address: {erc20ContractAddress}");

                /* Crowdsale contract deployment */
                var stages = project.Stages.OrderBy(s => s.StartDate).ToList();
                var stage = stages[0];
                
                var crowdsaleContractVersion = Integration.Blockchain.Ethereum.Constants.CrowdsaleContractVersions.Version1;
                if (!_crowdsaleServices.TryGetValue(crowdsaleContractVersion, out var crowdsaleService))
                {
                    throw new UnknownContractVersionException("Crowdsale", crowdsaleContractVersion);
                }
                
                var crowdSaleContract = await crowdsaleService.DeployAsync(token.Network, configuration =>
                {
                    configuration.OwnerAddress = companyWallet.AccountAddress;
                    configuration.ERC20Address = projectToDeployEntry.TokenContractAddress;
                    configuration.StageCount = stages.Count;
                    configuration.SoftCapDenominator = 30m;
                    configuration.SupportedStableCoins = project.Crowdsale.SupportedStableCoins.ToList();
                    configuration.PrivateKey = projectToDeployEntry.CompanyWallet.PrivateKey;
                });
                projectToDeployEntry.CrowdSaleContractAddress = crowdSaleContract.ContractAddress;
                
                project.Crowdsale.ContractAddress = crowdSaleContract.ContractAddress;
                project.Crowdsale.OwnerAddress = crowdSaleContract.OwnerAddress;
                project.Crowdsale.ContractVersion = Integration.Blockchain.Ethereum.Constants.CrowdsaleContractVersions.Version1;
                
                var tokenContractVersion = Integration.Blockchain.Ethereum.Constants.DefaultTokenContractVersion;
                if (!_tokenServices.TryGetValue(tokenContractVersion, out var tokenService))
                {
                    throw new UnknownContractVersionException("ERC20", tokenContractVersion);
                }
                
                // TODO fix because wallet must has ether
                await tokenService.SetWalletAllowanceAsync(companyWallet.Network, x =>
                {
                    x.Amount = token.TotalSupply;
                    x.ContractAddress = erc20ContractAddress;
                    x.SpenderAddress = crowdSaleContract.ContractAddress;
                    x.OwnerPrivateKey = projectToDeployEntry.CompanyWallet.PrivateKey;
                });
                
                _logger?.Information($"Project '{project.Id}' crowdsale contract deployed on address: {crowdSaleContract.ContractAddress}");

                var nativeCurrencyRate = stage.TokenPrice / 3000;
                stage.TokenPriceNativeCurrency = nativeCurrencyRate;
                await crowdsaleService.StartNewStageAsync(token.Network, crowdSaleContract.ContractAddress, c =>
                {
                    c.Name = stage.Name;
                    c.Cap = stage.TokensSupply;
                    c.Rate = nativeCurrencyRate;
                    c.StableCoinRate = stage.TokenPrice;
                    c.IsPrivate = stage.Type == StageType.Private;
                    c.MinIndividualCap = stage.MinimumPurchase;
                    c.MaxIndividualCap = stage.MaximumPurchase;
                    c.Whitelist = new List<string> {"0x4d45024afdD0B862448865eAB591d35EE3952293"};
                    c.PrivateKey = projectToDeployEntry.CompanyWallet.PrivateKey;
                });
                _context.Projects.Update(project);
                _context.Crowdsales.Update(project.Crowdsale);
                _context.Stages.Update(stage);
            }

            await _context.SaveChangesAsync();
            await _walletContext.SaveChangesAsync();

            return projectsToDeploy;
        }
        
        private async Task<Wallet> CreateWalletAsync(string userId, string network = Constants.BlockchainNetworks.Default)
        {
            var client = _ethereumClient.GetClient(network);
            var account = client.CreateWallet();
            var wallet = new Wallet
            {
                UserId = userId,
                Network = Constants.BlockchainNetworks.Ethereum,
                PrivateKey = account.PrivateKey,
                AccountAddress = account.Address,
                PublicKey = account.PublicKey
            };
            _walletContext.Wallets.Add(wallet);
            
            await client.TransferFundsAsync(wallet.AccountAddress, new decimal(0.2));
            
            return wallet;
        }

        private async Task<CompanyWallet> CreateCompanyWalletAsync(Guid companyId, string network = Constants.BlockchainNetworks.Default)
        {
            var client = _ethereumClient.GetClient(network);
            var account = client.CreateWallet();
            var walletEntity = new CompanyWallet
            {
                CompanyId = companyId,
                Network = Constants.BlockchainNetworks.Ethereum,
                AccountAddress = account.Address,
                PrivateKey = account.PrivateKey,
                PublicKey = account.PublicKey
            };

            await _walletContext.CompanyWallets.AddAsync(walletEntity);

            return walletEntity;
        }

        private async Task FetchExchangeRatesAsync()
        {
            _logger?.Verbose($"Starting to fetch currency exchange rates");
            var exchangeRates = await _exchangeRateRepository.GetUsdExchangeAssetsAsync();
            _logger?.Verbose($"Exchange Rate Repository returned {exchangeRates.Count} results");
            var cryptoRates = exchangeRates.Where(d => Application.Wallet.Constants.CryptoCurrencies.All.Contains(d.Key));
            var fiatRates = exchangeRates.Where(d => Application.Wallet.Constants.FIATCurrencies.All.Contains(d.Key));
            foreach (var rate in cryptoRates)
            {
                _walletContext.ExchangeRates.Add(new ExchangeRate
                {
                    Rate = rate.Value,
                    Source = Integration.Blockchain.CoinAPI.Constants.ExchangeRateSource,
                    Ticker = rate.Key,
                    BaseCurrency = Application.Wallet.Constants.FIATCurrencies.USD,
                    IsCrypto = true
                });
            }

            foreach (var rate in fiatRates)
            {
                _walletContext.ExchangeRates.Add(new ExchangeRate
                {
                    Rate = rate.Value,
                    Source = Integration.Blockchain.CoinAPI.Constants.ExchangeRateSource,
                    Ticker = rate.Key,
                    BaseCurrency = Application.Wallet.Constants.FIATCurrencies.USD,
                    IsCrypto = false
                });
            }

            await _walletContext.SaveChangesAsync();
            _logger?.Information($"Successfully added {cryptoRates.Count() + fiatRates.Count()} rates to database");
        }
    }
}