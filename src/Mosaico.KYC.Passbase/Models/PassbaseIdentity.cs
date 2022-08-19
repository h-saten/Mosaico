namespace Mosaico.KYC.Passbase.Models
{
    public class PassbaseIdentity
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public PassbaseOwner Owner { get; set; }
        public float Score { get; set; }
        public long Created { get; set; }
        public long Updated { get; set; }
    }
}