namespace Mosaico.Application.Identity.Queries.GetKangaUser
{
    public class GetKangaUserQueryResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string KangaUserId { get; set; }
        public bool KangaKycVerified { get; set; }
    }
}