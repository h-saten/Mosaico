using AutoMapper;
using MongoDB.Bson;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Graph.Wallet
{
    public class WalletMapping : Profile
    {
        public WalletMapping()
        {
            CreateMap<Transaction, Entities.Transaction>()
                .ForMember(t => t.Id, opt => opt.MapFrom(t => new ObjectId(t.Id.ToString())))
                .ForMember(t => t.PaymentCurrency, opt => opt.MapFrom(t => t.PaymentCurrency.Ticker))
                .ForMember(t => t.Type, opt => opt.MapFrom(t => t.Type.Key))
                .ForMember(t => t.Status, opt => opt.MapFrom(s => s.Status.Key))
                .ForMember(t => t.User, opt => opt.Ignore());
        }
    }
}