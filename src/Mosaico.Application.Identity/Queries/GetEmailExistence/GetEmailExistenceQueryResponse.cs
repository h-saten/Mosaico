namespace Mosaico.Application.Identity.Queries.GetEmailExistence
{
    public class GetEmailExistenceQueryResponse
    {
        public bool Exist { get; set; }

        public GetEmailExistenceQueryResponse()
        {
            Exist = false;
        }
    }
}