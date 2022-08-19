namespace Mosaico.Application.Identity.Queries.GetPhoneNumberExistence
{
    public class GetPhoneNumberExistenceResponse
    {
        public bool Exist { get; set; }

        public GetPhoneNumberExistenceResponse()
        {
            Exist = false;
        }
    }
}