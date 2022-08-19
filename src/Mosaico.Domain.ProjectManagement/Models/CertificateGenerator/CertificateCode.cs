namespace Mosaico.Domain.ProjectManagement.Models.CertificateGenerator
{
    public class CertificateCode
    {
        private readonly int _investorSequenceNumber;
        private readonly int _purchaseTransactionsAmount;

        public CertificateCode(int purchaseTransactionsAmount, int investorSequenceNumber)
        {
            _purchaseTransactionsAmount = purchaseTransactionsAmount;
            _investorSequenceNumber = investorSequenceNumber;
        }

        public override string ToString()
        {
            var investorNumberLength = _investorSequenceNumber.ToString().Length;
        
            var codeFirstPart = "";
            for (int i = 0; i < 7-investorNumberLength; i++)
            {
                codeFirstPart += "0";
            }
        
            codeFirstPart += _investorSequenceNumber;
        
            return $"{codeFirstPart}/{_purchaseTransactionsAmount}";
        }
    }
}