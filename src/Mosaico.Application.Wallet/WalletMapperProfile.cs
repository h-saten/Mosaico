using System.Linq;
using AutoMapper;
using Mosaico.Application.Wallet.Commands.TokenManagement.CreateToken;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Integration.Blockchain.Ethereum.Models;

namespace Mosaico.Application.Wallet
{
    public class WalletMapperProfile : Profile
    {
        public WalletMapperProfile()
        {
            CreateMap<Transaction, TransactionDTO>()
                .ForMember(t => t.TransactionId, opt => opt.MapFrom(t => t.Id))
                .ForMember(p => p.Status, opt => opt.MapFrom(d => d.Status.Title));
            
            CreateMap<Transaction, CompanyTransactionDTO>()
                .ForMember(t => t.TransactionId, opt => opt.MapFrom(t => t.Id))
                .ForMember(p => p.TokenAmount, opt => opt.MapFrom(d => d.TokenAmount))
                .ForMember(p => p.PayedAmount, opt => opt.MapFrom(d => d.PayedAmount))
                .ForMember(p => p.PaymentProcessor, opt => opt.MapFrom(d => d.PaymentProcessor))
                .ForMember(p => p.Currency, opt => opt.MapFrom(d => d.Currency))
                .ForMember(p => p.Status, opt => opt.MapFrom(d => d.Status.Key))
                .ForMember(p => p.InitiatedAt, opt => opt.MapFrom(d => d.InitiatedAt))
                .ForMember(p => p.FinishedAt, opt => opt.MapFrom(d => d.FinishedAt))
                .ForMember(p => p.FailureReason, opt => opt.MapFrom(d => d.FailureReason))
                .ForMember(p => p.TransactionType, opt => opt.MapFrom(d => d.Type.Title))
                .ForMember(p => p.TransactionHash, opt => opt.MapFrom(d => d.TransactionHash))
                .ForMember(p => p.From, opt => opt.MapFrom(d => d.From))
                .ForMember(p => p.To, opt => opt.MapFrom(d => d.To))
                .ForMember(p => p.ToDisplayName, opt => opt.MapFrom(d => d.ToDisplayName))
                .ForMember(p => p.FromDisplayName, opt => opt.MapFrom(d => d.FromDisplayName))
                .ForMember(p => p.PaymentMethod, opt => opt.MapFrom(d => d.PaymentMethod))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Token, TokenDTO>()
                .ForMember(t => t.Type, opt => opt.MapFrom(t => t.Type.Title));
            CreateMap<ExternalExchange, ExternalExchangeDTO>();
            CreateMap<TokenToExternalExchange, TokenExternalExchangeDTO>();
            CreateMap<CreateTokenCommand, Token>()
                .ForMember(t => t.Name, opt => opt.MapFrom(t => t.Name))
                .ForMember(t => t.NameNormalized, opt => opt.MapFrom(t => t.Name.ToUpperInvariant()))
                .ForMember(t => t.SymbolNormalized, opt => opt.MapFrom(t => t.Symbol.ToUpperInvariant()))
                .ForMember(t => t.Network, opt => opt.MapFrom(t => t.Network))
                .ForMember(t => t.Symbol, opt => opt.MapFrom(t => t.Symbol))
                .ForMember(t => t.CompanyId, opt => opt.MapFrom(t => t.CompanyId))
                .ForMember(t => t.TotalSupply, opt => opt.MapFrom(t => t.InitialSupply))
                .ForMember(t => t.IsBurnable, opt => opt.MapFrom(t => t.IsBurnable))
                .ForMember(t => t.IsMintable, opt => opt.MapFrom(t => t.IsMintable));
            
            CreateMap<TokenDetails, Token>()
                .ForMember(t => t.Address, opt => opt.MapFrom(t => t.ContractAddress))
                .ForMember(t => t.Name, opt => opt.MapFrom(t => t.TokenName))
                .ForMember(t => t.NameNormalized, opt => opt.MapFrom(t => t.TokenName.ToUpperInvariant()))
                .ForMember(t => t.SymbolNormalized, opt => opt.MapFrom(t => t.Symbol.ToUpperInvariant()))
                .ForMember(t => t.Symbol, opt => opt.MapFrom(t => t.Symbol))
                .ForMember(t => t.TotalSupply, opt => opt.MapFrom(t => t.TotalSupply))
                .ForMember(t => t.IsBurnable, opt => opt.MapFrom(t => t.Burnable))
                .ForMember(t => t.IsMintable, opt => opt.MapFrom(t => t.Mintable));
            
            CreateMap<Vesting, VestingDTO>()
                .ForMember(v => v.Claimed, opt => opt.Ignore())
                .ForMember(v => v.Status, opt => opt.Ignore())
                .ForMember(v => v.TransactionCount, opt => opt.Ignore())
                .ForMember(v => v.PercentageCompleted, opt => opt.Ignore())
                ;
            
            CreateMap<VestingFund, VestingFundDTO>();

            CreateMap<CreateVestingFundDTO, VestingFund>()
                .ForMember(v => v.Days, opt => opt.MapFrom(d => d.Days))
                .ForMember(v => v.StartAt, opt => opt.MapFrom(d => d.StartAt))
                .ForAllOtherMembers(opt => opt.Ignore());
            
            CreateMap<Deflation, DeflationDTO>();
            
            CreateMap<PaymentCurrency, PaymentCurrencyDTO>()
                .ForMember(pc => pc.Network, opt => opt.MapFrom(t => t.Chain))
                .ForMember(pc => pc.Decimals, opt => opt.MapFrom(t => t.DecimalPlaces));
                
            CreateMap<ProjectBankPaymentDetails, ProjectBankPaymentDetailsDTO>()
                .ForMember(t => t.Reference, opt => opt.Ignore());
            
            CreateMap<NFT, NFTDTO>()
                .ForMember(d => d.Network, opt => opt.MapFrom(t => t.NFTCollection.Network))
                .ForMember(d => d.Name, opt => opt.MapFrom(t => t.NFTCollection.Name))
                .ForMember(d => d.Symbol, opt => opt.MapFrom(t => t.NFTCollection.Symbol))
                .ForMember(d => d.Address, opt => opt.MapFrom(t => t.NFTCollection.Address))
                .ForMember(d => d.Uri, opt => opt.MapFrom(t => t.Uri))
                .ForMember(d => d.OwnerAddress, opt => opt.MapFrom(t => t.OwnerAddress))
                .ForMember(d => d.TokenId, opt => opt.MapFrom(t => t.TokenId))
                .ForMember(d => d.NFTCollectionId, opt => opt.MapFrom(t => t.NFTCollectionId));
            
            CreateMap<Vault, VaultDTO>();
            CreateMap<Operation, OperationDTO>();
            CreateMap<SalesAgent, SalesAgentDTO>();
            CreateMap<StakingPair, StakingPairDTO>()
                .ForMember(t => t.Version, opt => opt.MapFrom(t => t.StakingVersion))
                .ForMember(t => t.StakingRegulation, opt => opt.Ignore())
                .ForMember(t => t.TermsAndConditionsUrl, opt => opt.Ignore())
                .ForMember(t => t.PaymentCurrencies,
                    opt => opt.MapFrom(t => t.PaymentCurrencies.Select(pc => pc.PaymentCurrency.Ticker)));
        }
    }
}