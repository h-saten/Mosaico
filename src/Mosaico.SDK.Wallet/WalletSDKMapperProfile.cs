using AutoMapper;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.SDK.Wallet.Models;
using DomainCompanyWallet = Mosaico.Domain.Wallet.Entities.CompanyWallet;
using DomainCompanyWalletToToken = Mosaico.Domain.Wallet.Entities.CompanyWalletToToken;
using DomainTransaction = Mosaico.Domain.Wallet.Entities.Transaction;

namespace Mosaico.SDK.Wallet
{
    public class WalletSDKMapperProfile : Profile
    {
        public WalletSDKMapperProfile()
        {
            CreateMap<TokenToExternalExchange, TokenExchange>()
                .ForMember(p => p.Id, opt => opt.MapFrom(d => d.ExternalExchangeId))
                .ForMember(p => p.Url, opt => opt.MapFrom(d => d.ExternalExchange.Url))
                .ForMember(p => p.LogoUrl, opt => opt.MapFrom(d => d.ExternalExchange.LogoUrl))
                .ForMember(p => p.ListedAt, opt => opt.MapFrom(d => d.ListedAt))
                .ForMember(p => p.IsDisabled, opt => opt.MapFrom(d => d.ExternalExchange.IsDisabled));
            CreateMap<Token, MosaicoToken>()
                .ForMember(p => p.Type, opt => opt.MapFrom(d => d.Type.Key));
            CreateMap<ExternalExchange, TokenExchange>();
            CreateMap<CompanyWallet, MosaicoCompanyWallet>()
                .ForMember(p => p.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(p => p.Network, opt => opt.MapFrom(d => d.Network))
                .ForMember(p => p.AccountAddress, opt => opt.MapFrom(d => d.AccountAddress))
                .ForMember(p => p.CompanyId, opt => opt.MapFrom(d => d.CompanyId))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<Domain.Wallet.Entities.Wallet, MosaicoUserWallet>()
                .ForMember(p => p.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(p => p.Network, opt => opt.MapFrom(d => d.Network))
                .ForMember(p => p.AccountAddress, opt => opt.MapFrom(d => d.AccountAddress))
                .ForMember(p => p.UserId, opt => opt.MapFrom(d => d.UserId))
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<Domain.Wallet.Entities.Transaction, TransactionDetails>()
                .ForMember(p => p.TokenAmount, opt => opt.MapFrom(d => d.TokenAmount))
                .ForMember(p => p.PayedAmount, opt => opt.MapFrom(d => d.PayedAmount))
                .ForMember(p => p.UserId, opt => opt.MapFrom(d => d.UserId))
                .ForMember(p => p.Currency, opt => opt.MapFrom(d => d.Currency))
                .ForMember(p => p.InitiatedAt, opt => opt.MapFrom(d => d.InitiatedAt))
                .ForMember(p => p.FinishedAt, opt => opt.MapFrom(d => d.FinishedAt))
                .ForMember(p => p.TokenId, opt => opt.MapFrom(d => d.TokenId))
                .ForMember(p => p.FinishedAt, opt => opt.MapFrom(d => d.FinishedAt))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}