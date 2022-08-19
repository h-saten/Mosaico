using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace KangaExchange.SDK.Abstractions
{
    public class SavePartnerResponseDto
    {
        public string Result { get; set; }
        public string BuyCode { get; set; }
        public string Code { get; set; }
    }

    public interface ISavePartnerParameters
    {
        public string IcoId { get; set; }
        public string IcoTicker { get; set; }
        public string Email { get; set; }
        public string Reflink { get; set; }
        public double CurrencyFeeRate { get; set; }
        public double TokensFeeRate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string KangaApiPrivateKey { get; set; }
        public string KangaApiPublicKey { get; set; }
    }
    
    public class PartnerCodesResponseDto
    {
        public string Result { get; set; }
        public List<PartnerCodeEntryDto> Codes { get; set; }
        public string Code { get; set; }
    }

    public class PartnerReportEntryDto
    {
        public string Ieo { get; set; }
        public string Bonus { get; set; }
        public string Currency { get; set; }
        public string Fee { get; set; }
        public int Cnt { get; set; }
    }
    
    public class PartnerReportResponseDto
    {
        public string Result { get; set; }
        public List<PartnerReportEntryDto> Report { get; set; }
        public string Code { get; set; }
    }

    public class PartnerCodeEntryDto
    {
        public string Ieo { get; set; }
        public string FeeRate { get; set; }
        public string BuyCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    
    public interface IKangaAffiliationApiClient
    {
        Task<IRestResponse<SavePartnerResponseDto>> SavePartnerAsync(ISavePartnerParameters parameters);
        Task<IRestResponse<PartnerCodesResponseDto>> PartnerCodesAsync(string partnerEmail);
        Task<IRestResponse<PartnerReportResponseDto>> PartnerReportAsync(string partnerEmail);
    }
}